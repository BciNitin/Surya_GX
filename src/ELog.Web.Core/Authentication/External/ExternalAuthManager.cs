using Abp.Dependency;
using ELog.Web.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Web.Core.Authentication.External
{
    public class ExternalAuthManager : IExternalAuthManager, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IConfigurationRoot _appConfiguration;

        public ExternalAuthManager(IIocResolver iocResolver, IExternalAuthConfiguration externalAuthConfiguration, IWebHostEnvironment env)
        {
            _iocResolver = iocResolver;
            _externalAuthConfiguration = externalAuthConfiguration;
            _appConfiguration = env.GetAppConfiguration();
        }

        public Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode)
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.IsValidUser(providerKey, providerAccessCode);
            }
        }

        public Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode)
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.GetUserInfo(accessCode);
            }
        }

        public IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> CreateProviderApi(string provider)
        {
            try
            {
                var providerInfo = _externalAuthConfiguration.Providers.FirstOrDefault(p => p.Name == provider);
                providerInfo.ClientSecret = _appConfiguration["Authentication:AzureAd:ClientSecret"];

                if (providerInfo == null)
                {
                    throw new Exception("Unknown external auth provider: " + provider);
                }

                var providerApi = _iocResolver.ResolveAsDisposable<IExternalAuthProviderApi>(providerInfo.ProviderApiType);
                providerApi.Object.Initialize(providerInfo);
                return providerApi;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
