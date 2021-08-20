using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TzyEditor.TranslateCore;
using TzyEditor.TranslateCore.Basic;

namespace TzyEditor.Translate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static ApplicationContextImpl applicationContext = null;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //加载字典
            TranslateCore.GlobalDictionary.Instance.GetType();
            applicationContext = new ApplicationContextImpl(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var apppart = services.FirstOrDefault(x => x.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance;
            if (apppart != null)
            {
                ApplicationPartManager apm = apppart as ApplicationPartManager;
                //所有附件程序集
                applicationContext.AdditionalAssembly.ForEach((a) =>
                {
                    apm.ApplicationParts.Add(new AssemblyPart(a));
                });
            }

            //单例注入RestClient等
            services.AddToolDefined(Configuration)
                    .AddMysqlConnect(applicationContext);

            bool InitIsOk = applicationContext.Init().Result;

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                //o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                //o.ApiVersionReader = new QueryStringApiVersionReader();
                o.AssumeDefaultVersionWhenUnspecified = true;
                //o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
            });

            ;
            var basePath = AppContext.BaseDirectory + "Plugin//";
            var xmlPath1 = basePath + applicationContext.PluginFactory.GetPluginInfo("e7acec14-a68b-4116-b9a0-7d07be69de58").SwaggerXml;

            if (File.Exists(xmlPath1))
            {
                applicationContext.UseSwagger = true;

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "> 翻译网关接口文档",
                        Description = "**A simple example Translate Web API**(接口设计原则: `Post`->新加，`Put`->修改)",
                        Contact = new OpenApiContact { Name = "XueCat", Email = "", },
                        License = new OpenApiLicense { Name = "Tianzhiyou", Url = new Uri("https://www.perfectworld.com/") }
                        //TermsOfService = new Uri("None"),
                    });
                    
                    c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                    // http://localhost:9024/swagger/v1/swagger.json
                    // http://localhost:9024/swagger/

                    c.IncludeXmlComments(xmlPath1);
                    c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数

                    //c.OperationFilter<RemoveVersionFromParameter>();
                    c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                    c.DocInclusionPredicate((version, desc) =>
                    {

                        if (!desc.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo)) return false;
                        var versions = methodInfo.DeclaringType
                            .GetCustomAttributes(true)
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);


                        var maps = methodInfo
                            .GetCustomAttributes(true)
                            .OfType<MapToApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions)
                            .ToArray();

                        return versions.Any(v => $"v{v.MajorVersion.ToString()}" == version || $"v{v.MajorVersion.ToString()}.{v.MinorVersion.ToString()}" == version)
                               && (!maps.Any() || maps.Any(v => $"v{v.MajorVersion.ToString()}" == version));
                    });
                });
            }


            //插件加载之后引用
            services.AddAutoMapper(applicationContext.AdditionalAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowCredentials();
            });

            if (applicationContext.UseSwagger)
            {
                app.UseSwagger().UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslateGateway API V1");
                    //c.ShowRequestHeaders();
                });
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            applicationContext.Start().Wait();
        }
    }
}
