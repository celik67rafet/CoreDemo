using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterNavbarPartial:ViewComponent
    {
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;

            var user = c.Users.Where( x => x.UserName == userName ).FirstOrDefault();

            return View( user );
        }
    }
}
