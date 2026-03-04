namespace WeatherApp.API.DTOs.HelperClasses.ForecastWeatherHelpers;

using System.Text.Json.Serialization;

public class ForecastWeatherResponse
{
   

    [JsonPropertyName("city")]
    public City City { get; set; } = new City();

    [JsonPropertyName("list")]
    public List<WeatherList> Weather { get; set; } = new List<WeatherList>();
}