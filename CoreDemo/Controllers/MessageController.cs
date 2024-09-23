using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    [Authorize( Roles = "Admin,Moderator,Writer" )]
    public class MessageController : Controller
    {
        Message2Manager m2m = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendBox()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();
            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();
            var values = m2m.TGetSendBoxListByWriter( writerId );

            return View( values );
        }

        public IActionResult Inbox()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();
            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();
            var values = m2m.TGetInboxListByWriter( writerId );


            return View( values );
        }

        [HttpGet]
        public IActionResult DeleteMessage( int messageId )
        {
            Console.WriteLine( messageId );

            var value = m2m.TGetById( messageId );

            m2m.TDelete( value );

            return RedirectToAction("Inbox");
        }

        [HttpGet]
        public IActionResult SendMessage()
        {
            var userName = User.Identity.Name;

            var usermail = c.Users.Where(x => x.UserName == userName).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == usermail ).Select( y => y.WriterID ).FirstOrDefault();

            var values = c.Writers.ToList();

            SendMessageViewModel model = new SendMessageViewModel()
            {
                writers = values,
                writerId = writerId,
            };

            return View( model );
        }


        [HttpPost]
        public IActionResult SendMessage( Message2 p )
        {
            Console.WriteLine("==========PARAMETRELER==========");
            Console.WriteLine( " Alıcı: " + p.ReceiverID);
            Console.WriteLine( " Gönderici: " + p.SenderID);
            Console.WriteLine( " Konu: " + p.Subject);
            Console.WriteLine( " Mesaj: " + p.MessageDetails);
            Console.WriteLine("==========PARAMETRELER==========");

            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();

            var writerId = c.Writers.Where(x => x.WriterMail == usermail).Select(y => y.WriterID).FirstOrDefault();

            p.SenderID = writerId;
            p.MessageStatus = true;
            p.MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            m2m.TAdd(p);

            return RedirectToAction("SendBox");
        }

        [HttpGet]
        public IActionResult MessageDetailsFromInbox( int id )
        {
            var value = m2m.TGetById( id );

            value.MessageStatus = false;

            value.VisibleInSandBox = true;

            m2m.TUpdate( value );

            return View( value );
        }

        [HttpGet]
        public IActionResult MessageDetailsFromSendBox(int id)
        {
            var value = m2m.TGetById(id);

            m2m.TUpdate(value);

            return View(value);
        }
    }
}
