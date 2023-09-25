using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto
{
    public class ApprovalUserModuleMappingService : ApplicationService, IApprovalUserModuleMappingService
    {
        private readonly IRepository<ApprovalUserModuleMappingMaster> _approvalusermodulemappingrepository;
        private readonly IRepository<ApprovalLevelMaster> _approvallevelrepository;
        private readonly IRepository<ModuleMaster> _modulerepository;
        private readonly IRepository<SubModuleMaster> _submodulerepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubmoduleRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ApprovalUserModuleMappingService(IRepository<ApprovalUserModuleMappingMaster> approvalusermodulemappingrepository,
            IRepository<ApprovalLevelMaster> approvallevelrepository, IRepository<ModuleMaster> modulerepository, IRepository<SubModuleMaster> submodulerepository
            , IRepository<User, long> userRepository
            , IRepository<SubModuleMaster> subModuleRepository,
             IRepository<ModuleSubModule> moduleSubmoduleRepository,
             IRepository<ModuleMaster> moduleRepository)
        {
            _approvalusermodulemappingrepository = approvalusermodulemappingrepository;
            _approvallevelrepository = approvallevelrepository;
            _modulerepository = modulerepository;
            _submodulerepository = submodulerepository;
            _userRepository = userRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            _moduleSubmoduleRepository = moduleSubmoduleRepository;
        }

        public async Task<List<SelectListDto>> GetModulesAsync()
        {
            return await _moduleRepository.GetAll().Where(x => x.IsActive && x.Name == PMMSConsts.WIPModule).OrderBy(x => x.DisplayName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DisplayName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetSubModulesAsync()
        {
            var result = await (from moduleSubModule in _moduleSubmoduleRepository.GetAll()
                                join subModule in _subModuleRepository.GetAll()
                                on moduleSubModule.SubModuleId equals subModule.Id
                                join module in _moduleRepository.GetAll()
                                on moduleSubModule.ModuleId equals module.Id
                                where subModule.IsActive && module.IsActive && module.Name == PMMSConsts.WIPModule
                                && (subModule.DisplayName == PMMSConsts.SubModuleNameForRecipe)
                                //!= PMMSConsts.AdminModule && module.Name != PMMSConsts.MasterModule
                                orderby subModule.DisplayName
                                select new SelectListDto { Id = subModule.Id, Value = subModule.DisplayName }).ToListAsync() ?? default;



            return result;
        }

        public async Task<List<SelectListDto>> GetActivitySubModulesAsync()
        {
            var result = await (from moduleSubModule in _moduleSubmoduleRepository.GetAll()
                                join subModule in _subModuleRepository.GetAll()
                                on moduleSubModule.SubModuleId equals subModule.Id
                                join module in _moduleRepository.GetAll()
                                on moduleSubModule.ModuleId equals module.Id
                                where subModule.IsActive && module.IsActive && module.Name == PMMSConsts.WIPModule
                                && (subModule.DisplayName == PMMSConsts.ActivitySubModuleNameForArea || subModule.DisplayName == PMMSConsts.ActivitySubModuleNameForEquipment)
                                // && subModule.DisplayName != PMMSConsts.AdminModule && module.Name != PMMSConsts.MasterModule
                                orderby subModule.DisplayName
                                select new SelectListDto { Id = subModule.Id, Value = subModule.DisplayName }).ToListAsync() ?? default;



            return result;
        }

        [PMMSAuthorize(Permissions = "ApprovalUserModuleMappingMaster.View")]
        public async Task<ApprovalUserModuleMappingDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _approvalusermodulemappingrepository.GetAsync(input.Id);
            return ObjectMapper.Map<ApprovalUserModuleMappingDto>(entity);
        }

        [PMMSAuthorize(Permissions = "ApprovalUserModuleMappingMaster.View")]
        public async Task<PagedResultDto<ApprovalUserModuleMappingListDto>> GetAllAsync(PagedApprovalUserModuleMapRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ApprovalUserModuleMappingListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = "ApprovalUserModuleMappingMaster.Add")]
        public async Task<ApprovalUserModuleMappingDto> CreateAsync(CreateApprovalUserModuleMappingDto input)
        {
            //if (await _approvalusermodulemappingrepository.GetAll().AnyAsync(x => x.CubicleCode == input.CubicleCode))
            //{
            //    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleCodeAlreadyExist);
            //}
            var approvalUserModule = ObjectMapper.Map<ApprovalUserModuleMappingMaster>(input);
            //cubicle.TenantId = AbpSession.TenantId;
            await _approvalusermodulemappingrepository.InsertAsync(approvalUserModule);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<ApprovalUserModuleMappingDto>(approvalUserModule);
        }

        [PMMSAuthorize(Permissions = "ApprovalUserModuleMappingMaster.Edit")]
        public async Task<ApprovalUserModuleMappingDto> UpdateAsync(ApprovalUserModuleMappingDto input)
        {
            //if (await _cubicleRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.CubicleCode == input.CubicleCode))
            //{
            //    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleCodeAlreadyExist);
            //}
            var approvalUserModule = await _approvalusermodulemappingrepository.GetAsync(input.Id);

            ObjectMapper.Map(input, approvalUserModule);

            await _approvalusermodulemappingrepository.UpdateAsync(approvalUserModule);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = "ApprovalUserModuleMappingMaster.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _approvalusermodulemappingrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _approvalusermodulemappingrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<ApprovalUserModuleMappingListDto> ApplySorting(IQueryable<ApprovalUserModuleMappingListDto> query, PagedApprovalUserModuleMapRequestDto input)
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
        protected IQueryable<ApprovalUserModuleMappingListDto> ApplyPaging(IQueryable<ApprovalUserModuleMappingListDto> query, PagedApprovalUserModuleMapRequestDto input)
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
        protected IQueryable<ApprovalUserModuleMappingListDto> CreateUserListFilteredQuery(PagedApprovalUserModuleMapRequestDto input)
        {
            var ApprovalusermodulemappingQuery = from ApprovalUserModuleMapping in _approvalusermodulemappingrepository.GetAll()
                                                 join ApprovalLevelMaster in _approvallevelrepository.GetAll()
                                                 on ApprovalUserModuleMapping.AppLevelId equals ApprovalLevelMaster.LevelCode into ps
                                                 from ApprovalLevelMaster in ps.DefaultIfEmpty()
                                                 join module in _modulerepository.GetAll()
                                                 on ApprovalUserModuleMapping.ModuleId equals module.Id into areaps
                                                 from module in areaps.DefaultIfEmpty()
                                                 join submodule in _submodulerepository.GetAll()
                                                 on ApprovalUserModuleMapping.SubModuleId equals submodule.Id into submods
                                                 from submodule in submods.DefaultIfEmpty()
                                                 join user in _userRepository.GetAll()
                                                 on ApprovalUserModuleMapping.UserId equals user.Id into u
                                                 from user in u.DefaultIfEmpty()

                                                 select new ApprovalUserModuleMappingListDto
                                                 {
                                                     Id = ApprovalUserModuleMapping.Id,
                                                     AppLevelId = ApprovalUserModuleMapping.AppLevelId,
                                                     UserId = ApprovalUserModuleMapping.UserId,
                                                     UserEnteredAppLevelId = ApprovalLevelMaster.LevelName,
                                                     UserEnteredUserId = user.UserName,
                                                     ModuleId = module.Id,
                                                     SubModuleId = submodule.Id,
                                                     IsActive = ApprovalUserModuleMapping.IsActive,
                                                     ModuleName = module.DisplayName,
                                                     SubModuleName = submodule.DisplayName

                                                 };
            if (input.AppLevelId != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.AppLevelId == input.AppLevelId);
            }
            if (input.UserId != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.UserId == input.UserId);
            }
            if (input.ModuleId != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ModuleId == input.ModuleId);
            }
            if (input.SubModuleId != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.SubModuleId == input.SubModuleId);
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.IsActive);
                }
            }
            return ApprovalusermodulemappingQuery;
        }
    }
}
