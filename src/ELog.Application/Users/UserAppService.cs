using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Sessions;
using ELog.Application.Users.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Users
{
    [PMMSAuthorize]
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPlants> _userPlantRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly ISessionAppService _sessionAppService;
        private readonly IRepository<Setting, long> _settingRepository;

        public UserAppService(
           IRepository<User, long> userRepository,
           UserManager userManager,
           IRepository<Role> roleRepository, IRepository<UserPlants> userPlantRepository,
          IMasterCommonRepository masterCommonRepository,
          IRepository<ApprovalStatusMaster> approvalStatusRepository,
          IRepository<Setting, long> settingRepository,
          ISessionAppService sessionAppService)

        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _userPlantRepository = userPlantRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _settingRepository = settingRepository;
            _sessionAppService = sessionAppService;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<UserDto> GetAsync(EntityDto<long> input)
        {
            return await GetUserById(input);
        }

        [PMMSAuthorize]
        public async Task<UserDto> GetUserProfileAsync(EntityDto<long> input)
        {
            return await GetUserById(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<UsersListDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var userEntities = entities.Select(x => x.UserListDto)?.ToList();
            return new PagedResultDto<UsersListDto>(
                totalCount,
                userEntities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.Add)]



        public async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            var userConfiguration = await ValidateUserCreationLimit();
            if (!userConfiguration.IsUserCreationAllowed)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.UserCreationLimitExceeded, Convert.ToString(userConfiguration.UserCreationMaxValue)));
            }
            var user = ObjectMapper.Map<User>(input);
            user.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.User_SubModule);
            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.Name = input.FirstName;
            user.Surname = input.LastName;
            user.PhoneNumber = input.PhoneNumber;
            user.EmailAddress = input.Email;
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId).ConfigureAwait(false);
            try
            {
                user.IsLockout = false;
                CheckErrors(await _userManager.CreateAsync(user, input.Password).ConfigureAwait(false));

                if (!input.IsActive)
                {
                    await _userManager.UpdateIsActive(user, input.IsActive);
                }
                if (input.Plants != null)
                {
                    await AddUpdateUserPlantsAsync(user, input.Plants);
                }
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Message.Contains("User name"))
                {
                    string newMessage = ex.Message.Replace("User name", "Username");
                    throw new UserFriendlyException(422, newMessage);
                }
                else
                {
                    throw new UserFriendlyException(422, ex.Message);
                }
            }

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames).ConfigureAwait(false));
            }

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<UserDto>(user);
        }

        private async Task AddUpdateUserPlantsAsync(User user, List<int?> plants)
        {
            var oldPlants = (await _userPlantRepository.GetAllListAsync().ConfigureAwait(false))
                    .Where(a => a.UserId == user.Id)
                    .Select(p => p.PlantId);

            List<int?> oldPlantsList = oldPlants.Cast<int?>().ToList();
            var matchPlants = oldPlantsList.Intersect(plants);
            var plantToRemove = oldPlantsList.Except(matchPlants);
            var plantToAdd = plants.Except(matchPlants);

            var userId = await _userManager.GetUserIdAsync(user);
            if (userId != null)
            {
                foreach (var plant in plantToAdd)
                {
                    var userPlant = new UserPlants()
                    {
                        UserId = Convert.ToInt32(userId),
                        PlantId = (int)plant,
                    };
                    await _userPlantRepository.InsertAsync(userPlant);
                }
                foreach (var plant in plantToRemove)
                {
                    var isPlantExists = _userPlantRepository.GetAll().Where(a => a.UserId == Convert.ToInt32(userId) && a.PlantId == plant).FirstOrDefault();
                    if (isPlantExists != null)
                    {
                        isPlantExists.IsDeleted = true;
                        await _userPlantRepository.UpdateAsync(isPlantExists);
                    }
                }
            }
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<UserDto> UpdateAsync(UserDto input)
        {
            return await UpdateUser(input).ConfigureAwait(false);
        }

        [PMMSAuthorize]
        public async Task<UserDto> UpdateUserProfileAsync(UserDto input)
        {
            return await UpdateUser(input).ConfigureAwait(false);
        }

        private async Task<UserConfigurationDto> ValidateUserCreationLimit()
        {
            var userCreationLimitSetting = await _settingRepository.GetAll().Where(a => a.Name.ToLower() == PMMSConsts.UserCreationLimitSetting.ToLower() && a.TenantId == AbpSession.TenantId).FirstOrDefaultAsync();
            var userConfiguration = new UserConfigurationDto();
            var decodedUserCreationMaxValue = userCreationLimitSetting != null ? EncryptDecryptHelper.Decrypt(userCreationLimitSetting.Value) : null;
            var allActiveUserCount = await _userRepository.GetAll().Where(a => a.IsActive).CountAsync();
            if (allActiveUserCount < Convert.ToInt32(decodedUserCreationMaxValue))
            {
                userConfiguration.IsUserCreationAllowed = true;
            }
            else
            {
                userConfiguration.IsUserCreationAllowed = false;
                userConfiguration.UserCreationMaxValue = Convert.ToInt32(decodedUserCreationMaxValue);
            }
            return userConfiguration;
        }

        private async Task<UserDto> UpdateUser(UserDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id).ConfigureAwait(false);
            var userConfiguration = await ValidateUserCreationLimit();
            if (!input.ActiveInactiveStatusOfUser && input.IsActive && !userConfiguration.IsUserCreationAllowed)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.UserCreationLimitExceeded, Convert.ToString(userConfiguration.UserCreationMaxValue)));
            }
            user.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.User_SubModule, user.ApprovalStatusId);
            ObjectMapper.Map(input, user);
            user.Name = input.FirstName;
            user.Surname = input.LastName;
            user.PhoneNumber = input.PhoneNumber;
            user.EmailAddress = input.Email;
            try
            {
                CheckErrors(await _userManager.UpdateAsync(user).ConfigureAwait(false));
                if (input.Plants != null)
                {
                    await AddUpdateUserPlantsAsync(user, input.Plants);
                }
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Message.Contains("User name"))
                {
                    string newMessage = ex.Message.Replace("User name", "Username");
                    throw new UserFriendlyException(422, newMessage);
                }
            }

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames).ConfigureAwait(false));
            }

            return await GetAsync(input).ConfigureAwait(false);
        }

        private async Task<UserDto> GetUserById(EntityDto<long> input)
        {
            var entity = await _userRepository.GetAsync(input.Id);
            var userDto = ObjectMapper.Map<UserDto>(entity);
            userDto.FirstName = entity.Name;
            userDto.LastName = entity.Surname;
            userDto.Email = entity.EmailAddress;
            userDto.CreatedOn = entity.CreationTime;
            var rolesList = await _userManager.GetRolesAsync(entity);
            userDto.RoleNames = rolesList.ToArray();
            userDto.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.User_SubModule);
            userDto.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return userDto;
        }

        private async Task DeleteAssociatedPlants(User user)
        {
            var plants = await _userPlantRepository.GetAllListAsync(p => p.UserId == user.Id).ConfigureAwait(false);

            foreach (var plant in plants)
            {
                plant.IsDeleted = true;
                await _userPlantRepository.DeleteAsync(plant).ConfigureAwait(false);
            }
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id).ConfigureAwait(false);
            await DeleteAssociatedPlants(user).ConfigureAwait(false);
            await _userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.User_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectUserAsync(ApprovalStatusDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);

            user.ApprovalStatusId = input.ApprovalStatusId;
            user.ApprovalStatusDescription = input.Description;
            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<UserListInternalDto> ApplySorting(IQueryable<UserListInternalDto> query, PagedUserResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.UserListDto.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<UserListInternalDto> ApplyPaging(IQueryable<UserListInternalDto> query, PagedUserResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected UsersListDto MapToEntityDto(UsersListDto entity)
        {
            return ObjectMapper.Map<UsersListDto>(entity);
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected IQueryable<UserListInternalDto> CreateUserListFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = from user in _userRepository.GetAllIncluding(a => a.UserPlants).Where(x => x.UserName.ToLower() != "admin" && x.UserName.ToLower() != PMMSConsts.SuperAdminUserName)
                        join approvalStatus in _approvalStatusRepository.GetAll()
                            on user.ApprovalStatusId equals approvalStatus.Id into paStatus
                        from approvalStatus in paStatus.DefaultIfEmpty()
                        select new UserListInternalDto
                        {
                            DesignationId = user.DesignationId,
                            ModeId = user.ModeId,
                            UserPlants = user.UserPlants,
                            UserListDto = new UsersListDto
                            {
                                ApprovalStatusId = user.ApprovalStatusId,
                                CreationTime = user.CreationTime,
                                Id = user.Id,
                                IsActive = user.IsActive,
                                UserEnteredApprovalStatus = approvalStatus.ApprovalStatus,
                                UserName = user.UserName,
                                FirstName = user.Name,
                                LastName = user.Surname,
                                PswdResetDate = user.PasswordResetTime,
                            }
                        };
            if (input.PlantId != null)
            {
                query = query.Where(x => x.UserPlants.Any(x => x.PlantId == input.PlantId));
            }
            if (input.CreationFromTime != null && input.CreationToTime != null)
            {
                var startDate = input.CreationFromTime.Value.StartOfDay();
                var endDate = input.CreationToTime.Value.EndOfDay();
                query = query.Where(x => x.UserListDto.CreationTime >= startDate && x.UserListDto.CreationTime <= endDate);
            }
            if (input.DesignationId != null)
            {
                query = query.Where(x => x.DesignationId == input.DesignationId);
            }
            if (input.ApprovalStatusId != null)
            {
                query = query.Where(x => x.UserListDto.ApprovalStatusId == input.ApprovalStatusId.Value);
            }
            if (input.ModeId != null)
            {
                query = query.Where(x => x.ModeId == input.ModeId);
            }
            if (!(string.IsNullOrEmpty(input.UserName) || string.IsNullOrWhiteSpace(input.UserName)))
            {
                query = query.Where(x => x.UserListDto.UserName.Contains(input.UserName));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    query = query.Where(x => !x.UserListDto.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    query = query.Where(x => x.UserListDto.IsActive);
                }
            }
            return query;
        }

        public async Task<RoleCheckboxDto> GetAllRoles()
        {
            RoleCheckboxDto roleCheckboxDto = new RoleCheckboxDto
            {
                Name = "All",
                DisplayName = "Select All Roles"
            };
            string approvedApprovalStatus = ApprovalStatus.Approved.ToString().ToLower();
            roleCheckboxDto.UserRoles = await (from role in _roleRepository.GetAll()
                                               join approvalStatus in _approvalStatusRepository.GetAll()
                                               on role.ApprovalStatusId equals approvalStatus.Id
                                               where (role.IsActive && approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus && role.Name.ToLower() != StaticRoleNames.Tenants.SuperAdmin.ToLower())
                                               select new RoleCheckboxDto { Name = role.Name, DisplayName = role.DisplayName })?.ToListAsync();
            return roleCheckboxDto;
        }

        public async Task AddOrUpdateUserCreationLimitAsync(long noOfUsers)
        {
            var sessionUser = await _sessionAppService.GetCurrentLoginInformations();
            var isCurrentLoggedInUserSuperAdmin = sessionUser.User.RoleNames.Any(x => x.ToLower() == PMMSConsts.SuperAdminUserName.ToLower());
            if (!isCurrentLoggedInUserSuperAdmin)
            {
                throw new UserFriendlyException(PMMSValidationConst.CurrentUserIsNotSuperAdmin);
            }
            int? tenantId = AbpSession.TenantId;
            const string userConfigurationMaxValueSettingName = PMMSConsts.UserCreationLimitSetting;
            var userCreationLimitSettingValue = EncryptDecryptHelper.Encrypt(Convert.ToString(noOfUsers));

            var userCreationLimitSetting = await _settingRepository.GetAll().Where(a => a.Name.ToLower() == userConfigurationMaxValueSettingName.ToLower() && a.TenantId == tenantId).FirstOrDefaultAsync();

            if (userCreationLimitSetting == null)
            {
                await _settingRepository.InsertAsync(new Setting(tenantId, null, userConfigurationMaxValueSettingName, userCreationLimitSettingValue));
            }
            else
            {
                userCreationLimitSetting.Value = userCreationLimitSettingValue;
                await _settingRepository.UpdateAsync(userCreationLimitSetting);
            }
        }
    }
}