using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Admin
{
    public class AdminIndexWelcomeUser:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;

            using( Context c = new Context())
            {
                var userMail = c.Users.Where(u => u.UserName == userName).Select(u => u.Email).FirstOrDefault();

                ViewBag.writerName = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterName ).FirstOrDefault();
            }

            return View();
        }
    }
}
