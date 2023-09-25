using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using ELog.Core.Authorization.Roles;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Core.Authorization.Users
{
    [PMMSAuthorize]
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IRepository<PlantMaster> _plantMasterRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<RolePermissions> _rolePermissionsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPlants> _userPlantRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubModuleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IRepository<ApprovalUserModuleMappingMaster> _approvalUserModuleMappingMaster;

        public bool UserLockoutEnabledByDefault { get; private set; }
        public DateTime? DefaultAccountLockoutTimeSpan { get; private set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; private set; }

        public UserManager(
            RoleManager roleManager,
            UserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IPermissionManager permissionManager, IRepository<Setting, long> settingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings, IRepository<ModuleMaster> moduleRepository, IRepository<ModuleSubModule> moduleSubModuleRepository,
            ISettingManager settingManager, IRepository<PlantMaster> plantMasterRepository,
            IRepository<UserRole, long> userRoleRepository, IRepository<User, long> userRepository,
            IRepository<RolePermissions> rolePersmissionsRepository, IRepository<Role> roleRepository,
            IRepository<ApprovalStatusMaster> approvalStatusMasterRepository, IRepository<UserPlants> userPlantRepository,
             IHttpContextAccessor httpContextAccessor, IRepository<ModeMaster> modeRepository,
             IRepository<ApprovalUserModuleMappingMaster> approvalUserModuleMappingMaster,
            IRepository<SubModuleMaster> subModuleRepository)
            : base(
                roleManager,
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager)
        {
            _plantMasterRepository = plantMasterRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionsRepository = rolePersmissionsRepository;
            _roleRepository = roleRepository;
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _userRepository = userRepository;
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _userPlantRepository = userPlantRepository;
            _httpContextAccessor = httpContextAccessor;
            _subModuleRepository = subModuleRepository;
            _modeRepository = modeRepository;
            _moduleRepository = moduleRepository;
            _moduleSubModuleRepository = moduleSubModuleRepository;
            _approvalUserModuleMappingMaster = approvalUserModuleMappingMaster;
            _settingRepository = settingRepository;
        }

        public async Task<string> GetPlantName()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(plantId))
            {
                return await _plantMasterRepository.GetAll()
                                .Where(x => x.Id == Convert.ToInt32(plantId)).Select(x => x.PlantName)
                                .FirstOrDefaultAsync() ?? default;
            }
            return string.Empty;
        }
        public async Task<int> GetResetPasswordDaysLeft(long userId)
        {
            var user = await _userRepository.GetAll()
                               .Where(x => x.Id == userId)
                               .FirstOrDefaultAsync() ?? default;
            DateTime passwordResetDate = user?.PasswordResetTime != null ? (DateTime)user.PasswordResetTime : user.CreationTime;
            var resetPasswordDaysLeft = passwordResetDate.AddDays(90) - DateTime.Now;
            return Convert.ToInt32(resetPasswordDaysLeft.TotalDays);
        }
        public async Task<int> GetUserAssingedMode(long userId)
        {
            const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);

            return await (from user in _userRepository.GetAll()
                          join mode in _modeRepository.GetAll()
                          on user.ModeId equals mode.Id
                          where user.IsActive && user.ApprovalStatusId == approvalStatusValue && user.Id == userId
                          select mode.Id).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetRolePermissions(long userId)
        {

            const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);
            var approvedApprovalStatus = Enum.GetName(typeof(PMMSEnums.ApprovalStatus), approvalStatusValue);

            var result = from users in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on users.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         join approvalStatus in _approvalStatusMasterRepository.GetAll() on role.ApprovalStatusId equals approvalStatus.Id
                         where users.Id == userId && role.IsActive && approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus.ToLower()
                         select role.Name;

            var query = (from users in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on users.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         join approvalStatus in _approvalStatusMasterRepository.GetAll() on role.ApprovalStatusId equals approvalStatus.Id
                         join rolePermission in _rolePermissionsRepository.GetAll() on userRole.RoleId equals rolePermission.RoleId
                         where users.Id == userId && role.IsActive && approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus.ToLower()
                         select rolePermission.PermissionName);
            if (result.Contains("Admin") || result.Contains("SuperAdmin"))
            {
                return await query.ToListAsync();
            }
            return await query.Select(x => x.Replace("Password.View", "")).ToListAsync();
        }

        public async Task<User> UpdateIsActive(User user, bool IsActive)
        {
            user.IsActive = IsActive;
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IList<String>> GetApprovedandActiveRolesOnlyAsync(long userId)
        {
            string approvedApprovalStatus = nameof(ApprovalStatus.Approved).ToLower();
            return await (from role in _roleRepository.GetAll()
                          join userRole in _userRoleRepository.GetAll() on role.Id equals userRole.RoleId
                          join approvalStatus in _approvalStatusMasterRepository.GetAll()
                          on role.ApprovalStatusId equals approvalStatus.Id
                          where (role.IsActive &&
                          userRole.UserId == userId &&
                          approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus)
                          select role.Name)?.ToListAsync();
        }

        public async Task<IList<int?>> GetUsersPlantList(User user)
        {
            return await GetUserPlantList(user.Id);
        }

        public async Task<IList<int?>> GetUserPlantList(long userId)
        {
            const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
            var userPlants = await (from userPlant in _userPlantRepository.GetAll()
                                    join plant in _plantMasterRepository.GetAll()
                                    on userPlant.PlantId equals plant.Id
                                    where (userPlant.UserId == userId && plant.IsActive && plant.ApprovalStatusId == approvedApprovalStatusId)
                                    select userPlant.PlantId)?.ToListAsync();
            List<int?> userPlantsList = userPlants.Cast<int?>().ToList();
            return userPlantsList;
        }

        public async Task<string> GetConcurrencyStamp(long userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
            return user.ConcurrencyStamp;
        }

        public async Task<bool> IsGateEntrySubModuleActiveAsync(long userId, string moduleName, string subModuleName)
        {
            var moduleSubmoduleDetails = await (from moduleSubModule in _moduleSubModuleRepository.GetAll()
                                                join subModule in _subModuleRepository.GetAll()
                                                on moduleSubModule.SubModuleId equals subModule.Id
                                                join module in _moduleRepository.GetAll()
                                                on moduleSubModule.ModuleId equals module.Id
                                                where subModule.IsActive && module.IsActive && module.Name.Trim().ToLower() == moduleName.Trim().ToLower() && subModule.Name.Trim().ToLower() == subModuleName.Trim().ToLower()
                                                select moduleSubModule).FirstOrDefaultAsync() ?? default;

            return moduleSubmoduleDetails.IsSelected || moduleSubmoduleDetails.IsMandatory;
        }

        public async Task<List<String>> GetTransactionActiveSubModules(long userId)
        {
            var materialInspectionSubModuleId = await _subModuleRepository.GetAll().Where(x => x.IsActive && x.Name.Trim().ToLower() == PMMSPermissionConst.MaterialInspection_SubModule.Trim().ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
            return await (from submodule in _subModuleRepository.GetAll()
                          join module in _moduleSubModuleRepository.GetAll()
                          on submodule.Id equals module.SubModuleId
                          where submodule.IsActive
                          && (module.IsSelected || module.IsMandatory || module.SubModuleId == materialInspectionSubModuleId)
                          select submodule.Name).ToListAsync();
        }

        public async Task<int> GetUserAssingedApprovalLevelId(long userId)
        {
            var SubModuleId = await _subModuleRepository.GetAll().Where(x => x.IsActive && x.Name.Trim().ToLower() == PMMSPermissionConst.RecipeApproval.Trim().ToLower()).Select(x => x.Id).FirstOrDefaultAsync();

            return await (from user in _approvalUserModuleMappingMaster.GetAll()
                          where user.IsActive && user.SubModuleId == SubModuleId && user.UserId == userId && user.IsDeleted == false
                          select user.AppLevelId).FirstOrDefaultAsync();
        }
        public async Task<bool> IsMaterialInspectionModuleSelected()
        {
            return await (from submodule in _subModuleRepository.GetAll()
                          join module in _moduleSubModuleRepository.GetAll()
                          on submodule.Id equals module.SubModuleId
                          where submodule.IsActive && submodule.Name.Trim().ToLower() == PMMSPermissionConst.MaterialInspection_SubModule.Trim().ToLower()
                          select module.IsSelected).FirstOrDefaultAsync();
        }

        public async Task<bool> IsControllerMode(long userId)
        {
            const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);

            return await (from user in _userRepository.GetAll()
                          join mode in _modeRepository.GetAll()
                          on user.ModeId equals mode.Id
                          where user.IsActive && user.ApprovalStatusId == approvalStatusValue && user.Id == userId
                          select mode.IsController).FirstOrDefaultAsync();
        }
        public async Task InitializeLockoutSettings(int? tenantId)
        {
            var maxFailedAccessAttemptsBeforeLockoutSetting = await _settingRepository.GetAll().Where(a => a.Name.ToLower() == PMMSConsts.MaxFailedAccessAttemptsBeforeLockout.ToLower() && a.TenantId == tenantId).FirstOrDefaultAsync();
            var maxFailedAccessAttemptsBeforeLockoutValue = maxFailedAccessAttemptsBeforeLockoutSetting?.Value;
            var defaultAccountLockoutTimeSpanSetting = await _settingRepository.GetAll().Where(a => a.Name.ToLower() == PMMSConsts.DefaultAccountLockoutTimeSpan.ToLower() && a.TenantId == tenantId).FirstOrDefaultAsync();
            var defaultAccountLockoutTimeSpanValue = defaultAccountLockoutTimeSpanSetting?.Value;

            DefaultAccountLockoutTimeSpan = DateTime.Now.AddDays(Convert.ToInt32(defaultAccountLockoutTimeSpanValue));
            MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt32(maxFailedAccessAttemptsBeforeLockoutValue);
        }
        /// <summary>
        /// Get user lock out end date
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }


        /// <summary>
        /// Set user lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            _userRepository.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Increment failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            _userRepository.Update(user);

            return await Task.FromResult(user.AccessFailedCount);
        }
        public async Task<int> UpdateLockedOutSettingAsync(User user)
        {
            user.IsLockoutEnabled = true;
            _userRepository.Update(user);

            return await Task.FromResult(user.AccessFailedCount);
        }
        /// <summary>
        /// Reset failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            _userRepository.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get if lockout is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.IsLockout);
        }

        /// <summary>
        /// Set lockout enabled for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.IsLockout = enabled;
            _userRepository.Update(user);

            return Task.FromResult(0);
        }
    }
}