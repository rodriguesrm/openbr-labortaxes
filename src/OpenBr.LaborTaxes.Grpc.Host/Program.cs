using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Extensions;
using MsHost = Microsoft.Extensions.Hosting.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Grpc.Host
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
            MsHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsoleLogger();
                    logging.AddSeqLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
