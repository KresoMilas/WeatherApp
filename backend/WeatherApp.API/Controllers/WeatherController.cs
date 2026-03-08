using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ISearchHistoryService _searchHistoryService;

    public WeatherController(IWeatherService weatherService, ISearchHistoryService searchHistoryService)
    {
        _weatherService = weatherService;
        _searchHistoryService = searchHistoryService;
    }

    [HttpGet("current")]  
    public async Task<IActionResult> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)
    {
        try
        {
             var result = await _weatherService.GetCurrentWeatherAsync(lat, lon);
             return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("forecast")]  
    public async Task<IActionResult> GetForecast([FromQuery] string city)
    {
        try
        {
            var result = await _weatherService.Get5DayForecastAsync(city);
            if (User.Identity?.IsAuthenticated == true)
            {
                var userEmail = User.FindFirstValue("email");
                var condition = result.FiveDayForecastWeather
                                        .GroupBy(f => f.WeatherMain)
                                        .OrderByDescending(g => g.Count())
                                        .First().Key;
                await _searchHistoryService.AddSearchHistoryAsync(userEmail!, result.CityName, condition);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}