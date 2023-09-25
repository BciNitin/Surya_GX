using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ELog.Core;
using ELog.Core.Authorization.Users;
using ELog.Core.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class PMMSAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager userManager { get; set; }

        protected PMMSAppServiceBase()
        {
            LocalizationSourceName = PMMSConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual async Task<IList<String>> GetCurrentUserRolesAsync(User user)
        {
            return await userManager.GetRolesAsync(user);
        }

        protected virtual async Task<IList<String>> GetCurrentpermissionsAsync(User user)
        {
            return await userManager.GetRolePermissions(user.Id);
        }

        protected virtual async Task<IList<String>> GetApprovedandActiveRolesOnlyAsync(User user)
        {
            return await userManager.GetApprovedandActiveRolesOnlyAsync(user.Id);
        }
    }
}