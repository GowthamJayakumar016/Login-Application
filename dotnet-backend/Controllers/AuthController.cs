using LoginApi.DTOs;
using LoginApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var result = await _userService.RegisterAsync(registerDTO);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var result = await _userService.LoginAsync(loginDTO);

        if (!result.IsSuccess)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new { message = result.Message, token = result.Token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            username = User.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value,
            role = User.Claims.FirstOrDefault(claim => claim.Type.Contains("role"))?.Value
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok(new { message = "Welcome to the Admin endpoint." });
    }

    [Authorize(Roles = "User")]
    [HttpGet("user-only")]
    public IActionResult UserOnly()
    {
        return Ok(new { message = "Welcome to the User endpoint." });
    }
}
