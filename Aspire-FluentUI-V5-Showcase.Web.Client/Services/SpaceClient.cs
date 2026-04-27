using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace Aspire_FluentUI_V5_Showcase.Web.Client.Services;

public class SpaceClient(HttpClient httpClient, ILogger<SpaceClient> logger)
{
    private static NasaApodResponse? _cache;
    private static DateTime? _cacheTimestamp;

    public async Task<NasaApodResponse?> GetApodAsync(CancellationToken cancellationToken = default)
    {
        if (IsCacheFresh())
        {
            return _cache;
        }

        try
        {
            var response = await httpClient.GetAsync("planetary/apod?api_key=DEMO_KEY", cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return await ProcessSuccessfulResponse(response, cancellationToken);
            }

            return HandleApiFailure(response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch data from NASA API");
            return _cache;
        }
    }

    private bool IsCacheFresh() => 
        _cache != null && _cacheTimestamp.HasValue && (DateTime.Now - _cacheTimestamp.Value).TotalHours < 1;

    private async Task<NasaApodResponse?> ProcessSuccessfulResponse(HttpResponseMessage response, CancellationToken ct)
    {
        var data = await response.Content.ReadFromJsonAsync<NasaApodResponse>(cancellationToken: ct);
        if (data != null)
        {
            _cache = data;
            _cacheTimestamp = DateTime.Now;
        }
        return data;
    }

    private NasaApodResponse? HandleApiFailure(System.Net.HttpStatusCode statusCode)
    {
        logger.LogWarning("NASA API returned status code: {StatusCode}", statusCode);
        return _cache;
    }
}

public record NasaApodResponse(
    string Date,
    string Explanation,
    string? Hdurl,
    string Media_type,
    string Title,
    string Url
);
