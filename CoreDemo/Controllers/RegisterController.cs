using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
	[AllowAnonymous]
	public class RegisterController : Controller
	{
		WriterManager wm = new WriterManager( new EfWriterRepository() );

		[HttpGet]
		public IActionResult Index()
		{

			ViewBag.x = "Test Test";

			return View();
		}

		[HttpPost]
		public IActionResult Index( Writer writer )
		{
			WriterValidator wv = new WriterValidator();

			ValidationResult results = wv.Validate( writer );

			if( results.IsValid ) 
			{
				writer.WriterStatus = true;

				writer.WriterAbout = "test";

				wm.TAdd(writer);

				return RedirectToAction("Index", "Blog");
			}
			else
			{
				foreach( var item in results.Errors)
				{
					ModelState.AddModelError( item.PropertyName, item.ErrorMessage );
				}
			}

			return View();
		}


	}
}
