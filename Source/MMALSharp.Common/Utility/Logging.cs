using Microsoft.Extensions.Logging;
using System;

namespace MMALSharp.Common.Utility
{
    public static class MMALLog
    {
        /// <summary>
        /// Gets the global logger component.
        /// </summary>
        public static ILogger Logger => _logger;
        static readonly MMALLogger _logger = new MMALLogger();

        /// <summary>
        /// Responsible for getting/setting the working LoggerFactory.
        /// </summary>
        public static ILoggerFactory LoggerFactory
        {
            get => _logger.LoggerFactory;
            set => _logger.LoggerFactory = value;
        }

        private class MMALLogger : ILogger
        {
            public ILoggerFactory LoggerFactory
            {
                get { return _loggerFactory; }
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