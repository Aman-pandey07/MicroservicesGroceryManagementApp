using UserService.Dto;
using UserService.Models;

namespace UserService.Services
{
    public interface IAuthService
    {
        Task<UserDto> Register(UserregistrationDto dto);
        Task<UserDto> Login(UserLoginDto dto);
        Task<bool> UserExists(string email);
    }
}
