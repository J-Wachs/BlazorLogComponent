# Blazor Log Component

The Blazor Log Component is used to display real-time log messages from `Microsoft.Extensions.Logging` directly in
the UI of a Blazor application. The purpose is to provide developers and users with an in-app console for monitoring
and debugging application behavior, e.g. on an administrative dashboard/page.

The component can be configured to show a specific number of log messages and allows the user to filter which log
levels are visible. The filter toolbar is 'configuration-aware' and will automatically hide buttons for log levels
that are disabled in the application's master configuration (e.g., in `appsettings.json`).

The component has been developed in .NET 9 and is designed to be easily integrated into any Blazor application
(MAUI Blazor Hybrid, Server, or WebAssembly).

<img src="./Images/LogViewComponent.PNG" alt="Screen shot" style="max-width: 400px;">

## How does it work?

The Blazor Log Component works by hooking into the standard .NET logging pipeline. It consists of two main parts:
a backend service and a frontend Blazor component.

1.  **Backend:** A custom `ILoggerProvider` (`InMemoryLoggerProvider`) is registered in the application's service container. This provider creates loggers that forward every log message to a central, singleton service 1. (`LoggingService`). This service stores the most recent log messages in memory.

2.  **Frontend:** The `LogView.razor` component injects the `LoggingService`. It retrieves the stored messages on initialization and subscribes to an event to receive new messages in real-time. The component also injects a standard `ILogger` to query the application's master log level, ensuring the filter UI is always relevant.

The component uses a component-local JavaScript file (`LogView.razor.js`) to provide robust auto-scrolling, ensuring
the most recent log message is always in view.

The component has the following parameters:

*   **`int MaxVisibleMessages`**: This sets the maximum number of log messages that the component will display at
  * any given time. If the number of messages (after filtering) exceeds this value, the oldest messages are removed from the view. This parameter is optional and defaults to `100`.

## Installation instructions

### Setting up your project to use the Blazor Log Component

To use the Blazor Log Component in your own projects, you must first add the `BlazorLogComponent` project as a
project reference to your main application's solution. Then, you must register the required services in your
application's `Program.cs` or `MauiProgram.cs` file:

```csharp
// Add these using statements at the top of the file
using BlazorLogComponent.Interfaces;
using BlazorLogComponent.Services;
using BlazorLogComponent.Logging;
using Microsoft.Extensions.Logging;

// ... inside your CreateBuilder() or CreateMauiApp() method:

// 1. Register the singleton logging service.
builder.Services.AddSingleton<ILoggingService, LoggingService>();

// 2. Add the custom logger provider to the .NET logging pipeline.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.Services.AddSingleton<ILoggerProvider, InMemoryLoggerProvider>();
});

// End of BlazorLogComponent setup
```

### Adding and using the Blazor Log Component in your own projects

Please note, that if you copy the LogView.razor file into your own project, you must change the JavaScript import
path stated in the LogView.razor file to match your project's folder structure.

Add a global `using` statement to your main project's `_Imports.razor` file to make the component easily accessible:

```razor
@using BlazorLogComponent
```

You can now add the `LogView` component to any relevant page or layout in your project.

**Basic Example:**

```razor
<LogView />
```

**Example with a custom message limit:**

```razor
<LogView MaxVisibleMessages="20" />
```

## Modifying the Blazor Log Component for your own use

You are free to adapt a local version of this component to fit your needs. You might want to:

*   Change the default colors or styling in `LogView.razor.css`.
*   Add a 'Clear Log' button to the filter toolbar.
*   Implement text-based filtering in addition to level-based filtering.
*   Persist the user's filter selection using local storage.

## Found a bug?

Please create an issue in the repo.

## Known issues (Work in progress)

None at this time.

## FAQ

### Will this component interfere with other logging providers like Serilog?

No. The component is designed to work alongside any other logging provider. The .NET logging framework acts as a
broadcast hub, sending each log message to all registered providers simultaneously. As long as you do not call
`loggingBuilder.ClearProviders()` during setup, messages will be sent to both the in-app `LogView` and any other
destinations (like a file or console) that you have configured.

### Why is a singleton service used? Won't this show logs from other users in a Blazor Server app?

Yes, it will. The `LoggingService` is registered as a singleton, meaning one instance is shared across the entire
application.
*   **For MAUI Blazor Hybrid or Blazor WebAssembly:** This is the correct approach, as each user runs their own separate instance of the application.
*   **For a multi-user Blazor Server app:** The singleton service will be shared by all users. The log view would display a combined stream of logs from all active user sessions. To isolate logs per user in a Blazor Server environment, you would need to change the service registration from `AddSingleton` to `AddScoped`.

### Why does the component use JavaScript for auto-scrolling?

While a CSS-only approach (`flex-direction: column-reverse`) can work for simple lists, it fails when the list is
re-sorted or re-filtered, as it reverses the visual order of the DOM elements. A small, component-local JavaScript
module provides a much more robust and reliable auto-scrolling mechanism that works correctly even after the log
messages have been filtered.
