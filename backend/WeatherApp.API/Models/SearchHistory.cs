namespace WeatherApp.API.Models;

public class SearchHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime SearchDateTime { get; set; } = DateTime.UtcNow;
    public string CityName { get; set; } = string.Empty;
    public string WeatherCondition { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}