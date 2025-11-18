using BlazorLogComponent.Models;

namespace BlazorLogComponent.Interfaces;

/// <summary>
/// Defines the contract for a service that captures and provides in-memory log messages.
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// An event that fires whenever a new log message is recorded.
    /// </summary>
    event Action<LogMessage> OnNewLogMessage;

    /// <summary>
    /// Gets all currently stored log messages.
    /// </summary>
    IEnumerable<LogMessage> GetMessages();

    /// <summary>
    /// Adds a new log message to the in-memory store.
    /// </summary>
    /// <param name="message">The log message to add.</param>
    void AddMessage(LogMessage message);
}
