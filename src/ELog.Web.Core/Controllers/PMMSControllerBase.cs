using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using ELog.Core;
using Microsoft.AspNetCore.Identity;

namespace ELog.Web.Core.Controllers
{
    public abstract class PMMSControllerBase : AbpController
    {
        protected PMMSControllerBase()
        {
            LocalizationSourceName = PMMSConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
