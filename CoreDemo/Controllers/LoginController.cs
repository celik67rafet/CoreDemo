using CoreDemo.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
	[ AllowAnonymous ] // Proje bazında authorize ekleyince tüm sayfalar kapalı oldu ancak bunu eklersek eklediğimiz metod ve view açık olur.
	public class LoginController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;

        public LoginController( SignInManager<AppUser> signInManager )
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index( UserSignInViewModel p )
		{
			if( ModelState.IsValid ) 
			{

				var result = await _signInManager.PasswordSignInAsync( p.username, p.password, false, true );

				if( result.Succeeded ) 
				{
					return RedirectToAction("Index","Blog");
				}
				else
				{
					return RedirectToAction("Index","Login");
				}


			}
				return View();
		}

		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index","Login");
		}

		//[HttpPost]
		//public async Task<IActionResult> Index( Writer p )
		//{
			//Context c = new Context();

			//var dataValue = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);

			//if( dataValue != null) 
			//{
			//	HttpContext.Session.SetString("userName", p.WriterMail);
				
			//	return RedirectToAction("Index", "Writer");
			//}
			//else
			//{
			//	return View();
			//}


		//	Context c = new Context();

		//	var dataValue = c.Writers.FirstOrDefault( x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword );

		//	if( dataValue != null) 
		//	{
		//		var claims = new List<Claim>
		//		{
		//			new Claim( ClaimTypes.Name, p.WriterMail )
		//		};

		//		var userIdentity = new ClaimsIdentity( claims, "a" ); // burada bir değer göndermek gerekiyor neden? araştır...

		//		ClaimsPrincipal principal = new ClaimsPrincipal( userIdentity );

		//		await HttpContext.SignInAsync(principal);

		//		return RedirectToAction( "Index", "Writer" );
		//	}else 
		//	{
		//		return View();
		//	}
		//}
	}
}
