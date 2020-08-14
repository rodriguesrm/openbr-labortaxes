using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace OpenBr.Inss.Web.Api.Extensions
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
        /// <param name="app"></param>
        /// <returns></returns>
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

    }
}
