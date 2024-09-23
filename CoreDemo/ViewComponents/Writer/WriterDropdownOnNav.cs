using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterDropdownOnNav:ViewComponent
    {
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;

            var userImageUrl = c.Users.Where( x => x.UserName == userName ).Select( y => y.ImageUrl ).FirstOrDefault();

            ViewBag.imageUrl = userImageUrl;

            return View();
        }
    }
}
