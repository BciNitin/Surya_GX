using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.StandardWeights.Dto;
using ELog.Application.Masters.WeighingMachines.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Masters.StandardWeights
{
    [PMMSAuthorize]
    public class StandardWeightAppService : ApplicationService, IStandardWeightAppService
    {
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public StandardWeightAppService(IRepository<StandardWeightMaster> standardWeightRepository,
            IRepository<DepartmentMaster> departmentrepository, IRepository<AreaMaster> areaRepository,
            IRepository<PlantMaster> plantRepository, IHttpContextAccessor httpContextAccessor,
            IMasterCommonRepository masterCommonRepository,
          IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _standardWeightRepository = standardWeightRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _departmentRepository = departmentrepository;
            _plantRepository = plantRepository;
            _areaRepository = areaRepository;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<StandardWeightDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _standardWeightRepository.GetAsync(input.Id);
            var standardWeight = ObjectMapper.Map<StandardWeightDto>(entity);
            standardWeight.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.StandardWeight_SubModule);
            standardWeight.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return standardWeight;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<StandardWeightListDto>> GetAllAsync(PagedStandardWeightResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<StandardWeightListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<StandardWeightDto> CreateAsync(CreateStandardWeightDto input)
        {
            if (await _standardWeightRepository.GetAll().AnyAsync(x => x.StandardWeightId == input.StandardWeightId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StandardWeightIdAlreadyExist);
            }
            var standardWeight = ObjectMapper.Map<StandardWeightMaster>(input);
            standardWeight.TenantId = AbpSession.TenantId;
            standardWeight.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.StandardWeight_SubModule);
            await _standardWeightRepository.InsertAsync(standardWeight);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<StandardWeightDto>(standardWeight);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<StandardWeightDto> UpdateAsync(StandardWeightDto input)
        {
            if (await _standardWeightRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.StandardWeightId == input.StandardWeightId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StandardWeightIdAlreadyExist);
            }
            var standardWeight = await _standardWeightRepository.GetAsync(input.Id);
            standardWeight.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.StandardWeight_SubModule, standardWeight.ApprovalStatusId);
            ObjectMapper.Map(input, standardWeight);

            await _standardWeightRepository.UpdateAsync(standardWeight);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var standardWeight = await _standardWeightRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _standardWeightRepository.DeleteAsync(standardWeight).ConfigureAwait(false);
        }
        public async Task<List<StandardWeightStampingDueListDto>> GetStandardWeightStampingDueList()
        {
            var subPlantIdHeader = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var stampingDueWbList = (from standardWeight in _standardWeightRepository.GetAll()
                                     join plant in _plantRepository.GetAll()
                                     on standardWeight.SubPlantId equals plant.Id
                                     join area in _areaRepository.GetAll()
                                     on standardWeight.AreaId equals area.Id
                                     join department in _departmentRepository.GetAll()
                                     on standardWeight.DepartmentId equals department.Id

                                     select new StandardWeightStampingDueListDto
                                     {
                                         StandardWeightBoxId = standardWeight.StandardWeightId,
                                         StampingDoneOn = standardWeight.StampingDoneOn,
                                         StampingDueOn = standardWeight.StampingDueOn,
                                         SubPlant = plant.PlantId,
                                         DueDays = (standardWeight.StampingDueOn - DateTime.Now).Days,
                                         PlantId = standardWeight.SubPlantId,
                                         Area = area.AreaCode,
                                         Department = department.DepartmentCode
                                     });
            if (subPlantIdHeader != null && !string.IsNullOrEmpty(subPlantIdHeader))
            {
                stampingDueWbList = stampingDueWbList.Where(x => x.PlantId == Convert.ToInt32(subPlantIdHeader));
            }
            var result = await stampingDueWbList.ToListAsync();
            return result.Where(x => x.DueDays <= PMMSConsts.TotalStampingDays).ToList();
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<StandardWeightListDto> ApplySorting(IQueryable<StandardWeightListDto> query, PagedStandardWeightResultRequestDto input)
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
        protected IQueryable<StandardWeightListDto> ApplyPaging(IQueryable<StandardWeightListDto> query, PagedStandardWeightResultRequestDto input)
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

        protected StandardWeightListDto MapToEntityDto(StandardWeightMaster entity)
        {
            return ObjectMapper.Map<StandardWeightListDto>(entity);
        }

        protected IQueryable<StandardWeightListDto> CreateUserListFilteredQuery(PagedStandardWeightResultRequestDto input)
        {
            var subPlantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var standardWtquery = from wtBox in _standardWeightRepository.GetAll()
                                  join subPlant in _plantRepository.GetAll()
                                  on wtBox.SubPlantId equals subPlant.Id into ps
                                  from subPlant in ps.DefaultIfEmpty()
                                  join departMent in _departmentRepository.GetAll()
                                  on wtBox.DepartmentId equals departMent.Id into ds
                                  from departMent in ds.DefaultIfEmpty()
                                  join area in _areaRepository.GetAll()
                                  on wtBox.AreaId equals area.Id into areaps
                                  from area in areaps.DefaultIfEmpty()
                                  join approvalStatus in _approvalStatusRepository.GetAll()
                                            on wtBox.ApprovalStatusId equals approvalStatus.Id into paStatus
                                  from approvalStatus in paStatus.DefaultIfEmpty()
                                  select new StandardWeightListDto
                                  {
                                      Id = wtBox.Id,
                                      StandardWeightId = wtBox.StandardWeightId,
                                      AreaId = wtBox.AreaId,
                                      SubPlantId = wtBox.SubPlantId,
                                      UserEnteredSubPlantId = subPlant.PlantId,
                                      DepartmentId = wtBox.DepartmentId,
                                      CapacityinDecimal = wtBox.CapacityinDecimal,
                                      UserEnteredDepartmentId = departMent.DepartmentCode,
                                      UserEnteredAreaId = area.AreaCode,
                                      IsActive = wtBox.IsActive,
                                      ApprovalStatusId = wtBox.ApprovalStatusId,
                                      UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                  };
            if (input.SubPlantId != null)
            {
                standardWtquery = standardWtquery.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (!(string.IsNullOrEmpty(subPlantId) || string.IsNullOrWhiteSpace(subPlantId)))
            {
                standardWtquery = standardWtquery.Where(x => x.SubPlantId == Convert.ToInt32(subPlantId));
            }
            if (input.DepartmentId != null)
            {
                standardWtquery = standardWtquery.Where(x => x.DepartmentId == input.DepartmentId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                standardWtquery = standardWtquery.Where(x => x.StandardWeightId.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    standardWtquery = standardWtquery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    standardWtquery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                standardWtquery = standardWtquery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            return standardWtquery;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeight_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectStandardWeightAsync(ApprovalStatusDto input)
        {
            var standardWeight = await _standardWeightRepository.GetAsync(input.Id);

            standardWeight.ApprovalStatusId = input.ApprovalStatusId;
            standardWeight.ApprovalStatusDescription = input.Description;
            await _standardWeightRepository.UpdateAsync(standardWeight);
        }
    }
}