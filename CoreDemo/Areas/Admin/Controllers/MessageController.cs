using BusinessLayer.Concrete;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessageController : Controller
    {
        Context c = new Context();

        Message2Manager m2m = new Message2Manager( new EfMessage2Repository() );
        public IActionResult Inbox()
        {
            var userName = User.Identity.Name;

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            var writerID = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var inbox = m2m.TGetInboxListByWriter( writerID ).OrderByDescending(y => y.MessageID).ToList();
            
            var sendbox = m2m.TGetSendBoxListByWriter( writerID ).OrderByDescending(y => y.MessageID).ToList();

            var values = new MessageAllModelView()
            {
                Inbox = inbox,
                SendBox = sendbox,
            };

            return View( values );
        }

        [HttpGet]
        public IActionResult MessageDetails( int id ) 
        {
            var userMail = c.Users.Where( x => x.UserName == User.Identity.Name ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var message = m2m.TGetInboxListByWriter(writerId).Where( x => x.MessageID == id ).FirstOrDefault();

            Console.WriteLine("yazsana mk");

            if( message.MessageID == null)
            {
                Console.WriteLine("null geliyor");
            }
            else
            {
                Console.WriteLine(message.SenderUser.WriterName);
            }

            return View( message );
        }

        //[HttpPost]
        //public IActionResult SendMessage(Message2 p)
        //{
        //    Console.WriteLine("==========PARAMETRELER==========");
        //    Console.WriteLine(" Alıcı: " + p.ReceiverID);
        //    Console.WriteLine(" Gönderici: " + p.SenderID);
        //    Console.WriteLine(" Konu: " + p.Subject);
        //    Console.WriteLine(" Mesaj: " + p.MessageDetails);
        //    Console.WriteLine("==========PARAMETRELER==========");

        //    var username = User.Identity.Name;
        //    var usermail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();

        //    var writerId = c.Writers.Where(x => x.WriterMail == usermail).Select(y => y.WriterID).FirstOrDefault();

        //    p.SenderID = writerId;
        //    p.MessageStatus = true;
        //    p.MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        //    m2m.TAdd(p);

        //    return RedirectToAction("SendBox");
        //}

        [HttpGet]
        public IActionResult SendMessage() 
        {
            var userName = User.Identity.Name;

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            var values = c.Writers.Where( x => x.WriterMail != userMail ).ToList();

            return Json( values );
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message2 p ) {

            Console.WriteLine( "MessageDetails: " + p.MessageDetails );
            Console.WriteLine( "MessageDetails: " + p.Subject );
            Console.WriteLine( "MessageDetails: " + p.ReceiverID );

            var userName = User.Identity.Name;
            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();
            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            p.MessageStatus = true;
            p.VisibleInSandBox = true;
            p.MessageDate = DateTime.Parse( DateTime.Now.ToShortDateString() );
            p.SenderID = writerId;

            if ( p.ReceiverID != null || p.ReceiverID != 0) 
            {
                m2m.TAdd(p);
            }
            else
            {
                return Json( new { Errormessage = "Alıcı Seçimi Yapmadınız!" } );
            }

            return Json( new { message = "Mesaj Başarıyla Gönderildi" } );
        }

        public IActionResult MessageTrashBox() 
        {
            var userName = User.Identity.Name;

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var inbox = m2m.TGetInboxListByWriter( writerId ).OrderByDescending(y => y.MessageID).ToList();
            var sendbox = m2m.TGetSendBoxListByWriter( writerId ).OrderByDescending(y => y.MessageID).ToList();

            var values = new MessageAllModelView() { Inbox = inbox , SendBox = sendbox };   

            return View( values );
        }


        public IActionResult SendBox()
        {
            var userName = User.Identity.Name;

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var sendbox = m2m.TGetSendBoxListByWriter( writerId ).OrderByDescending( y => y.MessageID ).ToList();

            var inbox = m2m.TGetInboxListByWriter(writerId).OrderByDescending(y => y.MessageID).ToList();

            var values = new MessageAllModelView() 
            {
                Inbox = inbox,
                SendBox = sendbox
            };

            return View( values );

        }

        [HttpPost]
        public IActionResult DeleteFromSendBox([FromBody] List<int> ids)
        {
            if( ids == null)
            {
                return BadRequest( new { ErrorMessage = "Veri alınamadı, lütfen tekrar deneyiniz." } );
            }

            foreach( var id in ids)
            {
                var message = m2m.TGetById( id );

                message.VisibleInSandBox = false;

                m2m.TUpdate( message );
            }

            return Json( new { message = "Seçilen mesajlar silindi" } );
        }

        [HttpPost]
        public IActionResult DeleteMessages([FromBody] List<int> ids)
        {
            if( ids == null)
            {
                return BadRequest( new { ErrorMessage = "Veri alınamadı, lütfen tekrar deneyiniz." } );
            }

            foreach( var id in ids)
            {
                var message = m2m.TGetById( id );

                m2m.TDelete( message );
            }

            return Json( new { message = "Seçilen mesajlar silindi" } );
        }

        [HttpPost]
        public IActionResult TestMessages([FromBody] List<int> ids)
        {
            if (ids == null)
            {
                return BadRequest(new { Errormessage = "Veri alınamadı, lütfen tekrar deneyin." });
            }

            foreach (var id in ids)
            {
                Console.WriteLine(id.ToString());

                var message =  m2m.TGetById( id );

                message.MessageStatus = !message.MessageStatus;

                m2m.TUpdate( message );

            }

            return Json(new { message = "Mesajların durumu güncellendi." });
        }

    }
}
