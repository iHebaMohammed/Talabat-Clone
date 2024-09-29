using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.AdminDashboard.MVC.ViewModels;

namespace Talabat.AdminDashboard.MVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager=roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExsits = await roleManager.RoleExistsAsync(model.Name);
                if (!roleExsits)
                {
                    await roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is exsist!");
                    return View(nameof(Index) , await roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var mappedRole = new RoleViewModel() 
            {
                Name = role.Name,
                //Id = role.Id,
            };
            return View(mappedRole);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id , RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var roleExsits = await roleManager.RoleExistsAsync(model.Name);
                if (!roleExsits)
                {
                    var role = await roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is exsist!");
                    return RedirectToAction(nameof(Edit));
                }
            }
            return RedirectToAction(nameof(Edit));

        }
    }
}
