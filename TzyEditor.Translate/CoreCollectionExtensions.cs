using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TzyEditor.TranslateCore;
using TzyEditor.TranslateCore.Tool;

namespace TzyEditor.Translate
{
   

    public static class CoreCollectionExtensions
    {
        public static string CreateConfigURI(string str)
        {
            if (str.IndexOf("http:") >= 0 || str.IndexOf("https:") >= 0)
            {
                return str;
            }
            else
                return "http://" + str;
        }

        public static IServiceCollection AddMysqlConnect(this IServiceCollection services, ApplicationContext context)
        {
            context.ConnectionString = string.Format(
                "Server={0};Port={4};Database={1};Uid={2};Pwd={3};Pooling=true;minpoolsize=0;maxpoolsize=40;SslMode=none;");
            return services;
        }

        public static IServiceCollection AddToolDefined(this IServiceCollection services, IConfiguration config)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var url = CreateConfigURI(config?.GetSection("ExportServerUrl").ToString());
            services.AddSingleton<RestClient>(new RestClient(url));

            return services;
        }

        public static IConfigurationBuilder AddMemConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config)
        {
            string devopsurl = string.Empty;
            using (Ping ping = new Ping())
            {
                PingReply replay = ping.Send("10.244.95.1", 100);
                if (replay != null && replay.Status == IPStatus.Success)
                {
                    devopsurl = config.GetSection("DevOpsInnerUrl").ToString();
                }
                else
                {
                    devopsurl = config.GetSection("DevOpsOuterUrl").ToString();
                }

                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>() { { "DevOpsUrl", devopsurl } });
            }
            using (var client = new RestClient(CreateConfigURI(devopsurl)))
            {
                var webc = client.GetWebConfig().Result;
                if (webc != null)
                {
                    configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>() { { "ExportServerUrl", webc.ExportServerUrl } });
                }
            }

            return configurationBuilder;
        }
    }
}
