using TMS.Application.DTOs;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Services;
namespace TMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto { Id = user.Id, Email = user.Email };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto { Id = u.Id, Email = u.Email });
        }
    }
}
