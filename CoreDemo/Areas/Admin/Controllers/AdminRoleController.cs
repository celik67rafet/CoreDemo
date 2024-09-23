using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize( Roles = "Admin" )]
    public class AdminRoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminRoleController( RoleManager<AppRole> roleManager, UserManager<AppUser> userManager )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var values = _roleManager.Roles.ToList();

            return View( values );
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole( int id)
        {
            Context c = new Context();

            ViewBag.NameSurname = c.Users.Where( x => x.Id == id ).Select( y => y.NameSurname ).FirstOrDefault();

            var user = _userManager.Users.Where( x => x.Id == id ).FirstOrDefault();

            var roles = _roleManager.Roles.ToList();

            TempData["UserId"] = user.Id;

            var userRoles = await _userManager.GetRolesAsync(user);

            List<RoleAssignViewModel> model = new List<RoleAssignViewModel>();

            foreach( var item in roles) 
            {
                RoleAssignViewModel m = new RoleAssignViewModel();

                m.RoleID = item.Id;
                m.Name = item.Name;
                m.Exists = userRoles.Contains( item.Name );
                model.Add( m );
            }

            return View( model );
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole( List<RoleAssignViewModel> model ) 
        {
            var userId = (int)TempData["UserId"];

            var user = _userManager.Users.FirstOrDefault( x => x.Id == userId );

            foreach( var item in model) 
            {
                if( item.Exists)
                {
                    await _userManager.AddToRoleAsync( user, item.Name );
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync( user, item.Name );
                }
            }

            return RedirectToAction("UserRoleList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole( RoleUpdateViewModel p ) 
        {
            var values = _roleManager.Roles.Where(x => x.Id == p.Id).FirstOrDefault();

            values.Name = p.Name;

            var result = await _roleManager.UpdateAsync(values);

            if( result.Succeeded ) 
            {
                return Json( new { message = "Güncelle Başarılı" } );
            }
            else
            {
                return Json( new { message = "Bir sorun çıktı" });
            }


        }

        public IActionResult UserRoleList()
        {
            var values = _userManager.Users.ToList();

            return View( values );
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole( int id)
        {
            var values = _roleManager.Roles.Where( x => x.Id == id ).FirstOrDefault();

            var result = await _roleManager.DeleteAsync(values);

            if( result.Succeeded ) { return Json(new { message = "Silme Başarılı" }); } else { return Json(new { message = "Sorun Çıktı Güncelleme Başarısız" }); }
        }

        [HttpPost]
        public async Task<IActionResult> AddRole( RoleViewModel model )
        {
            Console.WriteLine("model verisi aşağıda:");
            Console.WriteLine( model.Name );

            if (ModelState.IsValid)
            {

                AppRole role = new AppRole()
                {
                    Name = model.Name,
                };

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {

                    return Json(new { message = "Başarılı" });

                }

                foreach (var item in result.Errors)
                {

                    ModelState.AddModelError("", item.Description);

                }

            }

            return Json( new { message = "Başarısız" } );
        }
    }
}
