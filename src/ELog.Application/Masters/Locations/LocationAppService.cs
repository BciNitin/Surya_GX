using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Locations.Dto;
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

namespace ELog.Application.Masters.Locations
{
    [PMMSAuthorize]
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationAppService(IRepository<LocationMaster> locationRepository,
            IRepository<PlantMaster> plantRepository,
            IRepository<CubicleMaster> cubicleRepository,
            IRepository<AreaMaster> areaRepository,
             IRepository<DepartmentMaster> departmentRepository,
            IRepository<EquipmentMaster> equipmentRepository,
            IMasterCommonRepository masterCommonRepository,
            IRepository<ApprovalStatusMaster> approvalStatusRepository,
            IHttpContextAccessor httpContextAccessor)

        {
            _locationRepository = locationRepository;
            _plantRepository = plantRepository;
            _cubicleRepository = cubicleRepository;
            _areaRepository = areaRepository;
            _departmentRepository = departmentRepository;
            _equipmentRepository = equipmentRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _httpContextAccessor = httpContextAccessor;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<LocationDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _locationRepository.GetAsync(input.Id);
            var location = ObjectMapper.Map<LocationDto>(entity);
            location.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Location_SubModule);
            location.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return location;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<LocationListDto>> GetAllAsync(PagedLocationResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LocationListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<LocationDto> CreateAsync(CreateLocationDto input)
        {
            if (await _locationRepository.GetAll().AnyAsync(x => x.LocationCode == input.LocationCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LocationCodeAlreadyExist);
            }
            var location = ObjectMapper.Map<LocationMaster>(input);
            location.TenantId = AbpSession.TenantId;
            location.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Location_SubModule);
            await _locationRepository.InsertAsync(location);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<LocationDto>(location);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<LocationDto> UpdateAsync(LocationDto input)
        {
            if (await _locationRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.LocationCode == input.LocationCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LocationCodeAlreadyExist);
            }
            if (!input.IsActive)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LocationCannotDeactivated);
                }
            }
            var location = await _locationRepository.GetAsync(input.Id);
            location.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Location_SubModule, location.ApprovalStatusId);
            ObjectMapper.Map(input, location);

            await _locationRepository.UpdateAsync(location);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var associatedEntities = await GetAllAssociatedMasters(input.Id);
            if (associatedEntities.Count > 0)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LocationDelete);
            }
            var location = await _locationRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _locationRepository.DeleteAsync(location).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Location_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectLocationAsync(ApprovalStatusDto input)
        {
            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LocationRejected);
                }
            }
            var location = await _locationRepository.GetAsync(input.Id);
            location.ApprovalStatusId = input.ApprovalStatusId;
            location.ApprovalStatusDescription = input.Description;
            await _locationRepository.UpdateAsync(location);
        }

        private async Task<List<string>> GetAllAssociatedMasters(int id)
        {
            List<string> lstAssociatedEntities = new List<string>();
            var entity = await (from location in _locationRepository.GetAll()
                                join cubicle in _cubicleRepository.GetAll()
                                on location.Id equals cubicle.SLOCId into cubicles
                                from cubicle in cubicles.DefaultIfEmpty()
                                join equipment in _equipmentRepository.GetAll()
                                on location.Id equals equipment.SLOCId into equipments
                                from equipment in equipments.DefaultIfEmpty()
                                where location.Id == id
                                select new
                                {
                                    cubicleSLOCId = cubicle.SLOCId,
                                    equipmentSLOCId = equipment.SLOCId
                                }).FirstOrDefaultAsync() ?? default;
            if (entity?.cubicleSLOCId > 0)
            {
                lstAssociatedEntities.Add("Cubicle");
            }
            if (entity?.equipmentSLOCId > 0)
            {
                lstAssociatedEntities.Add("Equipment");
            }
            return lstAssociatedEntities;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<LocationListDto> ApplySorting(IQueryable<LocationListDto> query, PagedLocationResultRequestDto input)
        {
            //Try to sort query if available
            ISortedResultRequest sortInput = input as ISortedResultRequest;
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
        protected IQueryable<LocationListDto> ApplyPaging(IQueryable<LocationListDto> query, PagedLocationResultRequestDto input)
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

        protected IQueryable<LocationListDto> CreateUserListFilteredQuery(PagedLocationResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var locationQuery = from location in _locationRepository.GetAll()
                                join plant in _plantRepository.GetAll()
                                on location.PlantId equals plant.Id into ps
                                from plant in ps.DefaultIfEmpty()
                                join dept in _departmentRepository.GetAll()
                                on location.DepartmentId equals dept.Id into ds
                                from dept in ds.DefaultIfEmpty()
                                join area in _areaRepository.GetAll()
                                on location.AreaId equals area.Id into areaps
                                from area in areaps.DefaultIfEmpty()

                                join approvalStatus in _approvalStatusRepository.GetAll()
                              on location.ApprovalStatusId equals approvalStatus.Id into paStatus
                                from approvalStatus in paStatus.DefaultIfEmpty()

                                select new LocationListDto
                                {
                                    Id = location.Id,
                                    LocationCode = location.LocationCode,
                                    StorageLocationType = location.StorageLocationType,
                                    UserEnteredDepartmentId = dept.DepartmentCode,
                                    UserEnteredAreaId = area.AreaCode,
                                    Area = location.AreaId,
                                    PlantId = location.PlantId,
                                    UserEnteredPlantId = plant.PlantId,
                                    IsActive = location.IsActive,
                                    ApprovalStatusId = location.ApprovalStatusId,
                                    UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                };
            if (input.PlantId != null)
            {
                locationQuery = locationQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                locationQuery = locationQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input.LocationCode) || string.IsNullOrWhiteSpace(input.LocationCode)))
            {
                locationQuery = locationQuery.Where(x => x.LocationCode.Contains(input.LocationCode));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    locationQuery = locationQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    locationQuery = locationQuery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                locationQuery = locationQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            return locationQuery;
        }
    }
}