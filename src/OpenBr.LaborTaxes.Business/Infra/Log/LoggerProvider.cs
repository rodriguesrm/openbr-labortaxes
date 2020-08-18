using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{

    public abstract class LoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        private ConcurrentDictionary<string, Logger> loggers = new ConcurrentDictionary<string, Logger>();
        private IExternalScopeProvider fScopeProvider;
        protected IDisposable SettingsChangeToken;

        public LoggerProvider()
        {
        }

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            fScopeProvider = scopeProvider;
        }

        ILogger ILoggerProvider.CreateLogger(string Category)
        {
            return loggers.GetOrAdd(Category,
            (category) => {
                return new Logger(this, category);
            });
        }

        public abstract bool IsEnabled(LogLevel logLevel);

        public abstract void WriteLog(LogEntry info);

        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (fScopeProvider == null)
                    fScopeProvider = new LoggerExternalScopeProvider();
                return fScopeProvider;
            }
        }

        #region IDisposable Support

        void IDisposable.Dispose()
        {
            if (!this.IsDisposed)
            {
                try
                {
                    Dispose(true);
                }
                catch
                {
                }

                this.IsDisposed = true;
                GC.SuppressFinalize(this);  // instructs GC not bother to call the destructor   
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (SettingsChangeToken != null)
            {
                SettingsChangeToken.Dispose();
                SettingsChangeToken = null;
            }
        }

        ~LoggerProvider()
        {
            if (!this.IsDisposed)
            {
                Dispose(false);
            }
        }

        public bool IsDisposed { get; protected set; }

        #endregion

    }

}