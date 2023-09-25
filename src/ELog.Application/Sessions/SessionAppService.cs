using Abp.Auditing;

using ELog.Application.Sessions.Dto;
using ELog.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Sessions
{
    public class SessionAppService : PMMSAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                var currentUser = await GetCurrentUserAsync();
                output.User = ObjectMapper.Map<UserLoginInfoDto>(currentUser);
                output.User.RoleNames = await GetApprovedandActiveRolesOnlyAsync(currentUser);
                output.User.Permissions = await GetCurrentpermissionsAsync(currentUser);
                output.User.PlantCode = await userManager.GetPlantName();
                output.User.ModeId = await userManager.GetUserAssingedMode(currentUser.Id);
                output.User.IsControllerMode = await userManager.IsControllerMode(currentUser.Id);
               // output.User.IsGateEntrySubModuleActive = await userManager.IsGateEntrySubModuleActiveAsync(currentUser.Id, PMMSConsts.InwardSubModule, PMMSConsts.GateEntrySubModule);
                output.User.TransactionActiveSubModules = await userManager.GetTransactionActiveSubModules(currentUser.Id);
                output.User.ApprovalLevelId = await userManager.GetUserAssingedApprovalLevelId(currentUser.Id);
                output.User.IsMaterialInspectionModuleSelected = await userManager.IsMaterialInspectionModuleSelected();
                output.User.ResetPasswordDaysLeft = await userManager.GetResetPasswordDaysLeft(currentUser.Id);
            }
            return output;
        }
    }
}