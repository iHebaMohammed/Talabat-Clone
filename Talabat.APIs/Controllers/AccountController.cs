using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;

        public AccountController(
            IMapper mapper,
            UserManager<AppUser> userManager , 
            SignInManager<AppUser> signInManager ,
            ITokenServices tokenServices)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized(new ApisResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password , false);
            if (!result.Succeeded) return Unauthorized(new ApisResponse(401));

            var userDTO = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateToken(user, _userManager)
            };
            return Ok(userDTO);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {

            if (CheckEmailExists(registerDTO.Email).Result.Value) 
                return BadRequest(new ApisValidationErrorResponse() { Errors = new[] { "This email is already in use" } });

            var user = new AppUser()
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email.Split('@')[0],
            };
            var result = await _userManager.CreateAsync(user , registerDTO.Password);
            if (!result.Succeeded) return BadRequest(new ApisResponse(400));
            var userDTO = new UserDTO() 
            {
                DisplayName = user.DisplayName, 
                Email = user.Email,
                Token =  await _tokenServices.CreateToken(user, _userManager)
            };
            return Ok(userDTO);
        }

        [Authorize]
        [CachedAttribute(600)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest(new ApisResponse(401));
            return Ok(new UserDTO()
            {
                DisplayName= user.DisplayName,
                Email  = user.Email,
                Token = await _tokenServices.CreateToken(user, _userManager)
            });
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO updatedAddress)
        {
            var mappedAddress = _mapper.Map<AddressDTO, Address>(updatedAddress);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            user.Address = mappedAddress;
            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded) return BadRequest(new ApisResponse(400 , "An error occured during updating the user adress"));

            var model = _mapper.Map<Address, AddressDTO>(user.Address);
            return Ok(model);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);
            var model = _mapper.Map<Address, AddressDTO>(user.Address);
            return Ok(model);
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return (await _userManager.FindByEmailAsync(email) != null);
        }
    }
}
