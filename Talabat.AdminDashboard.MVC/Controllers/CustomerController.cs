using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Talabat.AdminDashboard.MVC.ViewModels;
using Talabat.Core.Entities.Identity;

namespace Talabat.AdminDashboard.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public CustomerController(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager=userManager;
            this.roleManager=roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.Select(U => new UserViewModel {
                Username = U.UserName,
                DisplayName = U.DisplayName,
                Id = U.Id,
                Email = U.Email , 
                PhoneNumber = U.PhoneNumber,
                Roles = userManager.GetRolesAsync(U).Result,
            }).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var customer = await userManager.FindByIdAsync(id);
            var allRoles = await roleManager.Roles.ToListAsync();
            var ViewModel = new UserRoleViewModel()
            {
                UserId = customer.Id,
                Username=customer.DisplayName,
                Roles = allRoles.Select(R => new RoleViewModel
                {
                    Id = R.Id,
                    Name = R.Name,
                    IsSelected = userManager.IsInRoleAsync(customer, R.Name).Result
                }).ToList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(R => R == role.Name) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(R => R == role.Name) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
