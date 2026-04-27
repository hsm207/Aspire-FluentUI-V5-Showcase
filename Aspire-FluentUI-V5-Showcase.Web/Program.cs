using Microsoft.FluentUI.AspNetCore.Components;
using Aspire_FluentUI_V5_Showcase.Web.Client;
using Aspire_FluentUI_V5_Showcase.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<IWeatherClient, WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

builder.Services.AddHttpClient<Aspire_FluentUI_V5_Showcase.Web.Client.Services.SpaceClient>(client =>
{
    client.BaseAddress = new Uri("https://api.nasa.gov/");
}).AddStandardResilienceHandler(options =>
{
    options.Retry.MaxRetryAttempts = 1;
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(2);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Aspire_FluentUI_V5_Showcase.Web.Client._Imports).Assembly);

app.MapDefaultEndpoints();

app.MapGet("/weatherforecast", async (IWeatherClient client) =>
{
    return await client.GetWeatherAsync();
});

app.Run();
