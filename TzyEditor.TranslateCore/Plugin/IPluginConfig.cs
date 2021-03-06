using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TzyEditor.TranslateCore.Basic;

namespace TzyEditor.TranslateCore.Plugin
{
    public interface IPluginConfig
    {
        Type ConfigType { get; }

        Task<object> GetConfig(ApplicationContext context);

        Task<bool> SaveConfig(object cfg);

        object GetDefaultConfig(ApplicationContext context);

        Task<ResponseMessage> ConfigChanged(ApplicationContext context, object newConfig);


    }
}
