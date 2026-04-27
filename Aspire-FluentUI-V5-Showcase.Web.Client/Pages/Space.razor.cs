using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Aspire_FluentUI_V5_Showcase.Web.Client.Components.Space;
using Aspire_FluentUI_V5_Showcase.Web.Client.Services;

namespace Aspire_FluentUI_V5_Showcase.Web.Client.Pages;

public partial class Space : IDisposable
{
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;
    [Inject] private SpaceClient SpaceClient { get; set; } = default!;
    [Inject] private ILogger<Space> Logger { get; set; } = default!;

    private NasaApodResponse? apodData;
    private SpaceCardViewModel? cardViewModel;
    private PersistingComponentStateSubscription _subscription;

    protected override async Task OnInitializedAsync()
    {
        _subscription = ApplicationState.RegisterOnPersisting(PersistData);

        try
        {
            if (!ApplicationState.TryTakeFromJson<NasaApodResponse>("apodData", out var restored))
            {
                apodData = await SpaceClient.GetApodAsync();
            }
            else
            {
                apodData = restored;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during Space page initialization");
        }

        if (apodData == null)
        {
            // Bubbly Fallback: If NASA is grumpy, we serve our own curated cosmos! 
            apodData = new NasaApodResponse(
                "2026-04-26",
                "The universe is an infinite canvas of wonder. While our connection to the NASA API is momentarily paused (rate limits are so un-peppy!), we still marvel at the deep field of our imagination.",
                "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?q=80&w=2072&auto=format&fit=crop",
                "image",
                "The Infinite Deep Field",
                "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?q=80&w=2072&auto=format&fit=crop"
            );
        }

        if (apodData != null)
        {
            cardViewModel = new SpaceCardViewModel
            {
                Title = apodData.Title,
                ImageUrl = apodData.Url,
                Description = apodData.Explanation,
                Date = DateTime.TryParse(apodData.Date, out var date) ? date : DateTime.Now
            };
        }
    }

    private Task PersistData()
    {
        ApplicationState.PersistAsJson("apodData", apodData);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}
