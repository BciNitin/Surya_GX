using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Equipments.Dto;
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

namespace ELog.Application.Masters.Equipments
{
    [PMMSAuthorize]
    public class EquipmentAppService : ApplicationService, IEquipmentAppService
    {
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<EquipmentTypeMaster> _equipmentTypeMasterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public EquipmentAppService(IRepository<EquipmentMaster> equipmentRepository,
            IRepository<PlantMaster> plantRepository, IRepository<EquipmentTypeMaster> equipmentTypeMasterRepository,
            IHttpContextAccessor httpContextAccessor, IMasterCommonRepository masterCommonRepository,
            IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _equipmentRepository = equipmentRepository;
            _plantRepository = plantRepository;
            _equipmentTypeMasterRepository = equipmentTypeMasterRepository;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<EquipmentDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _equipmentRepository.GetAsync(input.Id);
            var equipment = ObjectMapper.Map<EquipmentDto>(entity);
            equipment.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Equipment_SubModule);
            equipment.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return equipment;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<EquipmentListDto>> GetAllAsync(PagedEquipmentResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<EquipmentListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto input)
        {
            if (await _equipmentRepository.GetAll().AnyAsync(x => x.EquipmentCode == input.EquipmentCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.EquipmentCodeAlreadyExist);
            }
            var equipment = ObjectMapper.Map<EquipmentMaster>(input);
            equipment.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Equipment_SubModule);
            equipment.TenantId = AbpSession.TenantId;
            await _equipmentRepository.InsertAsync(equipment);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<EquipmentDto>(equipment);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<EquipmentDto> UpdateAsync(EquipmentDto input)
        {
            if (await _equipmentRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.EquipmentCode == input.EquipmentCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.EquipmentCodeAlreadyExist);
            }
            var equipment = await _equipmentRepository.GetAsync(input.Id);
            equipment.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Equipment_SubModule, equipment.ApprovalStatusId);
            ObjectMapper.Map(input, equipment);

            await _equipmentRepository.UpdateAsync(equipment);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var equipment = await _equipmentRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _equipmentRepository.DeleteAsync(equipment).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Equipment_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectEquipmentAsync(ApprovalStatusDto input)
        {
            var equipment = await _equipmentRepository.GetAsync(input.Id);

            equipment.ApprovalStatusId = input.ApprovalStatusId;
            equipment.ApprovalStatusDescription = input.Description;
            await _equipmentRepository.UpdateAsync(equipment);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<EquipmentListDto> ApplySorting(IQueryable<EquipmentListDto> query, PagedEquipmentResultRequestDto input)
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
        protected IQueryable<EquipmentListDto> ApplyPaging(IQueryable<EquipmentListDto> query, PagedEquipmentResultRequestDto input)
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

        protected IQueryable<EquipmentListDto> CreateUserListFilteredQuery(PagedEquipmentResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var equipmentQuery = from equipment in _equipmentRepository.GetAll()
                                 join plant in _plantRepository.GetAll()
                                 on equipment.PlantId equals plant.Id into ps
                                 from plant in ps.DefaultIfEmpty()
                                 join equipmentType in _equipmentTypeMasterRepository.GetAll()
                                 on equipment.EquipmentTypeId equals equipmentType.Id into es
                                 from equipmentType in es.DefaultIfEmpty()
                                 join approvalStatus in _approvalStatusRepository.GetAll()
                             on equipment.ApprovalStatusId equals approvalStatus.Id into paStatus
                                 from approvalStatus in paStatus.DefaultIfEmpty()
                                 select new EquipmentListDto
                                 {
                                     Id = equipment.Id,
                                     EquipmentCode = equipment.EquipmentCode,
                                     PlantId = equipment.PlantId,
                                     UserEnteredPlantId = plant.PlantId,
                                     IsActive = equipment.IsActive,
                                     SLOCId = equipment.SLOCId,
                                     EquipmentTypeId = equipment.EquipmentTypeId,
                                     UserEnteredEquipment = equipmentType.EquipmentName,
                                     ApprovalStatusId = equipment.ApprovalStatusId,
                                     UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                 };
            if (input.PlantId != null)
            {
                equipmentQuery = equipmentQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                equipmentQuery = equipmentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input.EquipmentCode) || string.IsNullOrWhiteSpace(input.EquipmentCode)))
            {
                equipmentQuery = equipmentQuery.Where(x => x.EquipmentCode.Contains(input.EquipmentCode));
            }
            if (input.EquipmentTypeId != null)
            {
                equipmentQuery = equipmentQuery.Where(x => x.EquipmentTypeId == input.EquipmentTypeId);
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    equipmentQuery = equipmentQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    equipmentQuery = equipmentQuery.Where(x => x.IsActive);
                }
            }
            if (input.SLOCId != null)
            {
                equipmentQuery = equipmentQuery.Where(x => x.SLOCId == input.SLOCId);
            }
            if (input.ApprovalStatusId != null)
            {
                equipmentQuery = equipmentQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            return equipmentQuery;
        }
    }
}