using WeatherApp.API.DTOs.HelperClasses;

namespace WeatherApp.API.DTOs.HelperClasses.ForecastWeatherHelpers;


public class WeatherList
{
    public MainData Main { get; set; } = null!;           
    public List<WeatherData> Weather { get; set; } = [];  
    public WindData Wind { get; set; } = null!;   

    public CloudsData Clouds { get; set; } = null!;        

    public double Pop { get; set; }                       
    public long Dt { get; set; }                          
}