using System.IO;
using System.Reflection;
using System.Security.Claims;

using AutoMapper;

using FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions;
using FACTS.GenericBooking.Api.Filters;
using FACTS.GenericBooking.Api.Mappers;
using FACTS.GenericBooking.Api.Middleware;
using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Common.Email;
using FACTS.GenericBooking.Common.Helpers;
using FACTS.GenericBooking.Domain.Mappers;
using FACTS.GenericBooking.Domain.Services;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Ingres;

using FluentValidation.AspNetCore;

using MicroElements.Swashbuckle.FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

namespace FACTS.GenericBooking.Api
{
    public class Startup
    {
        private const string CorsPolicy = "Cors";
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration       = configuration;
            _hostingEnvironment = env;
        }

        private IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ForwardedHeadersOptions options = new()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            // set secure headers:
            // X-Frame-Options: Deny
            // X-Content-Type-Options: nosniff
            app.UseXfo(x => x.Deny());
            app.UseXContentTypeOptions();
            app.UseAuthentication();
            app.UseForwardedHeaders(options);
            app.UseEnableRequestBuffering();
            app.UseExceptionHandlerMiddleware();
            app.UseMiddleware<SecureSwaggerBasicAuthMiddleware>();
            app.UseHsts();
            app.UseCors(CorsPolicy);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "FACTS Generic Booking Api v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IFileProvider physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            services.AddSingleton<IFileProvider>(physicalProvider);
            
            CommonAppSettings commonAppSettings = Configuration.GetSection("CommonAppSettings").Get<CommonAppSettings>();
            commonAppSettings.EnvironmentName = _hostingEnvironment.EnvironmentName;
            string executingAssemblyFile = Assembly.GetEntryAssembly()?.Location;
            string executingAssemblyDirectory = Path.GetDirectoryName(executingAssemblyFile);
            commonAppSettings.ContentRootPath = executingAssemblyDirectory;
            services.AddSingleton(commonAppSettings);
            services.AddSingleton(Configuration.GetSection("EmailSettings").Get<EmailSettings>());
            AppSecrets appSecrets = Configuration.GetSection("AppSecrets").Get<AppSecrets>();
            services.AddSingleton(appSecrets);
            DatabaseConnections databaseConnections = new()
            {
                IngresDatabaseConnection = appSecrets.IngresDatabaseConnection,
                PostgresConnection       = appSecrets.PostgresConnection
            };
            services.AddSingleton(databaseConnections);
            services.AddJwtConfiguration(Configuration, appSecrets);
            //services.AddCustomAuthorisationPolicies(Configuration);
            services.AddMemoryCache();
            services.AddDetection();
            services.AddHttpContextAccessor();
            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddHealthChecks();
            services.AddControllers(options =>
            {
                options.Filters.Add(new FluentValidateAttribute());
                options.OutputFormatters.RemoveType<TextOutputFormatter>();
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                fv.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
            })
            .AddNewtonsoftJson(options => { options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; });
            services.AddApiVersioning(o => {
                //new UrlSegmentApiVersionReader();
                o.Conventions.Add(new VersionByNamespaceConvention());
                o.ReportApiVersions                   = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion                   = new ApiVersion(1, 0);
            });
            services.AddSwaggerGenConfiguration(Configuration, commonAppSettings);
            
            // databases
            services.AddPostgresDbContext(databaseConnections, ApplicationLogging.LoggerFactory, _hostingEnvironment.IsDevelopment());
            services.AddIngresNHibernateConfiguration(databaseConnections);

            services.AddCors(options => options.AddPolicy(CorsPolicy, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddAutoMapper(typeof(ApiAutomapperProfile).Assembly, typeof(DomainAutomapperProfile).Assembly);

            // services
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<ICachedIngresEntitiesService, CachedIngresEntitiesService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IQuoteRateService, QuoteRateService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IIngresAccessService, IngresAccessService>();
            services.AddScoped<IProcessEventService , ProcessEventService>();

            services.AddScoped<IJwtHelperService, JwtHelperService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}