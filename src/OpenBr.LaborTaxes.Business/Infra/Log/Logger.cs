using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{

    public class Logger : ILogger
    {

        private readonly LoggerProvider _provider;
        private readonly string _category;

        public Logger(LoggerProvider provider, string category)
        {
            _provider = provider;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
            => _provider.ScopeProvider.Push(state);

        public bool IsEnabled(LogLevel logLevel)
            => _provider.IsEnabled(logLevel);

        public void Log<TState>
        (
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {

            if ((this as ILogger).IsEnabled(logLevel))
            {

                LogEntry info = new LogEntry()
                {
                    Category = _category,
                    Level = logLevel,
                    Text = exception?.Message ?? state.ToString(),
                    Exception = exception != null ? new LogExceptionInfo(exception) : null,
                    EventId = eventId,
                    State = state
                };

                if (state is string)
                {
                    info.StateText = state.ToString();
                }
                else if (state is IEnumerable<KeyValuePair<string, object>> properties)
                {
                    info.StateProperties = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, object> item in properties)
                    {
                        info.StateProperties[item.Key] = item.Value;
                    }
                }

                if (_provider.ScopeProvider != null)
                {

                    _provider.ScopeProvider.ForEachScope((value, loggingProps) =>
                    {

                        if (info.Scopes == null)
                            info.Scopes = new List<LogScopeInfo>();

                        LogScopeInfo scope = new LogScopeInfo();

                        if (value is string)
                        {
                            scope.Text = value.ToString();
                        }
                        else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        {
                            if (scope.Properties == null)
                                scope.Properties = new Dictionary<string, object>();
                            foreach (KeyValuePair<string, object> pair in props)
                            {
                                scope.Properties[pair.Key] = pair.Value;
                            }
                        }

                        info.Scopes.Add(scope);


                    }, state);

                }

                _provider.WriteLog(info);

            }

        }
    }

}
