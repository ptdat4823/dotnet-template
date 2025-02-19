using Microsoft.AspNetCore.Mvc;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.DTOs;
using TMS.Domain.Entities;
using TMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace TMS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest(new { message = "Wrong email or password" });

            var res = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!res.Succeeded) return BadRequest(new { message = "Wrong email or password" });

            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { message = "Login successfully", userId = user.Id, token });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var res = await _userManager.CreateAsync(user, request.Password);
            if (!res.Succeeded) return BadRequest(res.Errors.Select(e => e.Description));

            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { message = "Signup successfully", userId = user.Id, token });
        }
    }
}
