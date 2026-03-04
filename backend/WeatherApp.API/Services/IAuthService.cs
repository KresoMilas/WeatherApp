using WeatherApp.API.DTOs.AuthDtos;

namespace WeatherApp.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(AuthRequestDto registerRequest);
    Task<AuthResponseDto> LoginAsync(AuthRequestDto loginRequest);
}   