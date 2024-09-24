using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentitySeedDbContext
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser() 
                {
                    DisplayName = "Heba Adel",
                    Email = "hebaadil@gmail.com",
                    UserName = "heba.adel",
                    PhoneNumber = "01001394765",
                };
                await userManager.CreateAsync(user , "P@$$w0rd"); 
            }
        }
    }
}
