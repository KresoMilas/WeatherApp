using WeatherApp.API.DTOs.StatsDtos;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;

namespace WeatherApp.API.Services;

public class StatsService : IStatsService
{
    private readonly AppDbContext _appDbContext;

    public StatsService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UserStatsDto> GetUserStatsAsync(string userEmail)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null) throw new InvalidOperationException("User not found.");

       var topCities = await _appDbContext.SearchHistories
        .Where(sh => sh.UserId == user.Id)
        .GroupBy(sh => sh.CityName)
        .Select(g => new TopCityDto { CityName = g.Key, SearchCount = g.Count() })
        .OrderByDescending(c => c.SearchCount)
        .Take(3)
        .ToListAsync();

        var recentSearches = await _appDbContext.SearchHistories
        .Where(sh => sh.UserId == user.Id)
        .OrderByDescending(sh => sh.SearchDateTime)
        .Take(3)
        .Select(sh => new RecentSearchDto
        {
            CityName = sh.CityName,
            WeatherCondition = sh.WeatherCondition,
            SearchDateTime = sh.SearchDateTime
        })
        .ToListAsync();

        var conditionDistribution = await _appDbContext.SearchHistories
            .Where(sh => sh.UserId == user.Id)
            .GroupBy(sh => sh.WeatherCondition)
            .Select(g => new ConditionCountDto { Condition = g.Key, Count = g.Count() })
            .ToListAsync();

        return new UserStatsDto
        {
            TopCities = topCities,
            RecentSearches = recentSearches,
            ConditionDistribution = conditionDistribution
        };
    }
}