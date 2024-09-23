using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    [Authorize( Roles = "Writer,Moderator,Admin" )]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            int writerId;



            using ( Context c = new Context() )
            {
                var userName = User.Identity.Name;
                
                var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

                writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();
            }

            CategoryManager cm = new CategoryManager( new EfCategoryRepository() );

            BlogManager bm = new BlogManager( new EfBlogRepository() );

            var categoryAverages = cm.AllCategoryAveragesWithCategories();

            foreach ( var item in categoryAverages ) {
            
                Console.WriteLine( "Kategori İsmi: " + item.Category.CategoryName + "Kategori Puanı: " + item.AverageRayting );

            };

            ViewBag.v1 = bm.TGetBlogsCount().ToString();

            ViewBag.v2 = bm.TGetBlogsCountByWriter(writerId).ToString();

            ViewBag.v3 = cm.TGetCategoriesCount().ToString();

            return View();
        }

        public IActionResult HelloWorld()
        {
            return Content("Hello World!");
        }
    }
}
