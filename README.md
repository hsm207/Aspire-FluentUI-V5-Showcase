# Aspire Fluent UI V5 Showcase

A sample project demonstrating .NET Aspire integration with Fluent UI Blazor V5 using the `InteractiveAuto` render mode.

## Features

- **NASA APOD Integration**: Displays the Astronomy Picture of the Day.
- **Service Discovery**: Uses Aspire to connect the web frontend to the API service.
- **InteractiveAuto**: Uses both Server and WASM rendering.
- **Resilience**: Basic Polly retry policies for API calls.
- **Responsive Layout**: Designed to fit different screen sizes.

## Project Structure

- `AppHost`: Orchestrates the services.
- `ApiService`: Minimal API providing data.
- `Web`: Blazor Server project.
- `Web.Client`: Blazor WASM project for interactivity.

## Requirements

- .NET 8.0 or 9.0
- .NET Aspire Workload

## Running the App

1. Run the AppHost:
   ```bash
   dotnet run --project Aspire-FluentUI-V5-Showcase.AppHost
   ```
2. Open the Aspire Dashboard URL shown in the terminal.
