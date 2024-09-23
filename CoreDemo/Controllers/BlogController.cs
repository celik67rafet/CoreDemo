using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDemo.Controllers
{
	public class BlogController : Controller
	{
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());

		Context c = new Context();

        BlogManager bm = new BlogManager( new EfBlogRepository() );

		[AllowAnonymous]
		public IActionResult Index()
		{
			var values = bm.GetBlogListWithCategory();

			return View( values );
		}

		[AllowAnonymous]
		public IActionResult BlogReadAll(int id)
		{
			ViewBag.i = id;

			var values = bm.GetBlogById( id ).Where( x => x.BlogStatus == true ).OrderByDescending( y => y.BlogID );

			return View( values );
		}

        [Authorize(Roles = "Admin,Writer,Moderator")]
        public IActionResult BlogListByWriter() 
		{
			var userName = User.Identity.Name;

			var writerEmail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

			var writerId = c.Writers.Where( x => x.WriterMail == writerEmail ).Select( y => y.WriterID ).FirstOrDefault();

			var values = bm.GetListWithCategoryByWriter( writerId );

            return View( values );
		}

        [Authorize(Roles = "Admin,Writer,Moderator")]
        [HttpGet]
		public IActionResult BlogAdd()
		{

            List<SelectListItem> CategoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();

            ViewBag.cv = CategoryValues;

            return View();
		}

		[Authorize( Roles = "Admin,Writer,Moderator" )]
		[HttpPost]
		public IActionResult BlogAdd( Blog p )
		{
			BlogValidator bv = new BlogValidator();

			ValidationResult results = bv.Validate( p );

			var userName = User.Identity.Name;

			var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

			var writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

			if( results.IsValid )
			{
				p.BlogStatus = true;
				p.BlogCreateDate = DateTime.Parse( DateTime.Now.ToShortDateString() );
				p.WriterID = writerId;
				bm.TAdd( p );
				return RedirectToAction("BlogListByWriter", "Blog");
			}
			else
			{
				foreach ( var item in results.Errors ) 
				{
					ModelState.AddModelError( item.PropertyName, item.ErrorMessage );
				}
			}


			return View();
		}

        [Authorize(Roles = "Admin,Writer,Moderator")]
        public IActionResult DeleteBlog( int id)
		{
			var blogValue = bm.TGetById( id );

			bm.TDelete( blogValue );

			return RedirectToAction("BlogListByWriter","Blog");
		}

        [Authorize(Roles = "Admin,Writer,Moderator")]
        [HttpGet]
		public IActionResult EditBlog( int id )
		{
			var blogValue = bm.TGetById(id);

			List<SelectListItem> categoryvalues = (from x in cm.GetList()
												   select new SelectListItem
												   {
													   Text = x.CategoryName,
													   Value = x.CategoryID.ToString()
												   }).ToList();

			ViewBag.cv = categoryvalues;

			return View(blogValue);
		}

        [Authorize(Roles = "Admin,Writer,Moderator")]
        [HttpPost]
		public IActionResult EditBlog( Blog p )
		{
			var existingBlog = bm.TGetById( p.BlogID );

			p.WriterID = existingBlog.WriterID;
			p.BlogCreateDate = existingBlog.BlogCreateDate;

			bm.TUpdate( p );

			return RedirectToAction("BlogListByWriter","Blog");
		}
		
	}
}
