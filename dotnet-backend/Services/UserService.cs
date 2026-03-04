using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using LoginApi.Data;
using LoginApi.DTOs;
using LoginApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LoginApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterDTO registerDTO)
    {
        var usernameExists = await _context.Users.AnyAsync(user => user.Username == registerDTO.Username);
        if (usernameExists)
        {
            return (false, "User already exists.");
        }

        var role = registerDTO.Role == "Admin" ? "Admin" : "User";

        var user = new User
        {
            Username = registerDTO.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, "Registration successful.");
    }

    public async Task<(bool IsSuccess, string Message, string Token)> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == loginDTO.Username);
        if (user is null)
        {
            return (false, "Invalid username or password.", string.Empty);
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
        if (!passwordIsValid)
        {
            return (false, "Invalid username or password.", string.Empty);
        }

        var token = GenerateJwtToken(user);
        return (true, "Login successful.", token);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var secret = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT secret key is missing.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
