using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Roles.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Roles;
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

namespace ELog.Application.Roles
{
    [PMMSAuthorize]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<PermissionMaster> _actionRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubModuleRepository;
        private readonly IRepository<RolePermissions, int> _rolePermissionsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _repository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager,
            IRepository<PermissionMaster> actionRepository, IRepository<ModuleSubModule> moduleSubModuleRepository,
            IRepository<ModuleMaster> moduleRepository, IRepository<SubModuleMaster> subModuleRepository,
            IRepository<RolePermissions, int> rolePermissionsRepository, IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserRole, long> userRoleRepository, IMasterCommonRepository masterCommonRepository,
            IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _repository = repository;
            _roleManager = roleManager;
            _actionRepository = actionRepository;
            _rolePermissionsRepository = rolePermissionsRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            _moduleSubModuleRepository = moduleSubModuleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userRoleRepository = userRoleRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<RoleDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _repository.GetAsync(input.Id);
            var role = MapToEntityDto(entity);
            role.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Role_SubModule);
            role.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return role;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<RoleListDto>> GetAllAsync(PagedRoleResultRequestDto input)
        {
            var query = CreateRoleListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<RoleListDto>(
                totalCount,
                entities);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            var role = ObjectMapper.Map<Role>(input);
            role.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Role_SubModule);
            role.TenantId = AbpSession.TenantId;
            role.SetNormalizedName();
            CheckErrors(await _roleManager.CreateAsync(role).ConfigureAwait(false));

            await SetGrantedPermissionsAsync(role, input.ModulePermissions).ConfigureAwait(false);

            return MapToEntityDto(role);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            if (!(await ValidateRoleIsNotAssignedForInActive(input)))
            {
                throw new UserFriendlyException(422, "Role cannot be de-activated.One or more users are associated with this role.");
            }
            var role = await _roleManager.GetRoleByIdAsync(input.Id).ConfigureAwait(false);
            role.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Role_SubModule, role.ApprovalStatusId);
            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role).ConfigureAwait(false));

            await SetUpdatedGrantedPermissionsAsync(role, input.ModulePermissions).ConfigureAwait(false);

            return MapToEntityDto(role);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var role = await _roleManager.FindByIdAsync(input.Id.ToString()).ConfigureAwait(false);
            if (role != null && await _userRoleRepository.GetAll().AnyAsync(x => x.RoleId == input.Id))
            {
                throw new UserFriendlyException(422, "Role cannot be deleted.Role is associated with one or more users.");
            }
            await ResetAllRolePermissionAsync(role).ConfigureAwait(false);

            CheckErrors(await _roleManager.DeleteAsync(role).ConfigureAwait(false));
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectRoleAsync(ApprovalStatusDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            role.ApprovalStatusId = input.ApprovalStatusId;
            role.ApprovalStatusDescription = input.Description;
            await _roleManager.UpdateAsync(role);
        }

        protected RoleDto MapToEntityDto(Role entity)
        {
            var roleDto = new RoleDto();
            if (entity != null)
            {
                var rolePermissionsDto = new List<RolePermissionsDto>();
                var rolePermissions = _rolePermissionsRepository.GetAllList(x => x.RoleId == entity.Id);
                var superadminRole = _repository.FirstOrDefault(r => r.Name.ToLower() == StaticRoleNames.Tenants.SuperAdmin.ToLower());
                var allModuleSubModules = (from moduleSubModule in _moduleSubModuleRepository.GetAll()
                                           join subModule in _subModuleRepository.GetAll()
                                           on moduleSubModule.SubModuleId equals subModule.Id
                                           join module in _moduleRepository.GetAll()
                                           on moduleSubModule.ModuleId equals module.Id
                                           orderby module.DisplayName, subModule.DisplayName
                                           select new RolePermissionsDto()
                                           {
                                               ModuleSubModuleId = moduleSubModule.Id,
                                               ModuleName = module.DisplayName,
                                               SubModuleName = subModule.DisplayName,
                                           }).ToList();

                foreach (var moduleSubModule in allModuleSubModules)
                {
                    var permissionDto = new RolePermissionsDto();
                    var allPermissions = new List<ActionDto>();
                    var actions = new List<ActionDto>();

                    permissionDto.ModuleName = moduleSubModule?.ModuleName;
                    permissionDto.SubModuleName = moduleSubModule?.SubModuleName;
                    if (permissionDto.SubModuleName == PMMSConsts.ModuleName && permissionDto.SubModuleName == PMMSConsts.SubModuleName)
                    {
                        allPermissions = allPermissions.Where(a => a.Action != PMMSConsts.AddPermission && a.Action == PMMSConsts.DeletePermission).ToList();
                    }
                    else
                    {
                        allPermissions = _actionRepository.GetAll()
                     .Select(x => new ActionDto { Id = x.Id, Action = x.Action, IsGranted = false })?
                     .ToList() ?? default;
                    }
                    moduleSubModule.GrantedPermissions = allPermissions;
                    permissionDto.ModuleSubModuleId = moduleSubModule?.ModuleSubModuleId;
                    var grantedPermissions = rolePermissions.Where(a => a.ModuleSubModuleId == moduleSubModule.ModuleSubModuleId && !a.IsDeleted)
                       .Distinct();
                    permissionDto.IsSuperAdminPermission = _rolePermissionsRepository.FirstOrDefault(x => x.RoleId == superadminRole.Id && x.ModuleSubModuleId == moduleSubModule.ModuleSubModuleId) != null;
                    foreach (var item in allPermissions)
                    {
                        item.PermissionName = permissionDto.SubModuleName + "." + (item?.Action);
                        if (grantedPermissions.Any(a => a.PermissionId == item.Id))
                        {
                            item.IsGranted = true;
                        }
                        actions.Add(item);
                    }
                    permissionDto.IsGranted = allPermissions.All(x => x.IsGranted);
                    permissionDto.GrantedPermissions = actions;
                    rolePermissionsDto.Add(permissionDto);
                }

                roleDto.ApprovalStatusDescription = entity.ApprovalStatusDescription;
                roleDto.Description = entity.Description;
                roleDto.DisplayName = entity.DisplayName;
                roleDto.Name = entity.Name;
                roleDto.Description = entity.Description;
                roleDto.IsDeleted = entity.IsDeleted;
                roleDto.IsActive = entity.IsActive;
                roleDto.ModulePermissions = rolePermissionsDto;
                roleDto.Id = entity.Id;
                roleDto.isSuperAdminRole = entity.Id == superadminRole.Id;
            }
            return roleDto;
        }

        protected IQueryable<RoleListDto> CreateRoleListFilteredQuery(PagedRoleResultRequestDto input)
        {
            var query = from role in _repository.GetAll()
                        join approvalStatus in _approvalStatusRepository.GetAll()
                            on role.ApprovalStatusId equals approvalStatus.Id into paStatus
                        from approvalStatus in paStatus.DefaultIfEmpty()
                        select new RoleListDto
                        {
                            ApprovalStatusId = role.ApprovalStatusId,
                            Description = role.Description,
                            DisplayName = role.DisplayName,
                            Id = role.Id,
                            IsActive = role.IsActive,
                            Name = role.Name,
                            UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                        };

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                query = query.Where(x => x.Name.Contains(input.Keyword)
                                            || x.DisplayName.Contains(input.Keyword)
                                            || x.Description.Contains(input.Keyword));
            }

            if (input.ApprovalStatusId != null)
            {
                query = query.Where(x => x.ApprovalStatusId == input.ApprovalStatusId.Value);
            }

            if (input.Status != null)
            {
                if (input.Status == 2)
                {
                    query = query.Where(x => !x.IsActive);
                }
                else if (input.Status == 1)
                {
                    query = query.Where(x => x.IsActive);
                }
            }
            return query;
        }

        //Only used for Create

        public async Task<List<ActionDto>> GetAllPermisionNameAsync()
        {
            var permissions = await _actionRepository.GetAll()
                          .Select(x => new ActionDto { Id = x.Id, Action = x.Action })?
                          .ToListAsync() ?? default;
            return permissions;

        }

        public async Task<List<RolePermissionsDto>> GetAllSubModulesWithPermissions()
        {
            var permissions = await _actionRepository.GetAll()
                          .Select(x => new ActionDto { Id = x.Id, Action = x.Action, IsGranted = false })?
                          .ToListAsync() ?? default;

            var moduleSubModulePermissions = permissions;

            foreach (var permission in moduleSubModulePermissions)
            {
                permission.IsGranted = false;
            }

            var result = await (from moduleSubModule in _moduleSubModuleRepository.GetAll()
                                join subModule in _subModuleRepository.GetAll()
                                on moduleSubModule.SubModuleId equals subModule.Id
                                join module in _moduleRepository.GetAll()
                                on moduleSubModule.ModuleId equals module.Id
                                orderby module.DisplayName, subModule.DisplayName
                                select new RolePermissionsDto()
                                {
                                    ModuleSubModuleId = moduleSubModule.Id,
                                    ModuleName = module.DisplayName,
                                    SubModuleName = subModule.DisplayName,
                                    GrantedPermissions = permissions
                                }).ToListAsync();

            foreach (var item in result)
            {
                if (item.SubModuleName == PMMSConsts.SubModuleName || item.SubModuleName == PMMSConsts.ModuleName)
                {
                    item.GrantedPermissions = moduleSubModulePermissions;
                }
                var superadminRole = _repository.FirstOrDefault(r => r.Name.ToLower() == StaticRoleNames.Tenants.SuperAdmin.ToLower());
                item.IsSuperAdminPermission = _rolePermissionsRepository.FirstOrDefault(x => x.RoleId == superadminRole.Id && x.ModuleSubModuleId == item.ModuleSubModuleId) != null;
            }
            return result;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Role_SubModule + "." + PMMSPermissionConst.View)]
        protected async Task<Role> GetEntityByIdAsync(int id)
        {
            return await _repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        private async Task<string> GetPermissionNameByIdAsync(int id)
        {
            var permission = await _actionRepository.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return permission?.Action;
        }

        private async Task<string> GetSubModuleNameByIdAsync(int id)
        {
            var moduleSubModulesDetails = await _moduleSubModuleRepository.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
            if (moduleSubModulesDetails != null)
            {
                var subModule = await _subModuleRepository.FirstOrDefaultAsync(x => x.Id == moduleSubModulesDetails.SubModuleId).ConfigureAwait(false);
                return subModule.Name;
            }
            return null;
        }

        protected IQueryable<RoleListDto> ApplySorting(IQueryable<RoleListDto> query, PagedRoleResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<RoleListDto> ApplyPaging(IQueryable<RoleListDto> query, PagedRoleResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected async Task SetGrantedPermissionsAsync(Role role, List<RolePermissionsDto> rolePermissionDto)
        {
            var rolePermissions = new List<RolePermissions>();
            foreach (var permission in rolePermissionDto)
            {
                foreach (var action in permission.GrantedPermissions)
                {
                    if (action.IsGranted)
                    {
                        rolePermissions.Add(new RolePermissions() { PermissionId = (int)action.Id, ModuleSubModuleId = permission.ModuleSubModuleId.Value });
                    }
                }
            }
            await AddRolePermissionAsync(role, rolePermissions).ConfigureAwait(false);
        }

        protected async Task SetUpdatedGrantedPermissionsAsync(Role role, List<RolePermissionsDto> rolePermissionDto)
        {
            foreach (var permission in rolePermissionDto)
            {
                var oldActions = (await _rolePermissionsRepository.GetAllListAsync().ConfigureAwait(false))
                                    .Where(a => a.RoleId == role.Id && a.ModuleSubModuleId == permission.ModuleSubModuleId)
                                    .Select(p => p.PermissionId).ToList();

                var permissionsIds = permission.GrantedPermissions.Where(a => a.IsGranted).ToList().ConvertAll(a => Convert.ToInt32(a.Id));
                var matchActions = oldActions.Intersect(permissionsIds);
                var actionToRemove = oldActions.Except(matchActions);
                var actionToAdd = permissionsIds.Except(matchActions);

                foreach (var action in actionToAdd)
                {
                    var rolePermissions = new List<RolePermissions>
                    {
                        new RolePermissions() { PermissionId = action, ModuleSubModuleId = permission.ModuleSubModuleId.Value }
                    };
                    await AddRolePermissionAsync(role, rolePermissions).ConfigureAwait(false);
                }
                foreach (var action in actionToRemove)
                {
                    var rolePermissions = new List<RolePermissions>
                    {
                        new RolePermissions() { PermissionId = action, ModuleSubModuleId = permission.ModuleSubModuleId.Value }
                    };
                    await RemoveRolePermissionAsync(role, rolePermissions).ConfigureAwait(false);
                }
            }
        }

        private async Task AddRolePermissionAsync(Role role, List<RolePermissions> permissionGrant)
        {
            using (_unitOfWorkManager.Current.SetTenantId(role.TenantId))
            {
                foreach (var item in permissionGrant)
                {
                    var permissionName = await GetPermissionNameByIdAsync(item.PermissionId).ConfigureAwait(false);
                    var subModuleName = await GetSubModuleNameByIdAsync(item.ModuleSubModuleId).ConfigureAwait(false);
                    await _rolePermissionsRepository.InsertAsync(new RolePermissions
                    {
                        ModuleSubModuleId = item.ModuleSubModuleId,
                        PermissionId = item.PermissionId,
                        RoleId = role.Id,
                        IsDeleted = false,
                        PermissionName = subModuleName != null ? subModuleName + "." + permissionName : null,
                    }).ConfigureAwait(false);
                }
                await _unitOfWorkManager.Current.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task ResetAllRolePermissionAsync(Role role)
        {
            var rolePermissions = await _rolePermissionsRepository.GetAllListAsync(p => p.RoleId == role.Id).ConfigureAwait(false);

            foreach (var item in rolePermissions)
            {
                item.IsDeleted = true;
                await _rolePermissionsRepository.DeleteAsync(item).ConfigureAwait(false);
            }
        }

        private async Task RemoveRolePermissionAsync(Role role, List<RolePermissions> rolePermissions)
        {
            foreach (var row in rolePermissions)
            {
                var grantedPermission = await _rolePermissionsRepository.FirstOrDefaultAsync(
                            p => p.RoleId == role.Id &&
                            p.ModuleSubModuleId == row.ModuleSubModuleId &&
                            p.PermissionId == row.PermissionId &&
                                 !p.IsDeleted
                            ).ConfigureAwait(false);
                if (grantedPermission != null)
                {
                    grantedPermission.IsDeleted = true;
                    await _rolePermissionsRepository.DeleteAsync(grantedPermission).ConfigureAwait(false);
                }
            }
        }

        private async Task<bool> ValidateRoleIsNotAssignedForInActive(RoleDto input)
        {
            return input.IsActive || !await _userRoleRepository.GetAll().AnyAsync(r => r.RoleId == input.Id);
        }
    }
}