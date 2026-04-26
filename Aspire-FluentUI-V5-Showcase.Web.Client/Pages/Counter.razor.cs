using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Aspire_FluentUI_V5_Showcase.Web.Client.Models;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace Aspire_FluentUI_V5_Showcase.Web.Client.Pages;

public partial class Counter
{
    [Inject]
    private IToastService ToastService { get; set; } = default!;

    private CounterViewModel ViewModel { get; set; } = new();

    private async Task IncrementCountAsync()
    {
        ViewModel.CurrentCount++;
        
        await ToastService.ShowToastAsync(options =>
        {
            options.Intent = ToastIntent.Success;
            options.Title = "Counter Incremented";
            options.Body = $"The current count is now {ViewModel.CurrentCount}.";
            options.Icon = new Icons.Regular.Size20.AddCircle();
            options.Timeout = 3000;
        });
    }
}
