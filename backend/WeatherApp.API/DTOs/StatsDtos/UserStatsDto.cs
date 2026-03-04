namespace WeatherApp.API.DTOs.StatsDtos;

public class UserStatsDto
{
    public List<TopCityDto> TopCities { get; set; } = [];
    public List<RecentSearchDto> RecentSearches { get; set; } = [];
    public List<ConditionCountDto> ConditionDistribution { get; set; } = [];
}
