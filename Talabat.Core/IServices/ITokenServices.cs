using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.IServices
{
    public interface ITokenServices
    {
        Task<string> CreateToken(AppUser user  , UserManager<AppUser> userManager);
    }
}
