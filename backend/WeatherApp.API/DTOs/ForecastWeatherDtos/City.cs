using System.Text.Json.Serialization;

namespace WeatherApp.API.DTOs.ForecastWeatherDtos;

public class City
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
    
}