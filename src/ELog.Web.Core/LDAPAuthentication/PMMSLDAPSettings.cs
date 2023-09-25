using Abp.Domain.Repositories;
using ELog.Core.Interfaces;
using ELog.Core.MultiTenancy;
using ELog.Web.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;

namespace ELog.Application.LDAPAuthentication
{
    public class PMMSLdapSettings : IPMMSLdapSettings
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public PMMSLdapSettings(IRepository<Tenant> tenantRepository, IWebHostEnvironment env)
        {
            _tenantRepository = tenantRepository;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        public async Task<bool> GetIsEnabled(int? tenantId)
        {
            return await Task
                .FromResult(bool.Parse(_appConfiguration["Authentication:PMMSLDAP:IsEnabled"]))
                .ConfigureAwait(false);
        }

        public async Task<ContextType> GetContextType(int? tenantId)
        {
            return await Task.FromResult(ContextType.Domain).ConfigureAwait(false);
        }

        public async Task<string> GetContainer(int? tenantId)
        {
            return await Task.FromResult(string.Empty).ConfigureAwait(false);
        }

        public async Task<string> GetDomain(int? tenantId)
        {
            return await Task
                .FromResult(_appConfiguration["Authentication:PMMSLDAP:DomainUrl"])
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserName(int? tenantId)
        {
            return await Task
                .FromResult(string.Format(_appConfiguration["Authentication:PMMSLDAP:AdminFetchUrl"], _appConfiguration["Authentication:PMMSLDAP:AdminUsername"]))
                .ConfigureAwait(false);
        }

        public async Task<string> GetPassword(int? tenantId)
        {
            return await Task.FromResult(_appConfiguration["Authentication:PMMSLDAP:AdminPassword"]).ConfigureAwait(false);
        }



        public string GetCommonFetchUrl()
        {
            return _appConfiguration["Authentication:PMMSLDAP:CommonFetchUrl"];
        }

        public string GetAdminFetchUrl()
        {
            return _appConfiguration["Authentication:PMMSLDAP:AdminFetchUrl"];
        }
    }
}
