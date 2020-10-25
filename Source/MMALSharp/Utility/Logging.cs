using System;
using Microsoft.Extensions.Logging;

namespace MMALSharp.Utility
{
    static class MmalLog
    {
        public static ILogger Logger { get; set; } = new MmalLogger();

        class MmalLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => Logger?.BeginScope(state);
            bool ILogger.IsEnabled(LogLevel logLevel) => Logger?.IsEnabled(logLevel) ?? false;

            void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Logger?.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}