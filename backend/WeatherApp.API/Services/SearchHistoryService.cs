using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Models;
using WeatherApp.API.DTOs.SearchHistoryDtos;
using WeatherApp.API.Data;

namespace WeatherApp.API.Services;

public class SearchHistoryService : ISearchHistoryService
{
    private readonly AppDbContext _context;

    public SearchHistoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddSearchHistoryAsync(string userEmail, string cityName, string weatherCondition)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        var searchHistory = new SearchHistory
        {
            UserId = user.Id,
            CityName = cityName,
            WeatherCondition = weatherCondition,    
            SearchDateTime = DateTime.UtcNow
        };

        _context.SearchHistories.Add(searchHistory);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SearchHistoryDto>> GetSearchHistoryAsync(string userEmail)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        return await _context.SearchHistories
            .Where(sh => sh.UserId == user.Id)  
            .OrderByDescending(sh => sh.SearchDateTime)
            .Select(sh => new SearchHistoryDto
            {
                City = sh.CityName,
                SearchDateTime = sh.SearchDateTime,
                WeatherDescription = sh.WeatherCondition
            })
            .ToListAsync();
    }
}