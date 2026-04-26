namespace Aspire_FluentUI_V5_Showcase.Web.Client;

public interface IWeatherClient
{
    Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default);
}
