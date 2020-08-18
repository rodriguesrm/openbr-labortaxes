using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBr.LaborTaxes.Business.Infra.Log;

namespace OpenBr.LaborTaxes.Web.Api
{

    /// <summary>
    /// Kick-off application object
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main method kick-off
        /// </summary>
        /// <param name="args">Arguments list</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host environment builder
        /// </summary>
        /// <param name="args">Arguments list</param>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddXLogger(opt =>
                    {
                        opt.LogLevel = LogLevel.Information;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
