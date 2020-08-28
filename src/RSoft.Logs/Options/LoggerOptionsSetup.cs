using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace RSoft.Logs.Options
{

    public class LoggerOptionsSetup : ConfigureFromConfigurationOptions<LoggerOptions>
    {

        private readonly IConfiguration _configuration;

        public LoggerOptionsSetup(ILoggerProviderConfiguration<ILoggerProvider> providerConfiguration, IConfiguration configuration) : base(providerConfiguration.Configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(LoggerOptions options)
        {
            base.Configure(options);
            
            options.Elastic = new ElasticOptions();
            _configuration.GetSection("Logging:Elastic").Bind(options.Elastic);

            if (string.IsNullOrWhiteSpace(options.Elastic.Uri))
                throw new ConfigurationErrorsException("Elastic 'Uri' configuration not found or invalid");
            if (string.IsNullOrWhiteSpace(options.Elastic.DefaultIndexName))
                throw new ConfigurationErrorsException("Elastic 'DefaultIndexName' configuration not found or invalid");

        }

    }

}
