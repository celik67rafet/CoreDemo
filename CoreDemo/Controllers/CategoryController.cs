using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager( new EfCategoryRepository() );
        public IActionResult Index()
        {
            var values = cm.GetList();

            return View( values );
        }
    }
}
