using LoginApi.DTOs;

namespace LoginApi.Services;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(RegisterDto registerDto);
    Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto loginDto);
}
