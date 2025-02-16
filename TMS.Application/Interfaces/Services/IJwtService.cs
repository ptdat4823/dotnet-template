using TMS.Domain.Entities;

namespace TMS.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}