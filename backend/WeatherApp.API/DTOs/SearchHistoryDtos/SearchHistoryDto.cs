namespace WeatherApp.API.DTOs.SearchHistoryDtos;

public class SearchHistoryDto
{
    public string City { get; set; } = string.Empty;
    public string WeatherDescription { get; set; } = string.Empty;
    public DateTime SearchDateTime { get; set; }
}