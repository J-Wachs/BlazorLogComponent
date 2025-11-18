using Microsoft.Extensions.Logging;

namespace BlazorLogComponent.Models;

/// <summary>
/// Represents a single log entry captured by the in-memory logging service.
/// </summary>
public class LogMessage
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public EventId EventId { get; set; }
    public Exception? Exception { get; set; }
}
