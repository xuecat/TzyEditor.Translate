using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TzyEditor.TranslateCore;
using TzyEditor.TranslateCore.Plugin;

namespace TzyEditor.Translate
{
    public class ApplicationContextImpl : ApplicationContext
    {
        private ILogger _exceptionLogger = null;
        public ApplicationContextImpl(IServiceCollection isc)
            : base(isc)
        {
            string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
            if (!System.IO.Directory.Exists(pluginPath))
            {
                System.IO.Directory.CreateDirectory(pluginPath);
            }
            
            _exceptionLogger = LogManager.GetCurrentClassLogger();

            //所有程序集
            DirectoryLoader dl = new DirectoryLoader();
            List<Assembly> assList = new List<Assembly>();
            var psl = dl.LoadFromDirectory(pluginPath);
            assList.AddRange(psl);
            AdditionalAssembly = assList;
        }

        public async override Task<bool> Init()
        {
            try
            {
                await base.Init();

                string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
                PluginFactory.Load(pluginPath);
                bool isOk = PluginFactory.Init(this).Result;
                return true;
            }
            catch (ReflectionTypeLoadException ex)
            {
                _exceptionLogger.Error("load exception：\r\n{0}", ex.ToString());
                if (ex.LoaderExceptions != null)
                {
                    foreach (Exception e in ex.LoaderExceptions)
                    {
                        _exceptionLogger.Error("{0}", e.ToString());
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _exceptionLogger.Error("init exception：\r\n{0}", ex.ToString());
                return false;
            }

            return false;
        }

        public async override Task<bool> Start()
        {
            await PluginFactory.Start(this);
            return await base.Start();
        }

        public async override Task<bool> Stop()
        {
            await PluginFactory.Stop(this);
            return await base.Stop();
        }
    }
}
