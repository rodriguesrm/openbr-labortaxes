using Microsoft.Extensions.Logging;
using System;

namespace RSoft.Logs
{
    public class Logger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }
    }
}
