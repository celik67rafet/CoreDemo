using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBlogController : Controller
    {
            BlogManager bm = new BlogManager( new EfBlogRepository() );
        public IActionResult Index() {

            var values = bm.GetBlogListWithCategoryAndWriter();
            
            return View( values ); 
        }

        [HttpGet]
        public IActionResult BlogStatusChanger(int id )
        {
            var blog = bm.GetBlogById( id ).FirstOrDefault();

            Console.WriteLine( id );

            if( blog != null ) 
            {
                blog.BlogStatus = !blog.BlogStatus;

                bm.TUpdate(blog);
            }
            else
            {
                return Json( new { message = "İşlem başarısız" } );
            }

            return Json( new { message = "İşlem Başarılı" } );
        }
    }
}
