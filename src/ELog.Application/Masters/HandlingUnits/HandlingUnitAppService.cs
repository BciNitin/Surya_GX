using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.HandlingUnits.Dto;
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

namespace ELog.Application.Masters.HandlingUnits
{
    [PMMSAuthorize]
    public class HandlingUnitAppService : ApplicationService, IHandlingUnitAppService
    {
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<HandlingUnitTypeMaster> _handlingUnitTypeMasterRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public HandlingUnitAppService(IRepository<HandlingUnitMaster> handlingunitRepository,
            IRepository<HandlingUnitTypeMaster> handlingUnitTypeMasterRepository,
            IRepository<PlantMaster> plantRepository, IHttpContextAccessor httpContextAccessor,
            IMasterCommonRepository masterCommonRepository, IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _handlingUnitRepository = handlingunitRepository;
            _handlingUnitTypeMasterRepository = handlingUnitTypeMasterRepository;
            _plantRepository = plantRepository;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<HandlingUnitDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _handlingUnitRepository.GetAsync(input.Id);
            var handlingUnit = ObjectMapper.Map<HandlingUnitDto>(entity);
            handlingUnit.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.HandlingUnit_SubModule);
            handlingUnit.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return handlingUnit;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<HandlingUnitListDto>> GetAllAsync(PagedHandlingUnitResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<HandlingUnitListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HandlingUnitDto> CreateAsync(CreateHandlingUnitDto input)
        {
            if (await _handlingUnitRepository.GetAll().AnyAsync(x => x.HUCode == input.HUCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.HUCodeAlreadyExist);
            }
            var handlingUnit = ObjectMapper.Map<HandlingUnitMaster>(input);
            handlingUnit.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.HandlingUnit_SubModule);
            handlingUnit.TenantId = AbpSession.TenantId;
            await _handlingUnitRepository.InsertAsync(handlingUnit);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<HandlingUnitDto>(handlingUnit);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HandlingUnitDto> UpdateAsync(HandlingUnitDto input)
        {
            if (await _handlingUnitRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.HUCode == input.HUCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.HUCodeAlreadyExist);
            }
            var handlingUnit = await _handlingUnitRepository.GetAsync(input.Id);
            handlingUnit.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.HandlingUnit_SubModule, handlingUnit.ApprovalStatusId);
            ObjectMapper.Map(input, handlingUnit);

            await _handlingUnitRepository.UpdateAsync(handlingUnit);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var handlingUnit = await _handlingUnitRepository.GetAsync(input.Id);
            await _handlingUnitRepository.DeleteAsync(handlingUnit);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.HandlingUnit_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectHandlingUnitAsync(ApprovalStatusDto input)
        {
            var handlingUnit = await _handlingUnitRepository.GetAsync(input.Id);

            handlingUnit.ApprovalStatusId = input.ApprovalStatusId;
            handlingUnit.ApprovalStatusDescription = input.Description;
            await _handlingUnitRepository.UpdateAsync(handlingUnit);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<HandlingUnitListDto> ApplySorting(IQueryable<HandlingUnitListDto> query, PagedHandlingUnitResultRequestDto input)
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
        protected IQueryable<HandlingUnitListDto> ApplyPaging(IQueryable<HandlingUnitListDto> query, PagedHandlingUnitResultRequestDto input)
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

        protected IQueryable<HandlingUnitListDto> CreateUserListFilteredQuery(PagedHandlingUnitResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = from handlingUnit in _handlingUnitRepository.GetAll()
                                    join plant in _plantRepository.GetAll()
                                    on handlingUnit.PlantId equals plant.Id into ps
                                    from plant in ps.DefaultIfEmpty()
                                    join handlingUnitType in _handlingUnitTypeMasterRepository.GetAll()
                                    on handlingUnit.HandlingUnitTypeId equals handlingUnitType.Id into es
                                    from handlingUnitType in es.DefaultIfEmpty()
                                    join approvalStatus in _approvalStatusRepository.GetAll()
                             on handlingUnit.ApprovalStatusId equals approvalStatus.Id into paStatus
                                    from approvalStatus in paStatus.DefaultIfEmpty()
                                    select new HandlingUnitListDto
                                    {
                                        Id = handlingUnit.Id,
                                        HUCode = handlingUnit.HUCode,
                                        PlantId = handlingUnit.PlantId,
                                        UserEnteredPlantId = plant.PlantId,
                                        IsActive = handlingUnit.IsActive,
                                        HandlingUnitTypeId = handlingUnit.HandlingUnitTypeId,
                                        Name = handlingUnit.Name,
                                        UserEnteredHandlingUnit = handlingUnitType.HandlingUnitName,
                                        ApprovalStatusId = handlingUnit.ApprovalStatusId,
                                        UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                    };
            if (input.PlantId != null)
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.HUCode.Contains(input.Keyword) || x.Name.Contains(input.Keyword));
            }
            if (input.HandlingUnitTypeId != null)
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.HandlingUnitTypeId == input.HandlingUnitTypeId);
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    handlingUnitQuery = handlingUnitQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    handlingUnitQuery = handlingUnitQuery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }

            return handlingUnitQuery;
        }
    }
}