using Microsoft.AspNetCore.Components;

namespace Aspire_FluentUI_V5_Showcase.Web.Client.Components.Space;

public partial class SpaceCard
{
    [Parameter]
    public SpaceCardViewModel? ViewModel { get; set; }
}
