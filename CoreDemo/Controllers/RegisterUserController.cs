using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        WriterManager wm = new WriterManager( new EfWriterRepository() );

        public RegisterUserController( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index( UserSignUpViewModel p )
        {
            if( ModelState.IsValid ) 
            {
                AppUser user = new AppUser() 
                {
                    Email = p.Mail,
                    UserName = p.UserName,
                    NameSurname = p.NameSurname,
                };

                var result = await _userManager.CreateAsync( user , p.Password );

                if( result.Succeeded ) 
                {
                    wm.TAdd( new Writer
                    {

                        WriterMail = user.Email,
                        WriterName = user.NameSurname,
                        WriterAbout = "Empty",
                        WriterStatus = true,
                        WriterPassword = "nan"

                    } );

                    return RedirectToAction("Index","Login");   
                }else 
                {
                    foreach( var item in result.Errors )
                    {
                        ModelState.AddModelError( "", item.Description );
                        Console.WriteLine( item.Description );
                    }
                }
            }

            return View( p );
        }
    }
}
