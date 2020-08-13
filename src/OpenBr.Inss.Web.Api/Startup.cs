using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenBr.Inss.Business.Infra.IoC;
using OpenBr.Inss.Business.Infra.MongoDb;
using System;
using System.IO;
using System.Reflection;

namespace OpenBr.Inss.Web.Api
{

    /// <summary>
    /// Application startup object
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Creates a new instance of the application
        /// </summary>
        /// <param name="configuration">Configuration object</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Injected settings property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {

            #region Swagger

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

            #endregion

            // Add application service
            services.AddApplicationService(Configuration);

            services.AddControllers();

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder object</param>
        /// <param name="env">Web host environment data</param>
        /// <param name="serviceProvider">Service provider collection</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenBrasil Inss v1.0");
                c.RoutePrefix = string.Empty;
            });

            #endregion

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Create database objects
            //IDbDocumentCollectionCreator creators = serviceProvider.GetService<IDbDocumentCollectionCreator>();
            //creators.Create().Wait();

        }
    }
}
