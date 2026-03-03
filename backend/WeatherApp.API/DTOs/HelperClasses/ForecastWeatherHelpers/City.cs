namespace WeatherApp.API.DTOs.HelperClasses.ForecastWeatherHelpers;

using System.Text.Json.Serialization;

public class City
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
    
}