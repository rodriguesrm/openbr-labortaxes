using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Log provider for elastic
    /// </summary>
    public class ElasticLoggerProvider : LoggerProvider
    {

        #region Local objects/variables

        const string _channelName = "channelName"; //TODO: Change to configuration
        private bool terminated;
        private readonly ConcurrentQueue<LogEntry> _entryQueue = new ConcurrentQueue<LogEntry>();

        #endregion

        #region Construtors

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="options">Options monitor to logger configuration</param>
        /// <param name="accessor">Http context acessor object</param>
        public ElasticLoggerProvider(IOptionsMonitor<LoggerOptions> options, IHttpContextAccessor accessor) : this(options.CurrentValue, accessor)
        {
            _settingsChangeToken = options.OnChange(opt => {
                Settings = opt;
            });
        }

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="settings">Logger options config settings</param>
        /// <param name="accessor">Http context acessor object</param>
        public ElasticLoggerProvider(LoggerOptions settings, IHttpContextAccessor accessor) : base(accessor)
        {
            Settings = settings;
            ProcessLog();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Logger options configuration
        /// </summary>
        internal LoggerOptions Settings { get; private set; }

        #endregion

        #region Local methods

        /// <summary>
        /// Write log event int destination
        /// </summary>
        private void WriteLogEvent()
        {
            LogEntry info = null;
            if (_entryQueue.TryDequeue(out info))
            {
                //if (info.EventId.Id == MqEventId.EventId.Id)
                //{
                //TODO: WriteIndented will be configuration
                string json = JsonSerializer.Serialize(info, new JsonSerializerOptions() { IgnoreNullValues = false, PropertyNameCaseInsensitive = true, WriteIndented = false });
                //Console.WriteLine(json);
                Console.WriteLine(info.State);
                //    _rabbitMqUtil.SendMessageAsync(_channelName, json);
                //}

            }
        }

        /// <summary>
        /// Star thread to process log
        /// </summary>
        private void ProcessLog()
        {
            Task.Run(() => {

                while (!terminated)
                {
                    try
                    {
                        WriteLogEvent();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch // (Exception ex)
                    {
                        //TODO: Fix to correct handle exception
                    }
                }
            });
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

        ///<inheritdoc/>
        public override void WriteLog(LogEntry info)
        {
            _entryQueue.Enqueue(info);
        }

        #endregion

        #region IDisposable Support

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            terminated = true;
            base.Dispose(disposing);
        }

        #endregion

    }
}
