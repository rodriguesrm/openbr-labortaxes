using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace OpenBr.LaborTaxes.Web.Api.Extensions
{

    /// <summary>
    /// Web-API application extensions
    /// </summary>
    public static class ApplicationExtensions
    {

        #region Swagger

        
        /// <summary>
        /// Add swagger configuration to service application
        /// </summary>
        /// <param name="services">Services application collection</param>
        public static IServiceCollection AddApplicationSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "OpenBrasil INSS",
                        Version = "v1",
                        Description = "INSS Calculator API",
                        Contact = new OpenApiContact
                        {
                            Name = "Rodrigo Rodrigues",
                            Url = new Uri("https://github.com/rodriguesrm")
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            return services;

        }

        /// <summary>
        /// Enable swagger in application
        /// </summary>
        /// <param name="app">Application builder object instance</param>
        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenBrasil Inss v1.0");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }

        #endregion

        #region HealthCheck

        /// <summary>
        /// Adds the HealthCheckService to the container, using the provided delegate to register health checks.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object instance</param>
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHealthChecks()
                    .AddMongoDb(
                                mongodbConnectionString: configuration.GetConnectionString("MongoDb"),
                                name: "MongoDB",
                                failureStatus: HealthStatus.Unhealthy,
                                timeout: TimeSpan.FromSeconds(15),
                                tags: new string[] { "mongodb" });

            return services;

        }

        /// <summary>
        /// Adds a middleware that provides health check status.
        /// </summary>
        /// <param name="app">Application builder object instance</param>
        public static IApplicationBuilder UseApplicationHealthChecks(this IApplicationBuilder app)
        {

            app
                .UseHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });


            return app;
        }

        #endregion

    }
}
