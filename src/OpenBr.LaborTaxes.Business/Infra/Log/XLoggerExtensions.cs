using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Microsoft.Extensions.Options;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{
    public static class XLoggerExtensions
    {

        static public ILoggingBuilder AddXLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<XLoggerOptions>, XLoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<XLoggerOptions>, LoggerProviderOptionsChangeTokenSource<XLoggerOptions, XLoggerProvider>>());
            return builder;
        }

        static public ILoggingBuilder AddXLogger(this ILoggingBuilder builder, Action<XLoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddXLogger();
            builder.Services.Configure(configure);

            return builder;

        }

    }

}