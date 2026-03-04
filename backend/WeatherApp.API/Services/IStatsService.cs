using WeatherApp.API.DTOs.StatsDtos;

namespace WeatherApp.API.Services;

public interface IStatsService
{
    Task<UserStatsDto> GetUserStatsAsync(string userEmail);
}