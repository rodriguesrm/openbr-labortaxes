using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;

namespace RSoft.Logs.Providers
{
    public class ConsoleLoggerProvider : LoggerProvider
    {

        #region Local objects/variables


        #endregion

        #region Construtors

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="options">Options monitor to logger configuration</param>
        public ConsoleLoggerProvider(IOptionsMonitor<LoggerOptions> options) : this(options.CurrentValue)
        {
            _settingsChangeToken = options.OnChange(opt =>
            {
                Settings = opt;
            });
        }

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="settings">Logger options config settings</param>
        public ConsoleLoggerProvider(LoggerOptions settings) : base(null)
        {
            Settings = settings;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Logger options configuration
        /// </summary>
        internal LoggerOptions Settings { get; private set; }

        #endregion

        #region Local methods

        protected override void WriteLogAction(LogEntry info)
        {
            Terminal.Print(info.Category, info.Level, info.Text, info.Exception);
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override bool IsEnabled(LogLevel logLevel)
        {

            bool result =
                logLevel != LogLevel.None
                && Settings.LogLevel != LogLevel.None
                && Convert.ToInt32(logLevel) >= Convert.ToInt32(Settings.LogLevel);

            return result;

        }

        #endregion

        #region IDisposable Support

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            _terminated = true;
            base.Dispose(disposing);
        }

        #endregion
    }

}