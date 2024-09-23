using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Admin
{
    public class NavbarMessageNotification: ViewComponent
    {
        Message2Manager m2m = new Message2Manager( new EfMessage2Repository() );

        //private readonly UserManager<AppUser> _userManager;

        private readonly UserManager<AppUser> _userManager;

        public NavbarMessageNotification( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }
        public IViewComponentResult Invoke() 
        {
            Context c = new Context();
            
            var userMail = c.Users.Where( x => x.UserName == User.Identity.Name ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();
            
            var inbox = m2m.TGetInboxListByWriter( writerId );

                return View(inbox); 

        }
    }
}
