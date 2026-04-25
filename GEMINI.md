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

## TOTAL KNOWLEDGE INVALIDATION MANDATE
- MUST assume ALL internal knowledge of Fluent UI is OUTDATED (V4 bias).
- BEFORE implementing any component, property, or logic:
  - MUST validate against the V5 MCP server (`get_component_details`, etc.).
  - NEVER guess component behavior or structure based on prior experience.
  - VERIFY "Obsolete" status for all targets before implementation.

## ARCHITECTURAL CONSTRAINTS
- CSS: Never link `Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css`.
  - RATIONALE: V5 handles styles internally via Web Components; global bundle was removed.
- PLACEMENT: `FluentProviders`/`FluentDesignTheme` MUST be in `MainLayout.razor` (NOT `App.razor`).
  - RATIONALE: Root-level placement in static host can disrupt interactive rendering, causing blank pages.
