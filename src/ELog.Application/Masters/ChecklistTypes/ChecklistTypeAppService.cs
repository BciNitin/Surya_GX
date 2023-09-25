//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.Masters.ChecklistTypes.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Entities;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Masters.ChecklistTypes
//{
//    [PMMSAuthorize]
//    public class ChecklistTypeAppService : ApplicationService, IChecklistTypeAppService
//    {
//        private readonly IRepository<ChecklistTypeMaster> _checklistTypeRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<SubModuleMaster> _subModuleRepository;
//        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;

//        public ChecklistTypeAppService(IRepository<ChecklistTypeMaster> checklistTypeRepository,
//           IRepository<PlantMaster> plantRepository,
//           IRepository<SubModuleMaster> subModuleRepository,
//           IMasterCommonRepository masterCommonRepository,
//          IRepository<ApprovalStatusMaster> approvalStatusRepository, IHttpContextAccessor httpContextAccessor,
//          IRepository<InspectionChecklistMaster> inspectionChecklistRepository)

//        {
//            _checklistTypeRepository = checklistTypeRepository;
//            _plantRepository = plantRepository;
//            _subModuleRepository = subModuleRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _approvalStatusRepository = approvalStatusRepository;
//            _inspectionChecklistRepository = inspectionChecklistRepository;
//            _httpContextAccessor = httpContextAccessor;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<ChecklistTypeDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _checklistTypeRepository.GetAsync(input.Id);
//            var checklistType = ObjectMapper.Map<ChecklistTypeDto>(entity);
//            checklistType.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.ChecklistType_SubModule);
//            checklistType.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
//            return checklistType;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<ChecklistTypeListDto>> GetAllAsync(PagedChecklistTypeResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<ChecklistTypeListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<ChecklistTypeDto> CreateAsync(CreateChecklistTypeDto input)
//        {
//            var checklistType = ObjectMapper.Map<ChecklistTypeMaster>(input);
//            checklistType.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.ChecklistType_SubModule);
//            checklistType.TenantId = AbpSession.TenantId;
//            var currentDate = DateTime.UtcNow;
//            checklistType.ChecklistTypeCode = $"CT{currentDate.Month:D2}{currentDate:yy}{_masterCommonRepository.GetNextUOMSequence():D4}";
//            await _checklistTypeRepository.InsertAsync(checklistType);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<ChecklistTypeDto>(checklistType);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<ChecklistTypeDto> UpdateAsync(ChecklistTypeDto input)
//        {
//            var checklistType = await _checklistTypeRepository.GetAsync(input.Id);
//            if (!input.IsActive)
//            {
//                if (await IsChecklistTypeAssociatedWithAnyMaster(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CheckListTypeInactive);
//                }
//            }
//            checklistType.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.ChecklistType_SubModule, checklistType.ApprovalStatusId);
//            ObjectMapper.Map(input, checklistType);

//            await _checklistTypeRepository.UpdateAsync(checklistType);

//            return await GetAsync(input);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.Delete)]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            if (await IsChecklistTypeAssociatedWithAnyMaster(input.Id))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CheckListTypeDelete);
//            }
//            var checklistType = await _checklistTypeRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _checklistTypeRepository.DeleteAsync(checklistType).ConfigureAwait(false);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.ChecklistType_SubModule + "." + PMMSPermissionConst.Approver)]
//        public async Task ApproveOrRejectChecklistTypeAsync(ApprovalStatusDto input)
//        {
//            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
//            {
//                if (await IsChecklistTypeAssociatedWithAnyMaster(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CheckListTypeRejected);
//                }
//            }
//            var equipment = await _checklistTypeRepository.GetAsync(input.Id);

//            equipment.ApprovalStatusId = input.ApprovalStatusId;
//            equipment.ApprovalStatusDescription = input.Description;
//            await _checklistTypeRepository.UpdateAsync(equipment);
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<ChecklistTypeListDto> ApplySorting(IQueryable<ChecklistTypeListDto> query, PagedChecklistTypeResultRequestDto input)
//        {
//            //Try to sort query if available
//            if (input is ISortedResultRequest sortInput && !sortInput.Sorting.IsNullOrWhiteSpace())
//            {
//                return query.OrderBy(sortInput.Sorting);
//            }

//            //IQueryable.Task requires sorting, so we should sort if Take will be used.
//            if (input is ILimitedResultRequest)
//            {
//                return query.OrderByDescending(e => e.Id);
//            }

//            //No sorting
//            return query;
//        }

//        /// <summary>
//        /// Should apply paging if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<ChecklistTypeListDto> ApplyPaging(IQueryable<ChecklistTypeListDto> query, PagedChecklistTypeResultRequestDto input)
//        {
//            //Try to use paging if available
//            if (input is IPagedResultRequest pagedInput)
//            {
//                return query.PageBy(pagedInput);
//            }

//            //Try to limit query result if available
//            if (input is ILimitedResultRequest limitedInput)
//            {
//                return query.Take(limitedInput.MaxResultCount);
//            }

//            //No paging
//            return query;
//        }

//        protected IQueryable<ChecklistTypeListDto> CreateUserListFilteredQuery(PagedChecklistTypeResultRequestDto input)
//        {
//            var subPlantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var checklistTypeQuery = from checklistType in _checklistTypeRepository.GetAll()
//                                     join plant in _plantRepository.GetAll()
//                                      on checklistType.SubPlantId equals plant.Id into ps
//                                     from plant in ps.DefaultIfEmpty()
//                                     join subModule in _subModuleRepository.GetAll()
//                                     on checklistType.SubModuleId equals subModule.Id
//                                     join approvalStatus in _approvalStatusRepository.GetAll()
//                                     on checklistType.ApprovalStatusId equals approvalStatus.Id into paStatus
//                                     from approvalStatus in paStatus.DefaultIfEmpty()
//                                     select new ChecklistTypeListDto
//                                     {
//                                         Id = checklistType.Id,
//                                         IsActive = checklistType.IsActive,
//                                         SubPlantId = checklistType.SubPlantId,
//                                         SubModuleId = checklistType.SubModuleId,
//                                         UserEnteredPlantId = plant.PlantId,
//                                         UserEnteredSubModuleName = subModule.DisplayName,
//                                         ChecklistName = checklistType.ChecklistName,
//                                         ChecklistTypeCode = checklistType.ChecklistTypeCode,
//                                         ApprovalStatusId = checklistType.ApprovalStatusId,
//                                         UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
//                                     };

//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                checklistTypeQuery = checklistTypeQuery.Where(x =>
//                x.ChecklistTypeCode.Contains(input.Keyword) ||
//                x.ChecklistName.Contains(input.Keyword) ||
//                x.UserEnteredSubModuleName.Contains(input.Keyword) ||
//                x.UserEnteredPlantId.Contains(input.Keyword));
//            }
//            if (input.SubPlantId != null)
//            {
//                checklistTypeQuery = checklistTypeQuery.Where(x => x.SubPlantId == input.SubPlantId);
//            }
//            if (!(string.IsNullOrEmpty(subPlantId) || string.IsNullOrWhiteSpace(subPlantId)))
//            {
//                checklistTypeQuery = checklistTypeQuery.Where(x => x.SubPlantId == Convert.ToInt32(subPlantId));
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    checklistTypeQuery = checklistTypeQuery.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    checklistTypeQuery = checklistTypeQuery.Where(x => x.IsActive);
//                }
//            }
//            if (input.ApprovalStatusId != null)
//            {
//                checklistTypeQuery = checklistTypeQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
//            }
//            return checklistTypeQuery;
//        }
//        private async Task<bool> IsChecklistTypeAssociatedWithAnyMaster(int id)
//        {
//            return await (from CheckPointType in _checklistTypeRepository.GetAll()
//                          join inspectionCheckList in _inspectionChecklistRepository.GetAll()
//                          on CheckPointType.Id equals inspectionCheckList.ChecklistTypeId into InsepectionCheckLists
//                          from inspectionCheckList in InsepectionCheckLists.DefaultIfEmpty()
//                          where inspectionCheckList.ChecklistTypeId == id
//                          select new
//                          {
//                              checklistTypeId = inspectionCheckList.ChecklistTypeId
//                          })
//                          .AnyAsync(x => x.checklistTypeId > 0);
//        }
//    }
//}