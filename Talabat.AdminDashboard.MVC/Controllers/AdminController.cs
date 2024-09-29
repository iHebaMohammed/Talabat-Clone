using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Identity;

namespace Talabat.AdminDashboard.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AdminController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) 
            {
                ModelState.AddModelError("Email" , "Email is not exsist!");
                return View(loginDTO);
            }
            var result = await signInManager.CheckPasswordSignInAsync(user , loginDTO.Password , false);
            if (result.Succeeded && await userManager.IsInRoleAsync(user , "Admin"))
                return RedirectToAction("Index" , "Role");
            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
