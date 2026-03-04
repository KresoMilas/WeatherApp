using WeatherApp.API.DTOs.HelperClasses;

namespace WeatherApp.API.DTOs.CurrentWeatherDtos;

public class OpenWeatherResponse
{
    public string Name { get; set; } = string.Empty;
    public MainData Main { get; set; } = null!;
    public List<WeatherData> Weather { get; set; } = [];
    public WindData Wind { get; set; } = null!;
    public SysData Sys { get; set; } = null!;
    public CloudsData Clouds { get; set; } = null!;
}