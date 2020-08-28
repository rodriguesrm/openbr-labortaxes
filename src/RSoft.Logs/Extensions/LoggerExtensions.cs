using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using RSoft.Logs.Options;
using RSoft.Logs.Providers;
using System;

namespace RSoft.Logs.Extensions
{

    /// <summary>
    /// Provide extensino methods for RSoft.Logger
    /// </summary>
    public static class LoggerExtensions
    {

        public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder)
        {

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, ConsoleLoggerProvider>>());
            builder.Services.AddHttpContextAccessor();

            return builder;

        }

        public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddConsoleLogger();
            builder.Services.Configure(configure);

            return builder;

        }

        public static ILoggingBuilder AddElasticLogger(this ILoggingBuilder builder)
        {

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ElasticLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, ElasticLoggerProvider>>());
            builder.Services.AddHttpContextAccessor();

            //string id = configuration["Logging:EventId:Id"];
            //string eventName = configuration["Logging:EventId:Name"];

            //if (!int.TryParse(id, out int eventId))
            //    eventId = 7007;
            //if (string.IsNullOrWhiteSpace(eventName))
            //    eventName = "Farmarcas.Radar.Logs";

            //MqEventId.EventId = new EventId(eventId, eventName);

            return builder;

        }

        public static ILoggingBuilder AddElasticLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddElasticLogger();
            builder.Services.Configure(configure);

            return builder;

        }

    }

}
