using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace CoreDemo.Controllers
{
	[AllowAnonymous]
	public class AboutController : Controller
	{
		AboutManager abm = new AboutManager(new EfAboutRepository());

		public IActionResult Index()
		{
			var values = abm.GetList();

			return View( values );
		}

		public PartialViewResult SocialMediaAbout()
		{

			return PartialView();
		}
	}
}
