using System.Text.Json.Serialization;

namespace WeatherApp.API.DTOs.ForecastWeatherDtos;

public class ForecastWeatherResponse
{
    [JsonPropertyName("city")]
    public City City { get; set; } = new City();

    [JsonPropertyName("list")]
    public List<WeatherList> Weather { get; set; } = new List<WeatherList>();
}