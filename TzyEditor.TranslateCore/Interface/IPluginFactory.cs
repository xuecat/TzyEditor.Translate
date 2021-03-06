using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TzyEditor.TranslateCore.Plugin;

namespace TzyEditor.TranslateCore.Interface
{
    public interface IPluginFactory
    {
        List<Assembly> LoadedAssembly { get; }

        IPlugin GetPlugin(string pluginId);
        PluginItem GetPluginInfo(string pluginId, bool secret = false);
        List<PluginItem> GetPluginList(bool secret = false);
        void Load(string pluginPath);


        Task<bool> Init(ApplicationContext context);

        Task<bool> Start(ApplicationContext context);

        Task<bool> Stop(ApplicationContext context);
    }
}
