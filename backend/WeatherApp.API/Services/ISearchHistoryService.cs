using WeatherApp.API.DTOs.SearchHistoryDtos;

namespace WeatherApp.API.Services;

public interface ISearchHistoryService
{
    Task AddSearchHistoryAsync(string userEmail, string cityName, string weatherCondition);
    Task<List<SearchHistoryDto>> GetSearchHistoryAsync(string userEmail);
}