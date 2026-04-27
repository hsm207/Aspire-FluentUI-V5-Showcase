using Aspire_FluentUI_V5_Showcase.Web.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddFluentUIComponents();

builder.Services.AddHttpClient<IWeatherClient, WeatherApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddHttpClient<Aspire_FluentUI_V5_Showcase.Web.Client.Services.SpaceClient>(client =>
{
    client.BaseAddress = new Uri("https://api.nasa.gov/");
    client.Timeout = TimeSpan.FromSeconds(3);
});

await builder.Build().RunAsync();
