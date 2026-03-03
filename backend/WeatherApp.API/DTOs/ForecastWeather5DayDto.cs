namespace WeatherApp.API.DTOs;

public class ForecastWeather5DayDto
{
    public string CityName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public List<ForecastWeatherDto> FiveDayForecastWeather { get; set; } = new List<ForecastWeatherDto>();
}