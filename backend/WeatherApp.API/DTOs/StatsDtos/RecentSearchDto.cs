namespace WeatherApp.API.DTOs.StatsDtos;

public class RecentSearchDto
{
    public string CityName { get; set; } = string.Empty;
    public string WeatherCondition { get; set; } = string.Empty;
    public DateTime SearchDateTime { get; set; }
}
