using Microsoft.AspNetCore.Mvc;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.DTOs;
using TMS.Domain.Entities;
using TMS.Application.Interfaces.Services;

namespace TMS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { message = "Wrong email or password" });
            }

            var token = _jwtService.GenerateJwtToken(user);

            return Ok(new { message = "Login successful", userId = user.Id, token });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword
            };

            await _userRepository.CreateUserAsync(user);
            var token = _jwtService.GenerateJwtToken(user);

            return Ok(new { message = "Signup successful", userId = user.Id, token });
        }
    }
}
