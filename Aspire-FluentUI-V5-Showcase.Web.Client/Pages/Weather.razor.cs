using Aspire_FluentUI_V5_Showcase.Web.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Aspire_FluentUI_V5_Showcase.Web.Client.Pages;

public partial class Weather
{
    private WeatherViewModel? _viewModel;
    
    [Inject] 
    private IWeatherClient WeatherApi { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var data = await WeatherApi.GetWeatherAsync();
        _viewModel = WeatherViewModel.FromForecasts(data);
    }
}
