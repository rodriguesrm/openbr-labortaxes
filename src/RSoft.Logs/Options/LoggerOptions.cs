using Microsoft.Extensions.Logging;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Logger option model
    /// </summary>
    public class LoggerOptions
    {

        /// <summary>
        /// Logging severity levels
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Elastic connection string 
        /// </summary>
        public string ConnectionString { get; set; } //TODO: substituir pelas configurações para o Elastic

    }
}
