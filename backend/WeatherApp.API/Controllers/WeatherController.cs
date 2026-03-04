using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("current")]  
    public async Task<IActionResult> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)
    {
        var result = await _weatherService.GetCurrentWeatherAsync(lat, lon);
        return Ok(result);
    }

    [HttpGet("forecast")]  
    public async Task<IActionResult> GetForecast([FromQuery] string city)
    {
        var result = await _weatherService.Get5DayForecastAsync(city);
        return Ok(result);
    }
}