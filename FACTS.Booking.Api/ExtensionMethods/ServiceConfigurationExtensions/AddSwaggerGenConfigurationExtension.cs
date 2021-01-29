using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using FACTS.GenericBooking.Common.Configuration;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions
{
    public static class AddSwaggerGenConfigurationExtension
    {
        public static void AddSwaggerGenConfiguration(this IServiceCollection services, IConfiguration configuration, CommonAppSettings commonAppSettings)
        {
            // register fluent validation assemblies
            List<ServiceDescriptor> serviceDescriptors = services.Where(descriptor => descriptor.ServiceType.GetInterfaces().Contains(typeof(IValidator))).ToList();
            serviceDescriptors.ForEach(descriptor => services.Add(ServiceDescriptor.Transient(typeof(IValidator), descriptor.ImplementationType)));
            // used for documentation markup
            //services.AddSwaggerExamples();
            services.AddSwaggerGen(c =>
            {
                // used for documentation markup
                //c.ExampleFilters();
                c.OperationFilter<TagByApiExplorerSettingsOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    In          = ParameterLocation.Header,
                    Type        = SecuritySchemeType.Http,
                    Scheme      = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id   = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
                c.EnableAnnotations();
                c.AddFluentValidationRules();
                c.SwaggerDoc("v1", new OpenApiInfo {Title = commonAppSettings.ApplicationName, Version = commonAppSettings.ApiVersion});
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }

    public class TagByApiExplorerSettingsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!(context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return;
            }

            ApiExplorerSettingsAttribute apiExplorerSettings = controllerActionDescriptor
                .ControllerTypeInfo.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), true)
                .Cast<ApiExplorerSettingsAttribute>().FirstOrDefault();
            if (apiExplorerSettings != null && !string.IsNullOrWhiteSpace(apiExplorerSettings.GroupName))
                operation.Tags = new List<OpenApiTag> {new OpenApiTag {Name = apiExplorerSettings.GroupName}};
            else
                operation.Tags = new List<OpenApiTag> {new OpenApiTag {Name = controllerActionDescriptor.ControllerName}};
        }
    }
}