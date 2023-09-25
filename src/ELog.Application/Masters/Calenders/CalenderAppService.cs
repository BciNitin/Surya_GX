using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Calenders.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Masters.Calenders
{
    [PMMSAuthorize]
    public class CalenderAppService : ApplicationService, ICalenderAppService
    {
        private readonly IRepository<CalenderMaster> _calenderRepository;
        private readonly IRepository<HolidayTypeMaster> _holidayTypeMaster;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAbpSession _abpSession;
        private readonly IMasterCommonRepository _masterCommonRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public CalenderAppService(IRepository<CalenderMaster> calenderRepository,
           IAbpSession abpSession, IRepository<HolidayTypeMaster> holidayTypeMaster,
            IRepository<PlantMaster> plantRepository, IRepository<ApprovalStatusMaster> approvalStatusRepository,
           IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)

        {
            _calenderRepository = calenderRepository;
            _abpSession = abpSession;
            _holidayTypeMaster = holidayTypeMaster;
            _httpContextAccessor = httpContextAccessor;
            _plantRepository = plantRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _masterCommonRepository = masterCommonRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<CalenderDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _calenderRepository.GetAsync(input.Id);
            var calender = ObjectMapper.Map<CalenderDto>(entity);
            calender.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Calendar_SubModule);
            calender.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return calender;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<CalenderListDto>> GetAllAsync(PagedCalenderResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<CalenderListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<CalenderDto> CreateAsync(CreateCalenderDto input)
        {
            var calender = ObjectMapper.Map<CalenderMaster>(input);
            calender.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Calendar_SubModule);
            calender.TenantId = AbpSession.TenantId;
            await _calenderRepository.InsertAsync(calender);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CalenderDto>(calender);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CalenderDto> UpdateAsync(CalenderDto input)
        {
            var calender = await _calenderRepository.GetAsync(input.Id);
            calender.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Calendar_SubModule, calender.ApprovalStatusId);
            ObjectMapper.Map(input, calender);

            await _calenderRepository.UpdateAsync(calender);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var calender = await _calenderRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _calenderRepository.DeleteAsync(calender).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Calendar_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectCalenderAsync(ApprovalStatusDto input)
        {
            var calender = await _calenderRepository.GetAsync(input.Id);

            calender.ApprovalStatusId = input.ApprovalStatusId;
            calender.ApprovalStatusDescription = input.Description;
            await _calenderRepository.UpdateAsync(calender);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<CalenderListDto> ApplySorting(IQueryable<CalenderListDto> query, PagedCalenderResultRequestDto input)
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
        protected IQueryable<CalenderListDto> ApplyPaging(IQueryable<CalenderListDto> query, PagedCalenderResultRequestDto input)
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

        protected CalenderListDto MapToEntityDto(CalenderMaster entity)
        {
            return ObjectMapper.Map<CalenderListDto>(entity);
        }

        protected IQueryable<CalenderListDto> CreateUserListFilteredQuery(PagedCalenderResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var calenderQuery = from calender in _calenderRepository.GetAll()
                                join holiday in _holidayTypeMaster.GetAll()
                                on calender.HolidayTypeId equals holiday.Id
                                join subPlant in _plantRepository.GetAll()
                                on calender.SubPlantId equals subPlant.Id
                                join approvalStatus in _approvalStatusRepository.GetAll()
                                on calender.ApprovalStatusId equals approvalStatus.Id into paStatus
                                from approvalStatus in paStatus.DefaultIfEmpty()
                                select new CalenderListDto
                                {
                                    Id = calender.Id,
                                    UserEnteredSubPlantId = subPlant.PlantId,
                                    IsActive = calender.IsActive,
                                    ApprovalStatusId = calender.ApprovalStatusId,
                                    UserEnteredApprovalStatus = approvalStatus.ApprovalStatus,
                                    HolidayTypeId = calender.HolidayTypeId,
                                    CalenderDate = calender.CalenderDate,
                                    Description = calender.Description,
                                    SubPlantId = calender.SubPlantId,
                                    UserEnteredHolidayType = holiday.HolidayType,
                                    HolidayName = calender.HolidayName
                                };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                calenderQuery = calenderQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }

            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                calenderQuery = calenderQuery.Where(x => x.HolidayName.Contains(input.Keyword) ||
                                                    x.UserEnteredHolidayType.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    calenderQuery = calenderQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    calenderQuery = calenderQuery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                calenderQuery = calenderQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            if (input.HolidayTypeId != null)
            {
                calenderQuery = calenderQuery.Where(x => x.HolidayTypeId == input.HolidayTypeId);
            }
            return calenderQuery;
        }
    }
}