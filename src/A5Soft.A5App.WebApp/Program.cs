using System;
using System.Threading.Tasks;
using A5Soft.A5App.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace A5Soft.A5App.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");

                var host = CreateHostBuilder(args).Build();

                logger.Debug("migrating security database");

                using (var scope = host.Services.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ISecurityOrmServiceProvider>();

                    //Do the migration asynchronously
                    await service.InitializeAsync();
                }

                logger.Debug("migration completed");

                await host.RunAsync();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();  // NLog: Setup NLog for Dependency injection
    }
}
