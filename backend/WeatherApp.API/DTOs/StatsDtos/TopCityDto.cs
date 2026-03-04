namespace WeatherApp.API.DTOs.StatsDtos;

public class TopCityDto
{
    public string CityName { get; set; } = string.Empty;
    public int SearchCount { get; set; }
}
