using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;
using WeatherApp.API.DTOs.AuthDtos;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]  
    public async Task<IActionResult> Login([FromBody] AuthRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(ex.Message); 
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequestDto registerRequest)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerRequest);
            return StatusCode(201, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); 
        }
    }


}