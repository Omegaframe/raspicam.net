using System;
using Microsoft.Extensions.Logging;

namespace MMALSharp.Utility
{
    static class MmalLog
    {
        public static ILogger Logger { get; set; } = new MmalLogger();

        public static ILoggerFactory LoggerFactory
        {
            get => ((MmalLogger)Logger).LoggerFactory;
            set => ((MmalLogger)Logger).LoggerFactory = value;
        }

        class MmalLogger : ILogger
        {
            public ILoggerFactory LoggerFactory
            {
                get => _loggerFactory;
                set
                {
                    _loggerFactory = value;
                    _logger = null;
                }
            }

            ILoggerFactory _loggerFactory;
            ILogger _logger;
            ILogger Logger => _logger ??= _loggerFactory?.CreateLogger("MMALSharp");

            IDisposable ILogger.BeginScope<TState>(TState state) => Logger?.BeginScope(state);
            bool ILogger.IsEnabled(LogLevel logLevel) => Logger?.IsEnabled(logLevel) ?? false;

            void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Logger?.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}