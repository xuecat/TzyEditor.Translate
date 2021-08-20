using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TranslateArchivePlugin.Manager;
using TranslateArchivePlugin.Models;
using TranslateArchivePlugin.Store;
using TzyEditor.TranslateCore;
using TzyEditor.TranslateCore.Basic;
using TzyEditor.TranslateCore.Plugin;

namespace TranslateArchivePlugin
{
    public class Plugin : PluginBase
    {
        public override string PluginID
        {
            get
            {
                return "e7acec14-a68b-4116-b9a0-7d07be69de58";
            }
        }

        public override string PluginName => "TranslateArchive";

        public override string Description => "TranslateArchiveManager";

        public override Task<ResponseMessage> Init(ApplicationContext context)
        {
            context.Services.AddDbContext<ArchiveDBContext>
                (options => options.UseMySql(context.ConnectionString), ServiceLifetime.Scoped);

            context.Services.AddScoped<ITranslateStore, TranslateStore>();
            context.Services.AddScoped<TranslateManager>();

            return base.Init(context);
        }


        public override Task<ResponseMessage> Start(ApplicationContext context)
        {
            return base.Start(context);
        }

        public override Task<ResponseMessage> Stop(ApplicationContext context)
        {
            return base.Stop(context);
        }
    }
}
