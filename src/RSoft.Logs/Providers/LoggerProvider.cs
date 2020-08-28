using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Model;
using System;
using System.Collections.Concurrent;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Abstract class to provider logger
    /// </summary>
    public abstract class LoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {

        #region Local objects/variables

        private readonly ConcurrentDictionary<string, Logger> _logger = new ConcurrentDictionary<string, Logger>();
        private IExternalScopeProvider _scopeProvider;
        private IHttpContextAccessor _accessor;

        protected IDisposable _settingsChangeToken;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of logger provider
        /// </summary>
        /// <param name="accessor">Http context acessor object instance</param>
        public LoggerProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        #endregion

        ///<inheritdoc/>
        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        ///<inheritdoc/>
        ILogger ILoggerProvider.CreateLogger(string Category)
        {
            return _logger.GetOrAdd(Category,
            (category) => {
                return new Logger(this, category, _accessor);
            });
        }

        /// <summary>
        /// Indicate log level is enabled
        /// </summary>
        /// <param name="logLevel">Severity log level</param>
        public abstract bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Write log into destiny (storage, api, etc)
        /// </summary>
        /// <param name="info">Log entry data object</param>
        public abstract void WriteLog(LogEntry info);

        /// <summary>
        /// External data scope provider 
        /// </summary>
        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (_scopeProvider == null)
                    _scopeProvider = new LoggerExternalScopeProvider();
                return _scopeProvider;
            }
        }

        #region IDisposable Support

        /// <summary>
        /// Release resource
        /// </summary>
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

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Indicate disposing object flag</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_settingsChangeToken != null)
            {
                _settingsChangeToken.Dispose();
                _settingsChangeToken = null;
            }
        }

        /// <summary>
        /// Destroy object instance a release resources
        /// </summary>
        ~LoggerProvider()
        {
            if (!this.IsDisposed)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Flag to detect redundant calls
        /// </summary>
        public bool IsDisposed { get; protected set; }

        #endregion

    }

}
