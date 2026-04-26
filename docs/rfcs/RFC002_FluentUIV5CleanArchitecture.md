# RFC002: Fluent UI V5 Clean Component Architecture

**Status:** Draft (Revised with Audit Corrections)  
**Date:** 2026-04-25  
**Author(s):** Gemini CLI

## 1. Vision & Value
The goal is to provide a robust, maintainable UI architecture for the `Aspire-FluentUI-V5-Showcase` application. By adopting a "Clean Component" pattern in Blazor, we decouple business logic from rendering, ensuring high performance, ease of testing, and consistent theming. This ROI is realized through reduced regression risk, faster onboarding, and professional-grade maintainability.

## 2. Status Quo & Timebombs
- **Status Quo:** Components like `Weather.razor` tightly couple API logic, UI markup, and styling, leading to fragile, "God Component" structures.
- **Timebombs:** 
    - **Discovery Disconnect:** WASM-based components cannot resolve internal Aspire service discovery URLs.
    - **Styling Entropy:** Lack of standardized styling patterns will lead to unmanageable `app.css` bloat.

## 3. The Vision (Endgame)
Components act as "Humble Objects" that delegate logic to partial classes (`.razor.cs`) and styling to CSS-variable-based design tokens. The system is distributed-aware, with explicit boundaries between browser-side display and server-side infrastructure.

## 4. Architectural Design
### 4.1 Boundary Contract
```mermaid
%% @tag: Component-Architecture
graph TD
    UI[Weather.razor] -->|Binds to| VM[WeatherViewModel]
    Partial[Weather.razor.cs] -->|Produces| VM
    Partial -->|Calls| IClient[IWeatherClient]
    IClient -->|Infrastructure| Forwarder[API Forwarder]
    Forwarder -->|Internal| ApiService[Aspire Apiservice]
```

### 4.2 Pattern: The Partial Class (Code-Behind)
Per [Microsoft Docs on Blazor Component Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/blazor-for-web-forms-developers/components#code-behind), we will use the partial class pattern to separate logic from markup.
**Snippet:**
*Weather.razor.cs*
```csharp
public partial class Weather
{
    private WeatherViewModel? _viewModel;
    [Inject] private IWeatherClient WeatherClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var data = await WeatherClient.GetWeatherAsync();
        _viewModel = WeatherViewModel.FromForecasts(data);
    }
}
```

### 4.3 Styling Strategy: CSS Variables & Design Tokens
Per [Migration Guide for DesignTheme](/Migration/DesignTheme), we will use V5-standard CSS variables for all component styling, ensuring centralized management and high performance. We will NOT use scoped CSS or ad-hoc style attribute proxies.

**Implementation Plan:**
1. Define custom theme variables in `app.css` using `:root` CSS variables.
2. Use C# `StylesVariables` constants for consistent token application in partial classes.
```css
/* app.css - Centralized Design Tokens */
:root {
    --datagrid-glow: var(--colorBrandBackground);
}
```
```csharp
// Weather.razor.cs - Using C# constants for tokens
var borderRadius = StylesVariables.Borders.Radius.Medium;
```

## 5. Phased Implementation
- **Phase 1:** Refactor `Weather.razor` to `Weather.razor.cs` (Partial Class pattern).
- **Phase 2:** Introduce `IWeatherClient` interface and `WeatherViewModel` DTO to decouple UI from `HttpClient`.
- **Phase 3:** Standardize component styling using CSS Custom Properties (Design Tokens).

## 6. Behavioral Contracts
- **Contract 1:** The UI layer MUST NOT contain direct `HttpClient` calls.
- **Contract 2:** All WASM service calls MUST route through a server-side forwarder.
- **Contract 3:** Component styles MUST use design tokens via CSS variables, NOT scoped CSS.

## 7. Operational Guardrails
- **Blast Radius:** If the API Forwarder fails, only the data-grid rendering is affected.
- **Panic Button:** Rollback to standard static rendering via RenderMode override.

## 8. Final Design Calibration
- **Versioning:** All Fluent UI components must strictly adhere to version `5.0.0-rc.2-26098.1`.
- **Parametric Compliance:** Every property MUST be verified against `mcp_fluent-ui-blazor_get_component_details` output.
