using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{
    public class XLoggerOptionsSetup : ConfigureFromConfigurationOptions<XLoggerOptions>
    {
        public XLoggerOptionsSetup(ILoggerProviderConfiguration<XLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }

    }

}