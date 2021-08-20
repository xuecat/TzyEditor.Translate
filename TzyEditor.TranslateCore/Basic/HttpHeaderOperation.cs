using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TzyEditor.TranslateCore.Basic
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {

        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            var f = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );
            swaggerDoc.Paths.Clear();
            foreach (var item in f)
            {
                swaggerDoc.Paths.Add(item.Key, item.Value);
            }
        }
    }

    public class HttpHeaderOperation : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            if (context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            {
                if (methodInfo.DeclaringType.IsDefined(typeof(ApiVersionAttribute), true))
                {

                    var apiVersion = methodInfo.DeclaringType.GetCustomAttributes(true).Where(a => a is ApiVersionAttribute).Select(a => a as ApiVersionAttribute).FirstOrDefault();
                    var mapVersion = methodInfo.GetCustomAttributes(true).Where(a => a is MapToApiVersionAttribute).Select(a => a as MapToApiVersionAttribute).FirstOrDefault();

                    var param = operation.Parameters.SingleOrDefault(a => a.Name == "version" && a.In == ParameterLocation.Path);
                    if (param != null)
                    {
                        //operation.Parameters.Remove(param);
                        if (apiVersion.Versions.Count > 0)
                        {
                            if (mapVersion != null)
                            {
                                param.Schema.Default = new OpenApiString(mapVersion.Versions[0].ToString());
                            }
                            else
                                param.Schema.Default = new OpenApiString(apiVersion.Versions[0].ToString());

                        }

                    }

                }
            }

            
        }
    }
}
