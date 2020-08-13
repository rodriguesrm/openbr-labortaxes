using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenBr.Inss.Business.Infra.Config;
using OpenBr.Inss.Business.Infra.MongoDb;

namespace OpenBr.Inss.Business.Infra.IoC
{

    /// <summary>
    /// Dependency Services Injector
    /// </summary>
    public static class DependencyInjection
    {

        /// <summary>
        /// Dependency injection service recorder
        /// </summary>
        /// <param name="services">Injection services collection</param>
        /// <param name="configuration">Configuration collection</param>
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {

            // Configuration
            services.AddScoped<IConfigurationBuilder, ConfigurationBuilder>();
            services.Configure<ApplicationConfig>(configuration.Bind);

            //MongoDB
            services.AddScoped<MongoDatabaseProvider>();
            services.AddScoped(s => s.GetService<MongoDatabaseProvider>().GetDatabase());

            // DbCreator
            services.AddScoped<IDbDocumentCollectionCreator, MongoCollectionCreator>();
            services.RegisterAllTypes<IDocumentCollectionCreator>(ServiceLifetime.Scoped, typeof(MongoCollectionCreator).Assembly);

            // Repositories
            //services.AddScoped<ICepRepository, CepRepository>();

            return services;

        }

    }

}