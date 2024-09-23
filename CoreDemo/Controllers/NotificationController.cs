using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    public class NotificationController : Controller
    {
        NotificationManager nm = new NotificationManager( new EfNotificationRepository() );
        public IActionResult Index()
        {

            return View();
        }

        [AllowAnonymous]
        public IActionResult AllNotification()
        {
            var values = nm.GetList();

            return View( values );
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult NotificationDetail( int id )
        {
            var values = nm.TGetById( id );

            values.NotificationStatus = false;

            nm.TUpdate( values );

            return View( values );
        }
    }
}
