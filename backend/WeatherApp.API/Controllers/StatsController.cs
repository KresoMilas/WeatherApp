using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserStats()
    {
        var result = await _statsService.GetUserStatsAsync(User.FindFirstValue("email")!);
        return Ok(result);
    }
}