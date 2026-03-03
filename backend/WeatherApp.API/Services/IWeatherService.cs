using WeatherApp.API.DTOs;

namespace WeatherApp.API.Services;
public interface IWeatherService
{
    Task<CurrentWeatherDto> GetCurrentWeatherAsync(double lat, double lon);
    Task<ForecastWeather5DayDto> Get5DayForecastAsync(string city);
}