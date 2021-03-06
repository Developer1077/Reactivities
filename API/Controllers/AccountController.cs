using API.DTOs;
using API.Services;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new AppResponse(StatusCodes.Status401Unauthorized,"Invalid Email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded) {
                return CreateUserObject(user);
            }
            return Unauthorized(new AppResponse(StatusCodes.Status401Unauthorized, "Invalid Email or password"));
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await _userManager.Users.AnyAsync(user => user.Email == registerDto.Email))
            {
                return BadRequest(new AppResponse(StatusCodes.Status400BadRequest, "Email taken"));
            }
            if (await _userManager.Users.AnyAsync(user => user.UserName == registerDto.Username))
            {
                return BadRequest(new AppResponse(StatusCodes.Status400BadRequest, "Username taken"));
            }

            var user = new AppUser
            {
                    DisplayName = registerDto.Username,
                    Email = registerDto.Email,
                    UserName = registerDto.Username                    
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return BadRequest(new AppResponse(StatusCodes.Status400BadRequest,"Problem registering user"));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser() {

            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }

        private UserDto CreateUserObject(AppUser user) {

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user), //You can't put confidential info in the token as it can be read.
                Username = user.UserName
            };
        }
    }
}
