using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using TzyEditor.TranslateCore;

namespace TzyEditor.Translate
{
    public class Program
    {
        private static NLog.ILogger ExceptionLogger = null;
        public static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

                ExceptionLogger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

                //全局异常日志
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                     .ConfigureAppConfiguration((hostingContext, config) =>
                     {
                         config.AddConfiguration(configuration)
                                .AddMemConfiguration(configuration);
                     })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();

                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog()
                    .UseUrls($"http://*:{configuration["Port"]}")
                    .Build()
                    .Run();

            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                ExceptionLogger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
        
        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (ExceptionLogger != null)
            {
                ExceptionLogger.Error("Exception: {0}", e.Exception.ToString());
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (ExceptionLogger != null)
            {
                ExceptionLogger.Fatal("Crash：\r\n{0}", e.ExceptionObject.ToString());
            }
        }
    }
}
