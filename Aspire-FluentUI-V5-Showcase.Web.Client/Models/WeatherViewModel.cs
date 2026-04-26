namespace Aspire_FluentUI_V5_Showcase.Web.Client.Models;

public class WeatherViewModel
{
    public WeatherForecast[] Forecasts { get; set; } = [];

    public static WeatherViewModel FromForecasts(WeatherForecast[] forecasts)
    {
        return new WeatherViewModel { Forecasts = forecasts };
    }
}
