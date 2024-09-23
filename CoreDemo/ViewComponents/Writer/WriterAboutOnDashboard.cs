using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterAboutOnDashboard: ViewComponent
    {
        WriterManager wm = new WriterManager( new EfWriterRepository() );

        Context c = new Context();

        public IViewComponentResult Invoke()
        {
            //var user = await _userManager.FindByNameAsync( User.Identity.Name );

            var userName = User.Identity.Name;

            ViewBag.vx = userName;

            var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            ViewBag.userImage = c.Users.Where( x => x.UserName == userName ).Select( y => y.ImageUrl ).FirstOrDefault();

            var writerID = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            var values = wm.GetWriterByID( writerID );

            return View( values );
        }
    }
}
