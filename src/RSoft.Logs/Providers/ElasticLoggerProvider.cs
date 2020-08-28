using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using nest = Nest;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Log provider for elastic
    /// </summary>
    public class ElasticLoggerProvider : LoggerProvider
    {

        #region Local objects/variables

        const string _channelName = "channelName"; //TODO: Change to configuration

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
            var createIndexResponse = client.Indices.Create(indexName,
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

        protected override void WriteLogAction(LogEntry info)
        {

            /*
            //if (info.EventId.Id == MqEventId.EventId.Id)
            //{
            //TODO: WriteIndented will be configuration
            // string json = JsonSerializer.Serialize(info, new JsonSerializerOptions() { IgnoreNullValues = false, PropertyNameCaseInsensitive = true, WriteIndented = false });
            //Console.WriteLine(json);
            if (info.Category == "Microsoft.Hosting.Lifetime")
                Console.WriteLine(info.Text ?? info.StateText ?? info.State.ToString());
            else
                Console.WriteLine($"{info.TimeStamp.ToLocalTime():yyyy-MM-dd hh:mm:ss.fff} [{info.Level}]: {info.Text ?? info.StateText ?? info.State.ToString()}");
            //    _rabbitMqUtil.SendMessageAsync(_channelName, json);
            //}
            */

            var url = "http://192.168.3.1:9200";
            var defaultIndex = "pock-nest-elk-actors";

            nest.ConnectionSettings settings = new nest.ConnectionSettings(new Uri(url)).DefaultIndex(defaultIndex);
            AddDefaultMappings(settings);

            nest.ElasticClient client = new nest.ElasticClient(settings);
            CreateIndex(client, defaultIndex);
            client.IndexDocument(info);

        }

        #endregion

    }
}
