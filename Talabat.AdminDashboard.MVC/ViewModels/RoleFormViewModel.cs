using System.ComponentModel.DataAnnotations;

namespace Talabat.AdminDashboard.MVC.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
