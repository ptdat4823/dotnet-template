using Microsoft.AspNetCore.Mvc;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.DTOs;
using TMS.Domain.Entities;

namespace TMS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || user.Password != request.Password)  // In production, use password hashing!
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new { message = "Login successful", userId = user.Id });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password  // In production, use password hashing!
            };

            await _userRepository.CreateUserAsync(user);

            return Ok(new { message = "Signup successful", userId = user.Id });
        }
    }
}
