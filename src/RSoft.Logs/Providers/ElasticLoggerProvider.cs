using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using nest = Nest;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;
using System.Linq;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Log provider for elastic
    /// </summary>
    internal class ElasticLoggerProvider : LoggerProvider
    {

        #region Local objects/variables

        private readonly nest.ConnectionSettings _settings;
        private readonly nest.ElasticClient _client;
        private readonly bool configIsOk;

        #endregion

        #region Construtors

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="options">Options monitor to logger configuration</param>
        /// <param name="accessor">Http context acessor object</param>
        public ElasticLoggerProvider(IOptionsMonitor<LoggerOptions> options, IHttpContextAccessor accessor) : this(options.CurrentValue, accessor)
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
        /// <param name="accessor">Http context acessor object</param>
        public ElasticLoggerProvider(LoggerOptions settings, IHttpContextAccessor accessor) : base(accessor)
        {
            Settings = settings;

            configIsOk = true;

            if (string.IsNullOrWhiteSpace(settings.Elastic.Uri))
            {
                configIsOk = false;
                Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Elastic 'Uri' configuration not found or invalid.\n{Terminal.Margin}Logger not work");
            }
            if (string.IsNullOrWhiteSpace(settings.Elastic.DefaultIndexName))
            {
                configIsOk = false;
                Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Elastic 'DefaultIndexName' configuration not found or invalid.\n{Terminal.Margin}Logger not work");
            }

            if (configIsOk)
            {
                _settings = new nest.ConnectionSettings(new Uri(settings.Elastic.Uri)).DefaultIndex(settings.Elastic.DefaultIndexName);
                AddDefaultMappings(_settings);
                _client = new nest.ElasticClient(_settings);
                CreateIndex(_client, settings.Elastic.DefaultIndexName);
            }

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
        /// Create index on elastic server
        /// </summary>
        /// <param name="client">Elastic client object instance</param>
        /// <param name="indexName">Index name to create</param>
        private void CreateIndex(nest.IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(Settings.Elastic.DefaultIndexName,
                index => index.Map<LogEntry>(x => x.AutoMap())
            );
        }

        /// <summary>
        /// Add default mapping model to elastic
        /// </summary>
        /// <param name="settings"></param>
        private void AddDefaultMappings(nest.ConnectionSettings settings)
        {
            settings.DefaultMappingFor<LogEntry>(m => m);
        }

        protected override void WriteLogAction(LogEntry info)
        {
            if (configIsOk)
            {
                if (!Settings.Elastic.IgnoreCategories.Contains(info.Category))
                {
                    nest.IndexResponse resp = _client.IndexDocument(info);
                    if (resp.Result == nest.Result.Error)
                        Terminal.Print(GetType().ToString(), LogLevel.Error, resp.DebugInformation, resp.OriginalException);
                }
            }
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
