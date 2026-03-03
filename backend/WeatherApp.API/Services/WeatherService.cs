using WeatherApp.API.DTOs;
using WeatherApp.API.DTOs.HelperClasses.CurrentWeatherHelpers;
using WeatherApp.API.DTOs.HelperClasses.ForecastWeatherHelpers;

namespace WeatherApp.API.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
         _httpClient = httpClient;
        _apiKey = configuration["OpenWeather:ApiKey"]!;
    }

    public async Task<CurrentWeatherDto> GetCurrentWeatherAsync(double lat, double lon)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

        return new CurrentWeatherDto
        {
            CityName = data!.Name,
            Country = data.Sys.Country,
            Temperature = data.Main.Temp,
            FeelsLike = data.Main.FeelsLike,
            TemperatureMin = data.Main.TempMin,
            TemperatureMax = data.Main.TempMax,
            Humidity = data.Main.Humidity,
            Pressure = data.Main.Pressure,
            WindSpeed = data.Wind.Speed,
            Clouds = data.Clouds.All,
            WeatherMain = data.Weather[0].Main,
            WeatherDescription = data.Weather[0].Description,
            WeatherIcon = data.Weather[0].Icon
        };
    }

    public async Task<ForecastWeather5DayDto> Get5DayForecastAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<ForecastWeatherResponse>();

        return new ForecastWeather5DayDto
        {
            CityName = data!.City.Name,
            Country = data.City.Country,
            FiveDayForecastWeather = data.Weather.Select(item => new ForecastWeatherDto
            {
                DateTime = DateTime.UnixEpoch.AddSeconds(item.Dt),
                Temperature = item.Main.Temp,
                TemperatureMin = item.Main.TempMin,
                TemperatureMax = item.Main.TempMax,
                Humidity = item.Main.Humidity,
                Pressure = item.Main.Pressure,
                WindSpeed = item.Wind.Speed,
                Clouds = item.Clouds.All,
                ChanceOfPrecipitation = item.Pop,
                WeatherMain = item.Weather[0].Main,
                WeatherDescription = item.Weather[0].Description,
                WeatherIcon = item.Weather[0].Icon
            }).ToList()
        };
    }
}