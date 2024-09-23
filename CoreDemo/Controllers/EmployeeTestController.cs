using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CoreDemo.Controllers
{
    [Authorize( Roles = "Admin" )]
    public class EmployeeTestController : Controller
    {

        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:7177/api/Default");

            var jsonString = await responseMessage.Content.ReadAsStringAsync();

            var values = JsonConvert.DeserializeObject<List<Class1>>(jsonString);

            return View( values );
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee( int id )
        {
            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:7177/api/Default/" + id);

            if( responseMessage.IsSuccessStatusCode ) 
            {
                var jsonEmployee = await responseMessage.Content.ReadAsStringAsync();

                Console.WriteLine( jsonEmployee );

                var values = JsonConvert.DeserializeObject<Class1>(jsonEmployee);

                return View( values );
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee( Class1 p )
        {
            var httpClient = new HttpClient();

            var jsonEmployee = JsonConvert.SerializeObject( p );

            var content = new StringContent( jsonEmployee , Encoding.UTF8, "application/json" );

            var responseMessage = await httpClient.PutAsync("https://localhost:7177/api/Default/" , content);

            if( responseMessage.IsSuccessStatusCode ) 
            {
                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı oldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Ekleme İşlemi Başarısız Oldu.";
                return View( p );
            }
        }

        public async Task<IActionResult> DeleteEmployee( int id )
        {
            var httpClient = new HttpClient();

            var responseMessage = await httpClient.DeleteAsync("https://localhost:7177/api/Default/" + id );

            if( responseMessage.IsSuccessStatusCode ) 
            {
                // Başarılı mesajını TempData'ya ekle
                TempData["SuccessMessage"] = "Silme işlemi başarılı oldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Silme işlemi başarısız oldu.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> AddEmployee( Class1 p )
        {
            var httpClient = new HttpClient();

            var jsonEmployee = JsonConvert.SerializeObject( p );

            StringContent content = new StringContent( jsonEmployee, Encoding.UTF8, "application/json" );

            var responseMessage = await httpClient.PostAsync("https://localhost:7177/api/Default", content);

            if( responseMessage.IsSuccessStatusCode ) 
            {
                TempData["SuccessMessage"] = "Ekleme İşlemi Başarılı.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Ekleme İşlemi Başarısız Oldu.";

                return View( p );
            }


        }

        public class Class1 {

            public int Id { get; set; }
            public string Name { get; set; }

        }
    }
}
