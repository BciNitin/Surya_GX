using Abp.AspNetCore;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Json;
using ELog.Core.Identity;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using ELog.Web.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ELog.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private const string _apiVersion = "v1";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
            services.AddHttpClient();
            services.AddDistributedMemoryCache();

          

            IdentityRegistrar.Register(services);
            AuthConfigurer.ConfigureAsync(services, _appConfiguration);


            IConfiguration configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json")
       .Build();

            services.AddDbContext<PMMSDbContext>(options =>
       options.UseMySql(configuration.GetConnectionString("DefaultConnection")));





            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );
            //services.AddIdentity<User, IdentityRole>(opt =>
            //{
            //    //previous code removed for clarity reasons
            //    opt.Lockout.AllowedForNewUsers = true;
            //    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            //    opt.Lockout.MaxFailedAccessAttempts = 3;
            //});
            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Surya Roshni API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
            });
            return services.AddAbp<PMMSWebHostModule>();
        }

#pragma warning disable GCop502 // The parameter '{0}' doesn't seem to be used in this method. Consider removing it. If the argument must be declared for compiling reasons, rename it to contain only underscore character.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
#pragma warning restore GCop502 // The parameter '{0}' doesn't seem to be used in this method. Consider removing it. If the argument must be declared for compiling reasons, rename it to contain only underscore character.
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseAbpRequestLocalization();
            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) &&
                !context.Request.Path.Value.StartsWith("/api/services", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "Surya Rishni API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("ELog.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
        }
    }
}