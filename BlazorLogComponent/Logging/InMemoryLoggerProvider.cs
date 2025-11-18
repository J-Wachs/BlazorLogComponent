using BlazorLogComponent.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlazorLogComponent.Logging;

/// <summary>
/// A provider that creates instances of the InMemoryLogger.
/// This provider is registered with the DI container to hook into the .NET logging pipeline.
/// </summary>
[ProviderAlias("InMemory")]
public class InMemoryLoggerProvider(ILoggingService loggingService) : ILoggerProvider
{
    /// <summary>
    /// Creates a new InMemoryLogger instance for a given category.
    /// </summary>
    /// <param name="categoryName">The category name of the logger, typically the fully qualified name of the class requesting the logger.</param>
    /// <returns>A new instance of InMemoryLogger.</returns>
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger(loggingService);

    public void Dispose()
    {
        // No resources to dispose in this provider.
        GC.SuppressFinalize(this);
    }
}
