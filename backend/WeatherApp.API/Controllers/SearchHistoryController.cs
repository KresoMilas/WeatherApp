using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchHistoryController : ControllerBase
{
    private readonly ISearchHistoryService _searchHistoryService;

    public SearchHistoryController(ISearchHistoryService searchHistoryService)
    {
        _searchHistoryService = searchHistoryService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSearchHistory()
    {
        var result = await _searchHistoryService.GetSearchHistoryAsync(User.FindFirstValue("email")!);
        return Ok(result);
    }
}