using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;
using System.Security.Claims;

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
        var result = await _weatherService.GetCurrentWeatherAsync(lat, lon);
        if (User.Identity?.IsAuthenticated == true)
        {
            var userEmail = User.FindFirstValue("email");
            await _searchHistoryService.AddSearchHistoryAsync(userEmail!, result.CityName, result.WeatherMain);
        }
        return Ok(result);
    }

    [HttpGet("forecast")]  
    public async Task<IActionResult> GetForecast([FromQuery] string city)
    {
        var result = await _weatherService.Get5DayForecastAsync(city);
        return Ok(result);
    }
}