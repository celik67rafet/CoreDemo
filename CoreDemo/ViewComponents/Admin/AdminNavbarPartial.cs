using BusinessLayer.Concrete;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Admin
{
    public class AdminNavbarPartial: ViewComponent
    {
        Context c = new Context();

        Message2Manager m2m = new Message2Manager( new EfMessage2Repository() );

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;

            ViewBag.UserName = userName;
            ViewBag.ImageUrl = c.Users.Where( x => x.UserName == userName ).Select( y => y.ImageUrl ).FirstOrDefault();

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var inboxList = m2m.TGetInboxListByWriter( writerId );

            var sendBoxList = m2m.TGetSendBoxListByWriter( writerId );

            var values = new MessageAllModelView() {
            
                Inbox = inboxList,
                SendBox = sendBoxList,
            };

            return View( values );
        }
    }
}
