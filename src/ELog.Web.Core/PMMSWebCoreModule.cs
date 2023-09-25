using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using ELog.Application;
using ELog.Application.LDAPAuthentication;
using ELog.Core;
using ELog.Core.Interfaces;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using ELog.Web.Core.Auditing;
using ELog.Web.Core.Authentication.External;
using ELog.Web.Core.Authentication.JwtBearer;
using ELog.Web.Core.Configuration;
using ELog.Web.Core.LDAPAuthentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ELog.Web.Core
{
    [DependsOn(
         typeof(PMMSApplicationModule),
         typeof(PMMSEntityFrameworkModule),
         typeof(AbpAspNetCoreModule),
         typeof(AbpZeroLdapModule)
     )]
    public class PMMSWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PMMSWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                PMMSConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(PMMSApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();

            Configuration.ReplaceService<IAuditingStore, PMMSAuditingStore>(DependencyLifeStyle.Singleton);
            Configuration.ReplaceService<IAuditingHelper, PMMSAuditingHelper>(DependencyLifeStyle.Singleton);
            IocManager.Register<IPMMSLdapSettings, PMMSLdapSettings>(); //change default setting source
            IocManager.Register<ILdapExternalAuthenticationSource, PMMSLDAPAuthenticationSource>(DependencyLifeStyle.Transient);
            IocManager.Register<ILDAPClient, LDAPClient>(DependencyLifeStyle.Transient);
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromSeconds(Double.Parse(_appConfiguration["Authentication:JwtBearer:Expiration"]));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMMSWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            if (bool.Parse(_appConfiguration["Authentication:AzureAd:IsEnabled"]))
            {
                var externalAuthConfiguration = IocManager.Resolve<IExternalAuthConfiguration>();
                externalAuthConfiguration.Providers.Add(
                     new ExternalLoginProviderInfo(
                        AzureAdAuthProvider.Name,
                        _appConfiguration["Authentication:AzureAd:ClientId"],
                        null,
                        typeof(AzureAdAuthProvider)
                    )
                );

                IocManager.Resolve<ApplicationPartManager>()
               .AddApplicationPartsIfNotAddedBefore(typeof(PMMSWebCoreModule).Assembly);
            }
        }
    }
}