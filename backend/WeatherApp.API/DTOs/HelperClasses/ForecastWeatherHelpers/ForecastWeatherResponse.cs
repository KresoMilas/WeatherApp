namespace WeatherApp.API.DTOs.HelperClasses;

using System.Text.Json.Serialization;

public class ForecastWeatherResponse
{
    public long Dt { get; set; }

    [JsonPropertyName("city")]
    public City City { get; set; } = new City();

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

        [JsonPropertyName("list")]
    public List<WeatherList> Weather { get; set; } = new List<WeatherList>();
}