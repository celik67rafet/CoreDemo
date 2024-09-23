using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminCommentController : Controller
	{
		CommentManager cm = new CommentManager( new EfCommentRepository() );
		public IActionResult Index()
		{
			var values = cm.TGetCommentsWithBlogs().OrderByDescending( y => y.CommentID ).ToList();

			return View( values );
		}

		[HttpPost]
		public IActionResult DeleteComment( int id)
		{

			if( id == 0 && id == null ) 
			{
				return BadRequest( new { errorMessage = "Bir sorun var, id boş veya null." } );
			}

			var comment = cm.TGetById(id);

			cm.TDelete( comment );

			return Json( new { message = "Başarılı!" } );

		}

		[HttpPost]
		public IActionResult ChangeCommentStatus( int id )
		{
			Console.WriteLine( id );

			if( id == 0 && id == null)
			{
				return BadRequest( new { errorMessage = "Bir sorun var, id boş veya null." } );
			}

			var comment = cm.TGetById(id);

			comment.CommentStatus = !comment.CommentStatus;

			cm.TUpdate( comment );

			return Json( new { message = "Başarılı" } );
		}
	}


}
