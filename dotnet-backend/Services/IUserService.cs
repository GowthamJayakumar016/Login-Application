using LoginApi.DTOs;

namespace LoginApi.Services;

public interface IUserService
{
    Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterDTO registerDTO);
    Task<(bool IsSuccess, string Message, string Token)> LoginAsync(LoginDTO loginDTO);
}
