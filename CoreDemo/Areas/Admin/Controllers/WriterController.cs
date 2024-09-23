using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WriterController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult GetWriterById( int writerId )
        {
            var findWriter =  writers.FirstOrDefault( x => x.Id == writerId );

                var jsonWriter = JsonConvert.SerializeObject( findWriter );

                return Json( jsonWriter );

        }

        [HttpPost]
        public IActionResult AddWriter( WriterClass w ) 
        {
            writers.Add( w );

            var jsonWriters = JsonConvert.SerializeObject( writers );

            return Json( jsonWriters );
        }


        [HttpPost]
        public IActionResult DeleteWriter( int writerId )

        {
            var writer = writers.FirstOrDefault(writers => writers.Id == writerId);

            writers.Remove( writer );

            string messageOk = writerId.ToString(); ;

            if( writer == null ) 
            {
                //messageOk = "Delete Problem";
            }
            else
            {
            }

            return Json( messageOk );
        }

        [HttpPost]
        public IActionResult UpdateWriter( WriterClass p )
        {
            var writer = writers.FirstOrDefault( x => x.Id == p.Id );

            writer.Name = p.Name;

            var jsonWriter = JsonConvert.SerializeObject( writer );

            return Json( jsonWriter );
        }

        [HttpGet]
        public IActionResult WriterList()
        {
            var jsonWriters = JsonConvert.SerializeObject( writers ); 

            return Json( jsonWriters );
        }

        public static List<WriterClass> writers = new List<WriterClass>
        {
            new WriterClass
            {
                Id = 1,
                Name = "Ayşe"
            },

            new WriterClass
            {
                Id= 2,
                Name = "John"
            },

            new WriterClass
            {
                Id= 3,
                Name = "Andrew"
            },

            new WriterClass
            {
                Id= 4,
                Name = "Felipe"
            }
        };
    }
}
