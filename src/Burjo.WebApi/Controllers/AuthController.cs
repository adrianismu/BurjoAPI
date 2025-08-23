using Microsoft.AspNetCore.Mvc;
using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;

namespace Burjo.WebApi.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            
            if (result == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Registration failed", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Login failed", error = ex.Message });
        }
    }
}
