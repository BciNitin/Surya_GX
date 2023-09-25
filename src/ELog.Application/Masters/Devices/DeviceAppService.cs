using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Devices.Dto;
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

namespace ELog.Application.Masters.Devices
{
    [PMMSAuthorize]
    public class DeviceAppService : ApplicationService, IDeviceAppService
    {
        private readonly IRepository<DeviceMaster> _deviceRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<DeviceTypeMaster> _deviceTypeMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public DeviceAppService(IRepository<DeviceMaster> deviceRepository,
            IRepository<PlantMaster> plantRepository, IRepository<DeviceTypeMaster> deviceTypeMasterRepository,
            IHttpContextAccessor httpContextAccessor, IMasterCommonRepository masterCommonRepository,
            IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _deviceRepository = deviceRepository;
            _plantRepository = plantRepository;
            _httpContextAccessor = httpContextAccessor;
            _deviceTypeMasterRepository = deviceTypeMasterRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<DeviceDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _deviceRepository.GetAsync(input.Id);
            var device = ObjectMapper.Map<DeviceDto>(entity);
            device.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Device_SubModule);
            device.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return device;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<DeviceListDto>> GetAllAsync(PagedDeviceResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<DeviceListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<DeviceDto> CreateAsync(CreateDeviceDto input)
        {
            if (await _deviceRepository.GetAll().AnyAsync(x => x.DeviceId == input.DeviceId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.DeviceIdAlreadyExist);
            }
            var device = ObjectMapper.Map<DeviceMaster>(input);
            device.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Device_SubModule);
            device.TenantId = AbpSession.TenantId;
            await _deviceRepository.InsertAsync(device);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<DeviceDto>(device);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<DeviceDto> UpdateAsync(DeviceDto input)
        {
            if (await _deviceRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.DeviceId == input.DeviceId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.DeviceIdAlreadyExist);
            }
            var device = await _deviceRepository.GetAsync(input.Id);
            device.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Device_SubModule, device.ApprovalStatusId);
            ObjectMapper.Map(input, device);

            await _deviceRepository.UpdateAsync(device);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var device = await _deviceRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _deviceRepository.DeleteAsync(device).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Device_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectDeviceAsync(ApprovalStatusDto input)
        {
            var device = await _deviceRepository.GetAsync(input.Id);

            device.ApprovalStatusId = input.ApprovalStatusId;
            device.ApprovalStatusDescription = input.Description;
            await _deviceRepository.UpdateAsync(device);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<DeviceListDto> ApplySorting(IQueryable<DeviceListDto> query, PagedDeviceResultRequestDto input)
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
        protected IQueryable<DeviceListDto> ApplyPaging(IQueryable<DeviceListDto> query, PagedDeviceResultRequestDto input)
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

        protected IQueryable<DeviceListDto> CreateUserListFilteredQuery(PagedDeviceResultRequestDto input)
        {
            var subPlantIdHeader = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var deviceQuery = from device in _deviceRepository.GetAll()
                              join plant in _plantRepository.GetAll()
                              on device.SubPlantId equals plant.Id into ps
                              from plant in ps.DefaultIfEmpty()
                              join deviceType in _deviceTypeMasterRepository.GetAll()
                              on device.DeviceTypeId equals deviceType.Id into es
                              from deviceType in es.DefaultIfEmpty()
                              join approvalStatus in _approvalStatusRepository.GetAll()
                             on device.ApprovalStatusId equals approvalStatus.Id into paStatus
                              from approvalStatus in paStatus.DefaultIfEmpty()
                              select new DeviceListDto
                              {
                                  Id = device.Id,
                                  DeviceId = device.DeviceId,
                                  SubPlantId = device.SubPlantId,
                                  UserEnteredPlantId = plant.PlantId,
                                  IsActive = device.IsActive,
                                  DeviceTypeId = device.DeviceTypeId,
                                  UserEnteredDeviceType = deviceType.DeviceName,
                                  Make = device.Make,
                                  Model = device.Model,
                                  ApprovalStatusId = device.ApprovalStatusId,
                                  UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                              };
            if (input.SubPlantId != null)
            {
                deviceQuery = deviceQuery.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (!(string.IsNullOrEmpty(subPlantIdHeader) || string.IsNullOrWhiteSpace(subPlantIdHeader)))
            {
                deviceQuery = deviceQuery.Where(x => x.SubPlantId == Convert.ToInt32(subPlantIdHeader));
            }
            if (!(string.IsNullOrEmpty(input.DeviceId) || string.IsNullOrWhiteSpace(input.DeviceId)))
            {
                deviceQuery = deviceQuery.Where(x => x.DeviceId.Contains(input.DeviceId));
            }
            if (input.DeviceTypeId != null)
            {
                deviceQuery = deviceQuery.Where(x => x.DeviceTypeId == input.DeviceTypeId);
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    deviceQuery = deviceQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    deviceQuery = deviceQuery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                deviceQuery = deviceQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }

            return deviceQuery;
        }
    }
}