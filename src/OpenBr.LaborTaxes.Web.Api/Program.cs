using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
