namespace WeatherApp.API.DTOs.ForecastWeatherDtos;

public class ForecastWeather5DayDto
{
    public string CityName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public List<ForecastWeatherDto> FiveDayForecastWeather { get; set; } = [];
}