using WeatherApp.API.DTOs.SearchHistoryDtos;
using WeatherApp.API.DTOs.ForecastWeatherDtos;

namespace WeatherApp.API.Services;

public interface ISearchHistoryService
{
    Task AddSearchHistoryAsync(string userEmail, string cityName, string weatherCondition);
    Task<List<SearchHistoryDto>> GetSearchHistoryAsync(string userEmail);
}