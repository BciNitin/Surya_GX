using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.PRNEntry.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Masters.PRNEntry
{
    [PMMSAuthorize]
    public class PRNEntryService : ApplicationService, IPRNEntryService
    {
        private readonly IRepository<PRNEntryMaster> _prnEntryRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public PRNEntryService(IRepository<PRNEntryMaster> prnEntryRepository, IRepository<PlantMaster> plantRepository, IRepository<ModuleMaster> moduleRepository,
           IRepository<SubModuleMaster> subModuleRepository,
          IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor,
                    IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _prnEntryRepository = prnEntryRepository;
            _plantRepository = plantRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _httpContextAccessor = httpContextAccessor;
            _approvalStatusRepository = approvalStatusRepository;
        }
        [PMMSAuthorize(Permissions = "PRNFileMaster.View")]
        public async Task<PRNEntryDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _prnEntryRepository.GetAsync(input.Id);
            var prnEntry = ObjectMapper.Map<PRNEntryDto>(entity);
            prnEntry.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.PrnEntry_SubModule);
            prnEntry.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return prnEntry;

        }

        [PMMSAuthorize(Permissions = "PRNFileMaster.View")]
        public async Task<PagedResultDto<PRNEntryListDto>> GetAllAsync(PagedPRNEntryResultRequestDto input)
        {
            var query = CreatePrnFileListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PRNEntryListDto>(
                totalCount,
                entities
            );
        }
        [PMMSAuthorize(Permissions = "PRNFileMaster.Add")]
        public async Task<PRNEntryDto> CreateAsync(CreatePRNEntryDto input)
        {
            if (await _prnEntryRepository.GetAll().AnyAsync(x => x.PRNFileName == input.PRNFileName))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PrnNameAlreadyExist);
            }
            var prnentry = ObjectMapper.Map<PRNEntryMaster>(input);
            var currentDate = DateTime.UtcNow;
            prnentry.TenantId = AbpSession.TenantId;
            prnentry.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.PrnEntry_SubModule);
            await _prnEntryRepository.InsertAsync(prnentry);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PRNEntryDto>(prnentry);
        }

        [PMMSAuthorize(Permissions = "PRNFileMaster.Edit")]
        public async Task<PRNEntryDto> UpdateAsync(PRNEntryDto input)
        {

            var prnfile = await _prnEntryRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, prnfile);

            await _prnEntryRepository.UpdateAsync(prnfile);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = "PRNFileMaster.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var prnentry = await _prnEntryRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _prnEntryRepository.DeleteAsync(prnentry).ConfigureAwait(false);
        }
        protected IQueryable<PRNEntryListDto> ApplySorting(IQueryable<PRNEntryListDto> query, PagedPRNEntryResultRequestDto input)
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
        protected IQueryable<PRNEntryListDto> ApplyPaging(IQueryable<PRNEntryListDto> query, PagedPRNEntryResultRequestDto input)
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

        [PMMSAuthorize(Permissions = "PRNFileMaster.Approver")]
        public async Task ApproveOrRejectActivityAsync(ApprovalStatusDto input)
        {

            var prnfileentry = await _prnEntryRepository.GetAsync(input.Id);
            prnfileentry.ApprovalStatusId = input.ApprovalStatusId;
            prnfileentry.ApprovalStatusDescription = input.Description;
            await _prnEntryRepository.UpdateAsync(prnfileentry);
        }

        protected IQueryable<PRNEntryListDto> CreatePrnFileListFilteredQuery(PagedPRNEntryResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var prnentryQuery = from prnentry in _prnEntryRepository.GetAll()
                                join subPlant in _plantRepository.GetAll()
                                 on prnentry.PlantId equals subPlant.Id into prnentrysp
                                from subPlant in prnentrysp.DefaultIfEmpty()
                                join module in _moduleRepository.GetAll()
                                on prnentry.ModuleId equals module.Id into activityms
                                from module in activityms.DefaultIfEmpty()
                                join subModule in _subModuleRepository.GetAll()
                                on prnentry.SubModuleId equals subModule.Id into activitysms
                                from subModule in activitysms.DefaultIfEmpty()
                                join approvalStatus in _approvalStatusRepository.GetAll()
                                on prnentry.ApprovalStatusId equals approvalStatus.Id into paStatus
                                from approvalStatus in paStatus.DefaultIfEmpty()
                                select new PRNEntryListDto
                                {
                                    Id = prnentry.Id,
                                    PRNFileName = prnentry.PRNFileName,
                                    SubPlantId = prnentry.PlantId,
                                    UserEnteredSubPlantId = subPlant.PlantId,
                                    ModuleId = module.Id,
                                    UserEnteredModuleId = module.Name,
                                    SubModuleId = subModule.Id,
                                    UserEnteredSubModuleId = subModule.Name,
                                    IsActive = prnentry.IsActive,
                                    ApprovalStatusId = prnentry.ApprovalStatusId,
                                    UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                };

            if (input.SubPlantId != null)
            {
                prnentryQuery = prnentryQuery.Where(x => x.SubPlantId == input.SubPlantId);
            }

            if (input.ModuleId != null)
            {
                prnentryQuery = prnentryQuery.Where(x => x.ModuleId == input.ModuleId);
            }
            if (input.SubModuleId != null)
            {
                prnentryQuery = prnentryQuery.Where(x => x.SubModuleId == input.SubModuleId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                prnentryQuery = prnentryQuery.Where(x => x.PRNFileName.Contains(input.Keyword)
                || x.PRNFileName.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    prnentryQuery = prnentryQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    prnentryQuery = prnentryQuery.Where(x => x.IsActive);
                }
            }
            return prnentryQuery;
        }
    }
}
