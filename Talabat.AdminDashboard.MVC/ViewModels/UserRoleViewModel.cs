namespace Talabat.AdminDashboard.MVC.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
