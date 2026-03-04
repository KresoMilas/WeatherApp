namespace WeatherApp.API.DTOs.ForecastWeatherDtos;

public class ForecastWeatherDto
{
    public double Clouds { get; set; }
    public DateTime DateTime { get; set; } 
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public double Rain { get; set; }
    public double ChanceOfPrecipitation { get; set; }
    public double Temperature { get; set; }
    public double TemperatureMax { get; set; }
    public double TemperatureMin { get; set; }
    public string WeatherDescription { get; set; } = string.Empty;
    public string WeatherIcon { get; set; } = string.Empty;
    public string WeatherMain { get; set; } = string.Empty;
    public double WindSpeed { get; set; }
}