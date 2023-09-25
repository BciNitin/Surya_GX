using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Cubicles.Dto;
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

namespace ELog.Application.Masters.Cubicles
{
    [PMMSAuthorize]
    public class CubicleAppService : ApplicationService, ICubicleAppService
    {
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<DeviceMaster> _deviceRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public CubicleAppService(IRepository<CubicleMaster> cubicleRepository,
            IRepository<PlantMaster> plantRepository,
            IRepository<DeviceMaster> deviceRepository,
             IRepository<AreaMaster> areaRepository,
             IHttpContextAccessor httpContextAccessor,
            IMasterCommonRepository masterCommonRepository,
            IRepository<ApprovalStatusMaster> approvalStatusRepository)
        {
            _cubicleRepository = cubicleRepository;
            _plantRepository = plantRepository;
            _deviceRepository = deviceRepository;
            _areaRepository = areaRepository;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<CubicleDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _cubicleRepository.GetAsync(input.Id);
            var cubicle = ObjectMapper.Map<CubicleDto>(entity);
            cubicle.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Cubicle_SubModule);
            cubicle.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return cubicle;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<CubicleListDto>> GetAllAsync(PagedCubicleResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<CubicleListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<CubicleDto> CreateAsync(CreateCubicleDto input)
        {
            if (await _cubicleRepository.GetAll().AnyAsync(x => x.CubicleCode == input.CubicleCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleCodeAlreadyExist);
            }
            var cubicle = ObjectMapper.Map<CubicleMaster>(input);
            cubicle.TenantId = AbpSession.TenantId;
            cubicle.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Cubicle_SubModule);
            await _cubicleRepository.InsertAsync(cubicle);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CubicleDto>(cubicle);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CubicleDto> UpdateAsync(CubicleDto input)
        {
            if (await _cubicleRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.CubicleCode == input.CubicleCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleCodeAlreadyExist);
            }
            if (!input.IsActive)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleCannotDeactivated);
                }
            }
            var cubicle = await _cubicleRepository.GetAsync(input.Id);
            cubicle.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Cubicle_SubModule, cubicle.ApprovalStatusId);
            ObjectMapper.Map(input, cubicle);

            await _cubicleRepository.UpdateAsync(cubicle);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var associatedEntities = await GetAllAssociatedMasters(input.Id);
            if (associatedEntities.Count > 0)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleDelete);
            }
            var cubicle = await _cubicleRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _cubicleRepository.DeleteAsync(cubicle).ConfigureAwait(false);
        }

        private async Task<List<string>> GetAllAssociatedMasters(int id)
        {
            List<string> lstAssociatedEntities = new List<string>();
            var entity = await (from cubicle in _cubicleRepository.GetAll()

                                join device in _deviceRepository.GetAll()
                                on cubicle.Id equals device.CubicleId into deviceCubicles
                                from device in deviceCubicles.DefaultIfEmpty()

                                where cubicle.Id == id
                                select new
                                {
                                    deviceCubicleId = device.CubicleId,
                                }).FirstOrDefaultAsync() ?? default;
            if (entity?.deviceCubicleId > 0)
            {
                lstAssociatedEntities.Add("Device");
            }

            return lstAssociatedEntities;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Cubicle_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectCubicleAsync(ApprovalStatusDto input)
        {
            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CubicleRejected);
                }
            }
            var cubicle = await _cubicleRepository.GetAsync(input.Id);
            cubicle.ApprovalStatusId = input.ApprovalStatusId;
            cubicle.ApprovalStatusDescription = input.Description;
            await _cubicleRepository.UpdateAsync(cubicle);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<CubicleListDto> ApplySorting(IQueryable<CubicleListDto> query, PagedCubicleResultRequestDto input)
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
        protected IQueryable<CubicleListDto> ApplyPaging(IQueryable<CubicleListDto> query, PagedCubicleResultRequestDto input)
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

        protected IQueryable<CubicleListDto> CreateUserListFilteredQuery(PagedCubicleResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var cubicleQuery = from cubicle in _cubicleRepository.GetAll()
                               join plant in _plantRepository.GetAll()
                               on cubicle.PlantId equals plant.Id into ps
                               from plant in ps.DefaultIfEmpty()
                               join area in _areaRepository.GetAll()
                               on cubicle.AreaId equals area.Id into areaps
                               from area in areaps.DefaultIfEmpty()
                               join approvalStatus in _approvalStatusRepository.GetAll()
                            on cubicle.ApprovalStatusId equals approvalStatus.Id into paStatus
                               from approvalStatus in paStatus.DefaultIfEmpty()
                               select new CubicleListDto
                               {
                                   Id = cubicle.Id,
                                   CubicleCode = cubicle.CubicleCode,
                                   Area = cubicle.AreaId,
                                   PlantId = cubicle.PlantId,
                                   UserEnteredPlantId = plant.PlantId,
                                   UserEnteredAreaId = area.AreaCode,
                                   IsActive = cubicle.IsActive,
                                   SLOCId = cubicle.SLOCId,
                                   ApprovalStatusId = cubicle.ApprovalStatusId,
                                   UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                               };
            if (input.PlantId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input.CubicleCode) || string.IsNullOrWhiteSpace(input.CubicleCode)))
            {
                cubicleQuery = cubicleQuery.Where(x => x.CubicleCode.Contains(input.CubicleCode));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    cubicleQuery = cubicleQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    cubicleQuery = cubicleQuery.Where(x => x.IsActive);
                }
            }
            if (input.SLOCId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.SLOCId == input.SLOCId);
            }
            if (input.ApprovalStatusId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            return cubicleQuery;
        }
    }
}