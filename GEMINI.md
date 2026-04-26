# Project Identity
This project is a demonstration of building a .NET Aspire application utilizing Fluent UI Blazor V5 with Interactive Auto render mode. It strictly adheres to Clean Architecture principles while maintaining alignment with Fluent UI's specific component patterns and modern C# best practices.

# Mandatory: Aspire/FluentV5 Protocol

## REBUILD MANDATE
- IF modifying `App.razor`, `Program.cs`, or Layouts:
  - MUST run `aspire resource webfrontend rebuild`.
  - NEVER verify until status is `Healthy`.

## VERIFICATION PROTOCOL
- BEFORE taking screenshots for error/blank page:
  - MUST run `curl -k <URL>` to inspect raw HTML.
  - VERIFY: Components are HTML-rendered (NOT literal tags).
  - MANDATE: Search raw HTML for the literal string `<ComponentName` (e.g., `<FluentDesignTheme`). If found, it is a NAMESPACE FAILURE.
  - VERIFY: `_Imports.razor` has library namespaces.

## DEPENDENCY PRE-FLIGHT
- BEFORE using auxiliary components (e.g., Icons, DataGrid):
  - MUST verify if a separate NuGet package is required (V5 often splits these).
  - MUST apply documented namespace aliases (e.g., `@using Icons = ...`) in `_Imports.razor`.

## HOST BOUNDARY MANDATE
- IF AppHost state is 'Unknown' or 'Stopped':
  - MUST STOP and ask the user to start the host.
  - NEVER attempt `aspire start` or `aspire run` autonomously.

## GLOBAL INTERACTIVITY MANDATE
- IF implementing Global Interactivity (e.g., `@rendermode="InteractiveAuto"` in `App.razor`):
  - MUST relocate infrastructure components (`Routes.razor`, `MainLayout.razor`, `NavMenu.razor`) to the `.Client` project.
  - RATIONALE: The WASM client MUST be able to resolve these types in its own assembly to support hydration.
  - RATIONALE: Prevents `InvalidOperationException` regarding `RenderFragment` serialization in layouts.
  - RATIONALE: Ensures `FluentProviders` in `MainLayout` are visible to the interactive circuit.

## TOTAL KNOWLEDGE INVALIDATION MANDATE
- MUST assume ALL internal knowledge of Fluent UI is OUTDATED (V4 bias).
- BEFORE implementing any component, property, or logic:
  - MUST validate against the V5 MCP server (`get_component_details`, etc.).
  - NEVER guess component behavior or structure based on prior experience.
  - VERIFY "Obsolete" status for all targets before implementation.

## DISTRIBUTED BOUNDARY MANDATE
- IF implementing API calls in .Client (WASM) project:
  - MUST NOT use Aspire Service Discovery URLs (e.g., `https+http://apiservice`).
  - MUST use a server-side API forwarder or absolute URLs reachable by the browser.
  - RATIONALE: The browser cannot resolve Aspire's internal service discovery scheme.

## ARCHITECTURAL CONSTRAINTS
- **SERIALIZATION BOUNDARY**: NEVER apply `@rendermode` directly to a component that receives a `RenderFragment` parameter (like `MainLayout` with `Body`) from a different rendering context.
  - RATIONALE: `RenderFragment` delegates cannot be serialized to cross the interactive circuit boundary.
- **PLACEMENT**: `FluentProviders` MUST be in `MainLayout.razor` (NOT `App.razor`).
  - MANDATE: The `FluentProviders` MUST live within the same interactive circuit as the components using its services. If the layout is static, interactive pages will NOT see the providers (the "Island Problem").
- **CSS**: MUST link `Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css` in the host page.
  - RATIONALE: While V5 handles most styles via Web Components, layout components (like FluentLayout) still rely on the scoped CSS bundle for grid definitions.
- **INTERACTIVE AUTO REGISTRATION**: IF using `InteractiveAuto` render mode, MUST register services (e.g., HttpClients) in BOTH the `.Web` (Server) and `.Web.Client` (WASM) projects to avoid `InvalidOperationException` during pre-rendering.

## COMPONENT DESIGN AND STYLING
- **SCOPED CSS**: MUST NOT disable scoped CSS in application projects. It is essential for component-level layout isolation.
- **LAYERED STYLING**:
  - **Layer 1**: Global semantic tokens in `app.css` MUST use generic names (e.g., `--accent-glow`). NEVER use component-specific names (e.g., `--weather-glow`) in global files.
  - **Layer 2**: Library tokens (e.g., `--accent-fill-rest`) provided by Fluent UI MUST be treated as the immutable design system foundation.
  - **Layer 3**: Scoped layout CSS in `.razor.css` MUST consume Layer 1 and Layer 2 tokens for theming.
- **HUMBLE COMPONENT**: MUST separate components into four parts:
  - `[Name]ViewModel.cs`: Pure data structure.
  - `[Name].razor.cs`: Partial class for logic and service interaction (The Brain).
  - `[Name].razor`: Markup only, binding to ViewModel (The Face).
  - `[Name].razor.css`: Scoped layout rules (The Makeup).
- **NAVIGATION**: Use `Href="/"` instead of `Href=""` for root links in `FluentNavItem`.
  - RATIONALE: An empty `Href` in `FluentNavItem` can cause the component to render as a `<button>` instead of an `<a>`, breaking standard navigation expectations.
