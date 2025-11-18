using BlazorLogComponent.Interfaces;
using BlazorLogComponent.Models;

namespace BlazorLogComponent.Services;

/// <summary>
/// A singleton service that captures and stores log messages in-memory.
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly LinkedList<LogMessage> _messages = new();
    private readonly int _maxMessages = 200; // Configurable limit to prevent memory leaks

    public event Action<LogMessage>? OnNewLogMessage;

    public IEnumerable<LogMessage> GetMessages()
    {
        lock (_messages)
        {
            return _messages.ToList(); // Return a copy for thread safety
        }
    }

    public void AddMessage(LogMessage message)
    {
        lock (_messages)
        {
            // Using a LinkedList is more efficient for adding and removing from the ends
            if (_messages.Count >= _maxMessages)
            {
                _messages.RemoveFirst(); // Remove the oldest message
            }
            _messages.AddLast(message);
        }

        // Notify subscribers about the new message
        OnNewLogMessage?.Invoke(message);
    }
}
