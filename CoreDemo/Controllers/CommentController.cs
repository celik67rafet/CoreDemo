using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
	public class CommentController : Controller
	{
		CommentManager cm = new CommentManager( new EfCommentRepository() );
		public IActionResult Index()
		{
			return View();
		}

        [AllowAnonymous]
        [HttpGet]
		public PartialViewResult PartialAddComment()
		{
			return PartialView();
		}


		[Authorize( Roles = "Member,Writer,Admin,Moderator" )]
		[HttpPost]
		public IActionResult PartialAddComment( Comment p, int BlogID )
		{
			p.CommentDate = DateTime.Parse( DateTime.Now.ToShortDateString() );
			p.CommentStatus = true;
			p.BlogID = BlogID;
			p.BlogScore = 7;

			cm.TAdd( p );

			return RedirectToAction("BlogReadAll","Blog",new { id = BlogID });
		}

		[AllowAnonymous]
		public PartialViewResult CommentListByBlog( int id )
		{
			var results = cm.GetList( id );

			return PartialView( results );
		}
	}
}
