using System.IO;

using FACTS.GenericBooking.Common.Helpers;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Web;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace FACTS.GenericBooking.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("init main");
            logger.Info("building webhost");

            IWebHostBuilder hostBuilder = WebHost
                .CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    IWebHostEnvironment env = hostingContext.HostingEnvironment;
                    config.AddEnvironmentVariables();
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                    IConfigurationRoot configurationRoot = config.Build();
                    LogManager.Configuration.Variables["connectionString"] = configurationRoot.GetConnectionString("PostgresConnection");
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                    ApplicationLogging.SetupLoggerFactory();
                })
                .UseNLog()
                .UseIISIntegration();

            IWebHost webHost = hostBuilder.Build();
            webHost.Run();
        }
    }
}