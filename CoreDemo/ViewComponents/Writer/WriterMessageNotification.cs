using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification:ViewComponent
    {
        Message2Manager mm = new Message2Manager( new EfMessage2Repository() );

        Context c = new Context();

        public IViewComponentResult Invoke()
        {
            var username = User.Identity.Name;

            var userMail = c.Users.Where( x => x.UserName == username ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var values = mm.TGetInboxListByWriter( writerId );
            var valuesUnread = values.Where( x => x.MessageStatus == true ).ToList();
            var valuesRead = values.Where( x => x.MessageStatus == false ).ToList();

            ViewBag.unreadCount = valuesUnread.Count();
            ViewBag.readCount = valuesRead.Count();
            ViewBag.totalCount = values.Count();

            return View( valuesUnread );
        }
    }
}
