using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.Modules.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Modules
{
    [PMMSAuthorize]
    public class ModuleAppService : ApplicationService, IModuleAppService
    {
        private readonly IRepository<ModuleMaster> _moduleMasterRepository;
        private readonly IRepository<SubModuleMaster> _subModuleMasterRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubModuleRepository;
        private readonly IRepository<SubModuleTypeMaster> _subModuleTypeRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ModuleAppService(IRepository<ModuleMaster> moduleMasterRepository,
            IRepository<SubModuleMaster> subModuleMasterRepository,
             IRepository<ModuleSubModule> moduleSubmoduleRepository, IRepository<SubModuleTypeMaster> subModuleTypeRepository)
        {
            _moduleMasterRepository = moduleMasterRepository;
            _subModuleMasterRepository = subModuleMasterRepository;
            _moduleSubModuleRepository = moduleSubmoduleRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _subModuleTypeRepository = subModuleTypeRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Module + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<ModuleListDto>> GetAllModuleAsync(PagedModuleResultRequestDto input)
        {
            var query = CreateModuleListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query).ConfigureAwait(false);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query).ConfigureAwait(false);

            return new PagedResultDto<ModuleListDto>(
                totalCount,
                entities.ConvertAll(MapToEntityDto));
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<SubModuleListDto>> GetAllSubModuleAsync(PagedSubModuleResultRequestDto input)
        {
            var query = CreateSubModuleListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<SubModuleListDto>(
                totalCount,
                entities);
        }

        protected ModuleListDto MapToEntityDto(ModuleMaster entity)
        {
            return ObjectMapper.Map<ModuleListDto>(entity);
        }

        protected IQueryable<ModuleMaster> CreateModuleListFilteredQuery(PagedModuleResultRequestDto input)
        {
            var query = _moduleMasterRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));

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
            return query.AsQueryable();
        }

        protected IQueryable<SubModuleListDto> CreateSubModuleListFilteredQuery(PagedSubModuleResultRequestDto input)
        {
            var query = from moduleSubModule in _moduleSubModuleRepository.GetAll()
                        join subModule in _subModuleMasterRepository.GetAll()
                        on moduleSubModule.SubModuleId equals subModule.Id
                        join module in _moduleMasterRepository.GetAll()
                        on moduleSubModule.ModuleId equals module.Id
                        join master in _subModuleTypeRepository.GetAll()
                        on subModule.SubModuleTypeId equals master.Id
                        select new SubModuleListDto
                        {
                            Id = subModule.Id,
                            Name = subModule.Name,
                            DisplayName = subModule.DisplayName,
                            SubModuleType = master.SubModuleType,
                            IsActive = subModule.IsActive,
                            ModuleName = module.DisplayName,
                            ModuleId = module.Id,
                            UserEnteredApprovalRequired = subModule.IsApprovalRequired ? "Yes" : "No",
                            IsApprovalRequired = subModule.IsApprovalRequired
                        };

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
            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                query = query.Where(x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword));
            }
            if (input.ModuleId != null)
            {
                query = query.Where(x => x.ModuleId == input.ModuleId);
            }
            if (input.ApprovalRequired != null)
            {
                if (input.ApprovalRequired == (int)ApprovalRequireEnum.Yes)
                {
                    query = query.Where(x => x.IsApprovalRequired);
                }
                else if (input.ApprovalRequired == (int)ApprovalRequireEnum.No)
                {
                    query = query.Where(x => !x.IsApprovalRequired);
                }
            }
            return query;
        }

        protected IQueryable<ModuleMaster> ApplySorting(IQueryable<ModuleMaster> query, PagedModuleResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
                return query.OrderBy(sortInput.Sorting);

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
        protected IQueryable<ModuleMaster> ApplyPaging(IQueryable<ModuleMaster> query, PagedModuleResultRequestDto input)
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

        protected IQueryable<SubModuleListDto> ApplySorting(IQueryable<SubModuleListDto> query, PagedSubModuleResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
                return query.OrderBy(sortInput.Sorting);

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
        protected IQueryable<SubModuleListDto> ApplyPaging(IQueryable<SubModuleListDto> query, PagedSubModuleResultRequestDto input)
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

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Module + "." + PMMSPermissionConst.Edit)]
        public async Task<ModuleDto> Update(ModuleDto input)
        {
            var lowerCaseName = input.Name.ToLower();
            if (_moduleMasterRepository.GetAll().Any(m => m.Id != input.Id && m.Name.ToLower() == lowerCaseName))
            {
                throw new UserFriendlyException(422, "Module name already exist.");
            }
            var moduleMaster = await _moduleMasterRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, moduleMaster);

            var updatedModuleMaster = await _moduleMasterRepository.UpdateAsync(moduleMaster);
            var moduleSubMOduleInput = new ModuleSubModuleDto
            {
                Id = input.Id,
                SubModule = input.SubModules
            };
            await AssignSubModules(moduleSubMOduleInput).ConfigureAwait(false);
            return ObjectMapper.Map<ModuleDto>(updatedModuleMaster);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Module + "." + PMMSPermissionConst.View)]
        public async Task<ModuleDto> Get(EntityDto<int> input)
        {
            var subModules = await GetSubModules(input).ConfigureAwait(false);
            return await _moduleMasterRepository.GetAll()
                .Select(module => new ModuleDto
                {
                    Id = module.Id,
                    Description = module.Description,
                    DisplayName = module.DisplayName,
                    Name = module.Name,
                    SubModules = subModules,
                    IsActive = module.IsActive
                })
                .FirstOrDefaultAsync(x => x.Id == input.Id);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<SubModuleDto>> GetSubModules(EntityDto<int> input)
        {
            return await (from moduleSubModule in _moduleSubModuleRepository.GetAll()
                          join subModule in _subModuleMasterRepository.GetAll()
                          on moduleSubModule.SubModuleId equals subModule.Id
                          where moduleSubModule.ModuleId == input.Id && subModule.IsActive
                          orderby subModule.DisplayName
                          select new SubModuleDto
                          {
                              Name = subModule.Name,
                              DisplayName = subModule.DisplayName,
                              Description = subModule.Description,
                              Sequence = subModule.Sequence,
                              SubModuleTypeId = subModule.SubModuleTypeId,
                              Id = subModule.Id,
                              IsDeleted = moduleSubModule.IsDeleted,
                              IsMandatory = subModule.SubModuleTypeId == (int)PMMSEnums.SubModuleType.Mandatory,
                              IsActive = subModule.IsActive,
                              IsSelected = moduleSubModule.IsSelected,
                              IsApprovalRequired = subModule.IsApprovalRequired,
                          }).ToListAsync().ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SubModule + "." + PMMSPermissionConst.View)]
        public async Task<SubModuleDto> GetSubModule(int Id)
        {
            return await _subModuleMasterRepository.GetAll()
                             .Where(x => x.Id == Id)
                             .Select(x => new SubModuleDto
                             {
                                 Id = x.Id,
                                 Description = x.Description,
                                 DisplayName = x.DisplayName,
                                 Name = x.Name,
                                 IsActive = x.IsActive,
                                 IsSelected = x.SubModuleTypeId == (int)PMMSEnums.SubModuleType.Mandatory,
                                 IsMandatory = x.SubModuleTypeId == (int)PMMSEnums.SubModuleType.Mandatory,
                                 Sequence = x.Sequence,
                                 SubModuleTypeId = x.SubModuleTypeId,
                                 IsApprovalRequired = x.IsApprovalRequired,
                                 IsApprovalWorkflowRequired = x.IsApprovalWorkflowRequired
                             })
                             .FirstOrDefaultAsync();
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<UpdateSubModuleDto> UpdateSubModule(UpdateSubModuleDto input)
        {
            var subModuleMaster = await _subModuleMasterRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, subModuleMaster);
            var updatedSubModuleMaster = await _subModuleMasterRepository.UpdateAsync(subModuleMaster);

            return ObjectMapper.Map<UpdateSubModuleDto>(updatedSubModuleMaster);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Module + "." + PMMSPermissionConst.Edit)]
        public async Task<ModuleSubModuleDto> AssignSubModules(ModuleSubModuleDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                ValidateModuleSubModuleDto(input);
                await ManageModuleSubModule(input);
                return input;
            }
        }

        private void ValidateModuleSubModuleDto(ModuleSubModuleDto input)
        {
            if (input.Id <= 0 || input.SubModule.Select(a => a.Id).Any(x => x <= 0))
            {
                throw new UserFriendlyException(422, "Module or Sub-Module Id cannot be zero");
            }
            if (!_moduleMasterRepository.GetAll().Any(x => x.Id == input.Id) ||
                _subModuleMasterRepository.GetAll().Where(x => input.SubModule.Select(a => a.Id)
                .Contains(x.Id)).Count() != input.SubModule.Count)
            {
                throw new UserFriendlyException(422, "Module or Sub-Module Id does not exist");
            }
        }

        protected async Task ManageModuleSubModule(ModuleSubModuleDto input)
        {
            var lstModuleExists = await _moduleSubModuleRepository.GetAll()
                                .Where(module => module.ModuleId == input.Id && !module.IsMandatory)
                                .ToListAsync();

            foreach (var moduleSubModuleToUpdate in lstModuleExists)
            {
                var isPresent = input.SubModule.Find(a => a.Id == moduleSubModuleToUpdate.SubModuleId);
                if (isPresent != null)
                {
                    moduleSubModuleToUpdate.IsSelected = isPresent.IsSelected;
                }
                await _moduleSubModuleRepository.UpdateAsync(moduleSubModuleToUpdate);
            }
        }
        public async Task<int> GetModuleByName(string moduleName)
        {
            if (moduleName != null)
            {
                moduleName = moduleName.Trim().ToLower();
                var module = await _moduleMasterRepository.GetAll().Where(a => a.Name == moduleName).OrderBy(x => x.DisplayName)
                      .FirstOrDefaultAsync();
                return module.Id;
            }
            return default;
        }

        public async Task<int> GetSubmoduleByName(string submoduleName)
        {
            if (submoduleName != null)
            {
                submoduleName = submoduleName.Trim().ToLower();
                var submodule = await _subModuleMasterRepository.GetAll().Where(a => a.Name == submoduleName).OrderBy(x => x.DisplayName)
                      .FirstOrDefaultAsync();
                return submodule.Id;
            }
            return default;
        }
    }
}