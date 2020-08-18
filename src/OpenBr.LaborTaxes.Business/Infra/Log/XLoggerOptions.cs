using Microsoft.Extensions.Logging;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{
    public class XLoggerOptions
    {

        public XLoggerOptions()
        {
        }

        public LogLevel LogLevel { get; set; } = LogLevel.Information;

    }
}
