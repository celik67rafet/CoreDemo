using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
	public class WriterController : Controller
	{
		WriterManager wm = new WriterManager( new EfWriterRepository() );

		private readonly UserManager<AppUser> _userManager;

		Context c = new Context();
        public WriterController( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }

		public IActionResult Index()
		{
			var userMail = User.Identity.Name;

			var writerName = c.Writers.Where( x => x.WriterMail == userMail ).Select( x => x.WriterName ).FirstOrDefault();

			ViewBag.UserMail = userMail;
			ViewBag.text = "test";
			ViewBag.writerName = writerName;	

			return View();
		}

		public async Task<IActionResult> WriterProfile()
		{
			var values = await _userManager.FindByNameAsync( User.Identity.Name );

			return View( values ); 
		}

		public IActionResult WriterMail()
		{
			return View();
		}

		public IActionResult Test() 
		{
			return View();
		}

		public PartialViewResult WriterNavbarPartial()
		{
            var userName = User.Identity.Name;

            var user = c.Users.Where(x => x.UserName == userName).FirstOrDefault();

            return PartialView( user );
		}

		public PartialViewResult WriterFooterPartial()
		{
			return PartialView();
		}

		[HttpGet]
		public async Task<IActionResult> WriterEditProfile()
		{
			var values = await _userManager.FindByNameAsync( User.Identity.Name );

			UserUpdateViewModel model = new UserUpdateViewModel();

			Console.WriteLine("**********BAŞLANGIÇTA*************");
			Console.WriteLine( values.UserName );
			Console.WriteLine( values.Email );
			Console.WriteLine( values.NameSurname );
			Console.WriteLine( values.ImageUrl );
			Console.WriteLine("**********BAŞLANGIÇTA*************");

			model.namesurname = values.NameSurname; 
			model.imageurl = values.ImageUrl;
			model.mail = values.Email;
			model.username = values.UserName;

			return View( model );

            // ==================== YÖNTEM 2 BAŞLANGIÇ. ====================================

            //Context c = new Context();

            //var username = User.Identity.Name;

            //var usermail = c.Users.Where( x => x.UserName == username ).Select( y => y.Email ).FirstOrDefault();

            //UserManager userManager = new UserManager( new EfUserRepository() );

            //var id = c.Users.Where( x => x.Email == usermail ).Select( y => y.Id ).FirstOrDefault();

            //var values = userManager.TGetById( id );

            //return View( values );

            // ==================== YÖNTEM 2 BİTİŞ ====================================


            // ==================== YÖNTEM 1 BAŞLANGIÇ. ====================================

            //int writerId;

            //using( Context c = new Context())
            //{
            //	var userName = User.Identity.Name;

            //	var userMail = c.Users.Where( x => x.UserName == userName ).Select( y => y.Email ).FirstOrDefault();

            //	writerId = c.Writers.Where( x => x.WriterMail == userMail ).Select( y => y.WriterID ).FirstOrDefault();

            //}

            //var writerValues = wm.TGetById( writerId );

            //return View( writerValues );

            // ==================== YÖNTEM 1 BİTİŞ. ====================================

        }

		[HttpPost]
		public async Task<IActionResult> WriterEditProfile( UserUpdateViewModel model )
		{
			var values = await _userManager.FindByNameAsync( User.Identity.Name );

			Console.WriteLine("**********SUBMIT EDİNCE*******************");
			Console.WriteLine("**********VALUES*******************");
			Console.WriteLine( values.NameSurname );
			Console.WriteLine( values.Email );
			Console.WriteLine( values.UserName );
			Console.WriteLine( values.ImageUrl );
			Console.WriteLine("**********MODELS*******************");
			Console.WriteLine( model.namesurname );
			Console.WriteLine( model.mail );
			Console.WriteLine( model.username );
			Console.WriteLine( model.imageurl );
            Console.WriteLine("**********SUBMIT EDİNCE*******************");

            values.Email = model.mail;
			values.NameSurname = model.namesurname;
			values.UserName = model.username;
			if( model.isChangePassword)
			{
				values.PasswordHash = _userManager.PasswordHasher.HashPassword(values, model.password);
				Console.WriteLine("işaretli");
			}
			else
			{
				Console.WriteLine("işaretlenmedi");
			}

			
			if( model.WriterImage == null)
			{
				Console.WriteLine("Adam yükleme yapmayacakmış resim böyle kalacak");
			}
			else
			{
				Console.WriteLine("Önce eski resimi sil ve yenisini yükle");

				if( !string.IsNullOrEmpty( values.ImageUrl ))
				{
					var oldImagePath = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", values.ImageUrl );

					if( System.IO.File.Exists( oldImagePath ))
					{
						System.IO.File.Delete( oldImagePath );	
					}
				}
				
					var extension = Path.GetExtension( model.WriterImage.FileName );
					var newImageName = Guid.NewGuid().ToString() + extension;
					var location = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", newImageName );

					using ( var stream = new FileStream( location, FileMode.Create ))
					{
						model.WriterImage.CopyTo( stream );
					}

                    values.ImageUrl = newImageName;
			}

			var result = await _userManager.UpdateAsync( values );

            if (result.Succeeded)
            {
                // Writer nesnesini güncellemek için doğru işlemi yaptığınızdan emin olun
                var writer = c.Writers.FirstOrDefault(x => x.WriterMail == values.Email);

                if (writer != null)
                {
                    writer.WriterName = values.NameSurname;
                    writer.WriterImage = values.ImageUrl; // Güncellenmiş resmi kullan
                    wm.TUpdate(writer);
                }

                wm.TUpdate( writer );

                return RedirectToAction("Index", "Dashboard");
            }

            // Eğer güncelleme başarısızsa, hata mesajı ile sayfayı geri döndürebilirsiniz
            ModelState.AddModelError("WriterImage", "Profil güncellenemedi.");
            return View(model);

            // ================= YÖNTEM 1 BAŞLANGIÇ =====================================

            //WriterValidator wl = new WriterValidator();

            //ValidationResult results = wl.Validate( p );

            //if ( results.IsValid ) 
            //{
            //	wm.TUpdate( p );

            //	return RedirectToAction("Index", "Dashboard");
            //}
            //else
            //{
            //	foreach( var item in results.Errors)
            //	{
            //		ModelState.AddModelError( item.PropertyName, item.ErrorMessage );
            //	}
            //return View();
            //}

            // ================= YÖNTEM 1 BİTİŞ =====================================


        }

		[HttpGet]
		public IActionResult WriterAdd()
		{


			return View();
		}

		[HttpPost]
		public IActionResult WriterAdd( AddProfileImage p )
		{
			Writer writer = new Writer();
			
			if( p.WriterImage != null )
			{
				var extension = Path.GetExtension(p.WriterImage.FileName);

				var newImageName = Guid.NewGuid() + extension;

				var location = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", newImageName );

				var stream = new FileStream(location, FileMode.Create);

				p.WriterImage.CopyTo( stream );

				writer.WriterImage = newImageName;
			}

			writer.WriterName = p.WriterName;
			writer.WriterMail = p.WriterMail;
			writer.WriterStatus = true;
			writer.WriterAbout = p.WriterAbout;
			writer.WriterPassword = p.WriterPassword;

			wm.TAdd( writer );

			return View();
		}

		public IActionResult WriterMessages()
		{
			return View();
		}
	}
}
