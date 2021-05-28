using System;
using Microsoft.Extensions.Logging;

namespace Raspicam.Net.Utility
{
    static class MmalLog
    {
        public static ILogger Logger { get; set; } = new MmalLogger();

        class MmalLogger : ILogger, IDisposable
        {
            public IDisposable BeginScope<TState>(TState state) => this;
            bool ILogger.IsEnabled(LogLevel logLevel) => true;
            void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => Console.WriteLine(formatter);

            public void Dispose() { }
        }
    }
}