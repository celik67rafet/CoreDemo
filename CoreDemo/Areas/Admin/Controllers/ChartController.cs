using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryChart() 
        {
            List<CategoryClass> list = new List<CategoryClass>();

            list.Add(new CategoryClass() { CategorysBlogCount = 10, CategoryName = "Teknoloji" });
            list.Add(new CategoryClass() { CategorysBlogCount = 12, CategoryName = "Yazılım" });
            list.Add(new CategoryClass() { CategorysBlogCount = 5, CategoryName = "Spor" });

            return Json( new { jsonList = list } );
        }
    }
}
