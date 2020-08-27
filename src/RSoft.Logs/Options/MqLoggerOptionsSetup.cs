using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace RSoft.Logs.Options
{

    public class MqLoggerOptionsSetup : ConfigureFromConfigurationOptions<LoggerOptions>
    {

        private readonly IConfiguration _configuration;

        public MqLoggerOptionsSetup(ILoggerProviderConfiguration<ILoggerProvider> providerConfiguration, IConfiguration configuration) : base(providerConfiguration.Configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(LoggerOptions options)
        {
            base.Configure(options);
            //TODO: NotImplementedException
            //options.ConnectionString = ... 
        }

    }

}
