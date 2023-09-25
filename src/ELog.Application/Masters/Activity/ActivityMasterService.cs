using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.Activity.Dto;
using ELog.Application.CommonDto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Activity
{
    [PMMSAuthorize]


    public class ActivityMasterService : ApplicationService, IActivityMasterService
    {
        private readonly IRepository<ActivityMaster> _activityRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public ActivityMasterService(IRepository<ActivityMaster> activityRepository, IRepository<ModuleMaster> moduleRepository,
            IRepository<SubModuleMaster> subModuleRepository,
           IMasterCommonRepository masterCommonRepository,
                     IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _activityRepository = activityRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
        }
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.View)]
        [PMMSAuthorize(Permissions = "ActivityMaster.View")]
        public async Task<ActivityDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _activityRepository.GetAsync(input.Id);
            var activity = ObjectMapper.Map<ActivityDto>(entity);
            activity.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Activity_SubModule);
            activity.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return activity;
            // return ObjectMapper.Map<ActivityDto>(entity);
        }
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.View)]
        [PMMSAuthorize(Permissions = "ActivityMaster.View")]
        public async Task<PagedResultDto<ActivityListDto>> GetAllAsync(PagedActivityResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ActivityListDto>(
                totalCount,
                entities
            );
        }
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.Add)]
        [PMMSAuthorize(Permissions = "ActivityMaster.Add")]
        public async Task<ActivityDto> CreateAsync(CreateActivityDto input)
        {
            if (await _activityRepository.GetAll().AnyAsync(x => x.ActivityCode == input.ActivityCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.ActivityAlreadyExist);
            }
            var activity = ObjectMapper.Map<ActivityMaster>(input);
            var currentDate = DateTime.UtcNow;
            activity.TenantId = AbpSession.TenantId;
            activity.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Activity_SubModule);
            await _activityRepository.InsertAsync(activity);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<ActivityDto>(activity);
        }
        // [PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.Edit)]
        [PMMSAuthorize(Permissions = "ActivityMaster.Edit")]
        public async Task<ActivityDto> UpdateAsync(ActivityDto input)
        {

            var activity = await _activityRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, activity);

            await _activityRepository.UpdateAsync(activity);

            return await GetAsync(input);
        }
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.Delete)]
        [PMMSAuthorize(Permissions = "ActivityMaster.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var activity = await _activityRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _activityRepository.DeleteAsync(activity).ConfigureAwait(false);
        }

        protected IQueryable<ActivityListDto> ApplySorting(IQueryable<ActivityListDto> query, PagedActivityResultRequestDto input)
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
        protected IQueryable<ActivityListDto> ApplyPaging(IQueryable<ActivityListDto> query, PagedActivityResultRequestDto input)
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
        // [PMMSAuthorize(Permissions = PMMSPermissionConst.Activity_SubModule + "." + PMMSPermissionConst.Approver)]
        [PMMSAuthorize(Permissions = "ActivityMaster.Approver")]
        public async Task ApproveOrRejectActivityAsync(ApprovalStatusDto input)
        {
            //if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
            //{
            //    var associatedEntities = await GetAllAssociatedMasters(input.Id);
            //    if (associatedEntities.Count > 0)
            //    {
            //        throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantRejected);
            //    }
            //}
            var activity = await _activityRepository.GetAsync(input.Id);
            activity.ApprovalStatusId = input.ApprovalStatusId;
            activity.ApprovalStatusDescription = input.Description;
            await _activityRepository.UpdateAsync(activity);
        }

        protected ActivityListDto MapToEntityDto(ActivityMaster entity)
        {
            return ObjectMapper.Map<ActivityListDto>(entity);
        }
        protected IQueryable<ActivityListDto> CreateUserListFilteredQuery(PagedActivityResultRequestDto input)
        {
            var activityQuery = from activity in _activityRepository.GetAll()
                                join module in _moduleRepository.GetAll()
                                on activity.ModuleId equals module.Id into activityms
                                from module in activityms.DefaultIfEmpty()
                                join subModule in _subModuleRepository.GetAll()
                                on activity.SubModuleId equals subModule.Id into activitysms
                                from subModule in activitysms.DefaultIfEmpty()
                                join approvalStatus in _approvalStatusRepository.GetAll()
                                on activity.ApprovalStatusId equals approvalStatus.Id into paStatus
                                from approvalStatus in paStatus.DefaultIfEmpty()
                                select new ActivityListDto
                                {
                                    Id = activity.Id,
                                    ActivityName = activity.ActivityName,
                                    ActivityCode = activity.ActivityCode,
                                    ModuleId = module.Id,
                                    UserEnteredModuleId = module.Name,
                                    SubModuleId = subModule.Id,
                                    UserEnteredSubModuleId = subModule.DisplayName,
                                    IsActive = activity.IsActive,
                                    Description = activity.Description,
                                    ApprovalStatusId = activity.ApprovalStatusId,
                                    UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                };
            if (input.ModuleId != null)
            {
                activityQuery = activityQuery.Where(x => x.ModuleId == input.ModuleId);
            }
            if (input.SubModuleId != null)
            {
                activityQuery = activityQuery.Where(x => x.SubModuleId == input.SubModuleId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                activityQuery = activityQuery.Where(x => x.ActivityName.Contains(input.Keyword)
                || x.ActivityCode.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    activityQuery = activityQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    activityQuery = activityQuery.Where(x => x.IsActive);
                }
            }
            //if (input.ApprovalStatusId != null)
            //{
            //    activityQuery = activityQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            //}

            return activityQuery;
        }
    }
}
