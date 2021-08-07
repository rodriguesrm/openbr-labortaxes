using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBr.LaborTaxes.Business.Infra.IoC;
using OpenBr.LaborTaxes.Business.Infra.MongoDb;
using OpenBr.LaborTaxes.Grpc.Host.Services;
using System;

namespace OpenBr.LaborTaxes.Grpc.Host
{
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
            services.AddGrpc();
            services.AddApplicationService(Configuration);
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

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<LaborTaxesGrpcService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            // Create database objects
            IDbDocumentCollectionCreator creators = serviceProvider.GetService<IDbDocumentCollectionCreator>();
            creators.Create().Wait();

            // Seed data from seeders
            var dbDataSeeder = serviceProvider.GetService<IDbDataSeeder>();
            dbDataSeeder.Seed().Wait();

        }
    }
}
