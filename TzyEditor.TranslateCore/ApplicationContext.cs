using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TzyEditor.TranslateCore.Interface;

namespace TzyEditor.TranslateCore
{
    public class ApplicationContext
    {
        public List<Assembly> AdditionalAssembly { get; set; }

        public IServiceCollection Services { get;}
        public IPluginFactory PluginFactory { get; set; }
        public IApplicationBuilder ApplicationBuilder { get; set; }
        public bool UseSwagger { get; set; }
        public string ConnectionString { get; set; }
        public ApplicationContext(IServiceCollection isc)
        {
            Services = isc;
        }

        public virtual Task<bool> Init()
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> Start()
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> Stop()
        {
            return Task.FromResult(true);
        }
    }
}
