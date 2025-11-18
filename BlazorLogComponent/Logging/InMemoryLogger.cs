using BlazorLogComponent.Interfaces;
using BlazorLogComponent.Models;
using Microsoft.Extensions.Logging;

namespace BlazorLogComponent.Logging;

/// <summary>
/// A custom logger implementation that forwards log messages to the ILoggingService.
/// </summary>
public class InMemoryLogger(ILoggingService loggingService) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

    // Log messages of any level are considered enabled for this logger.
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = new LogMessage
        {
            Timestamp = DateTime.Now,
            Level = logLevel,
            EventId = eventId,
            Message = formatter(state, exception),
            Exception = exception
        };

        loggingService.AddMessage(message);
    }
}
