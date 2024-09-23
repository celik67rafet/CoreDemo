using System.ComponentModel.DataAnnotations;

namespace CoreDemo.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol Adı Boş Olamaz")]
        public string Name { get; set; }
    }
}
