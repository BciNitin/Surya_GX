//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.Masters.InspectionChecklists.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Entities;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Masters.InspectionChecklists
//{
//    [PMMSAuthorize]
//    public class InspectionChecklistAppService : ApplicationService, IInspectionChecklistAppService
//    {
//        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<SubModuleMaster> _submoduleRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
//        private readonly IRepository<CheckpointMaster> _checkpointRepository;
//        private readonly IRepository<CheckpointTypeMaster> _checkpointTypeRepository;
//        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionHeaderRepository;
//        private readonly IRepository<MaterialInspectionRelationDetail> _materialCheckListDetailRepository;

//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

//        public InspectionChecklistAppService(IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
//            IRepository<PlantMaster> plantRepository, IRepository<SubModuleMaster> subModuleRepository, IMasterCommonRepository masterCommonRepository,
//          IRepository<CheckpointMaster> checkpointRepository, IRepository<CheckpointTypeMaster> checkpointTypeRepository,
//            IHttpContextAccessor httpContextAccessor, IRepository<ApprovalStatusMaster> approvalStatusRepository,
//            IRepository<VehicleInspectionHeader> vehicleInspectionHeaderRepository,
//            IRepository<MaterialInspectionRelationDetail> materialCheckListDetailRepository)

//        {
//            _inspectionChecklistRepository = inspectionChecklistRepository;
//            _plantRepository = plantRepository;
//            _submoduleRepository = subModuleRepository;
//            _checkpointRepository = checkpointRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _masterCommonRepository = masterCommonRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _approvalStatusRepository = approvalStatusRepository;
//            _checkpointTypeRepository = checkpointTypeRepository;
//            _vehicleInspectionHeaderRepository = vehicleInspectionHeaderRepository;
//            _materialCheckListDetailRepository = materialCheckListDetailRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<InspectionChecklistDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _inspectionChecklistRepository.GetAsync(input.Id);
//            var inspectionChecklist = ObjectMapper.Map<InspectionChecklistDto>(entity);

//            var inspectionCheckpoints = await (from checkpoint in _checkpointRepository.GetAll()
//                                               join checkpointType in _checkpointTypeRepository.GetAll()
//                                               on checkpoint.CheckpointTypeId equals checkpointType.Id
//                                               where checkpoint.InspectionChecklistId == inspectionChecklist.Id
//                                               select new CheckpointDto
//                                               {
//                                                   Id = checkpoint.Id,
//                                                   CheckpointTypeId = checkpoint.CheckpointTypeId,
//                                                   CheckpointTypeName = checkpointType.Title,
//                                                   CheckpointName = checkpoint.CheckpointName,
//                                                   ModeId = checkpoint.ModeId,
//                                                   ValueTag = checkpoint.ValueTag,
//                                                   AcceptanceValue = checkpoint.AcceptanceValue,
//                                                   InspectionChecklistId = checkpoint.InspectionChecklistId
//                                               }).ToListAsync() ?? default;

//            inspectionChecklist.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.InspectionChecklist_SubModule);
//            inspectionChecklist.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
//            inspectionChecklist.Checkpoints = new List<CheckpointDto>();
//            if (inspectionCheckpoints?.Count > 0)
//            {
//                inspectionChecklist.Checkpoints = inspectionCheckpoints;
//            }
//            return inspectionChecklist;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<InspectionChecklistListDto>> GetAllAsync(PagedInspectionChecklistResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<InspectionChecklistListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<InspectionChecklistDto> CreateAsync(CreateInspectionChecklistDto input)
//        {
//            if (await _inspectionChecklistRepository.GetAll().AnyAsync(x => x.ChecklistCode == input.ChecklistCode))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.InspectionCheckListAlreadyExist);
//            }

//            var inspectionChecklist = ObjectMapper.Map<InspectionChecklistMaster>(input);
//            inspectionChecklist.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.InspectionChecklist_SubModule);
//            inspectionChecklist.TenantId = AbpSession.TenantId;
//            var currentDate = DateTime.UtcNow;
//            inspectionChecklist.Version++;
//            inspectionChecklist.ChecklistCode = $"CL{currentDate.Month:D2}{currentDate:yy}{_masterCommonRepository.GetNextUOMSequence():D4}";

//            var insertedInspectionChecklist = await _inspectionChecklistRepository.InsertAsync(inspectionChecklist);

//            CurrentUnitOfWork.SaveChanges();
//            foreach (var checkpoint in input.Checkpoints)
//            {
//                var checkpointToInsert = ObjectMapper.Map<CheckpointMaster>(checkpoint);
//                checkpointToInsert.InspectionChecklistId = insertedInspectionChecklist.Id;
//                checkpointToInsert.TenantId = AbpSession.TenantId;
//                await _checkpointRepository.InsertAsync(checkpointToInsert);
//            }
//            return ObjectMapper.Map<InspectionChecklistDto>(inspectionChecklist);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<InspectionChecklistDto> UpdateAsync(InspectionChecklistDto input)
//        {
//            if (await _inspectionChecklistRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.ChecklistCode == input.ChecklistCode))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.InspectionCheckListAlreadyExist);
//            }

//            var inspectionChecklist = await _inspectionChecklistRepository.GetAsync(input.Id);
//            inspectionChecklist.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.InspectionChecklist_SubModule, inspectionChecklist.ApprovalStatusId);
//            ObjectMapper.Map(input, inspectionChecklist);

//            inspectionChecklist.Version++;

//            await _inspectionChecklistRepository.UpdateAsync(inspectionChecklist);
//            if (input.Checkpoints != null)
//            {
//                //Insert Checkpoint
//                foreach (var checkpoint in input.Checkpoints.Where(x => x.Id == 0))
//                {
//                    var checkpointToInsert = ObjectMapper.Map<CheckpointMaster>(checkpoint);
//                    checkpointToInsert.InspectionChecklistId = input.Id;
//                    checkpointToInsert.TenantId = AbpSession.TenantId;
//                    await _checkpointRepository.InsertAsync(checkpointToInsert);
//                }

//                var inspectionChecklistCheckpoints = await _checkpointRepository.GetAll().Where(x => x.InspectionChecklistId == input.Id).ToListAsync();
//                var inputCheckpointsToUpdateIds = input.Checkpoints.Where(x => x.Id > 0).Select(x => x.Id);
//                //Update Checkpoints
//                foreach (var checkpointId in inputCheckpointsToUpdateIds)
//                {
//                    var checkpointFromUpdate = input.Checkpoints.First(x => x.Id == checkpointId);
//                    var checkpointToUpdate = inspectionChecklistCheckpoints.FirstOrDefault(x => x.Id == checkpointId);
//                    if (checkpointToUpdate != null)
//                    {
//                        ObjectMapper.Map(checkpointFromUpdate, checkpointToUpdate);
//                        await _checkpointRepository.UpdateAsync(checkpointToUpdate);
//                    }
//                }

//                //Delete Checkpoints
//                await DeleteCheckpointsAsync(inspectionChecklistCheckpoints.Where(x => !inputCheckpointsToUpdateIds.Contains(x.Id)).ToList());
//            }
//            return await GetAsync(input);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.Delete)]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            if (await IsCheckPointAssociatedWithAnyTransaction(input.Id))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.InspectionCheckListDelete);
//            }
//            var inspectionChecklist = await _inspectionChecklistRepository.GetAllIncluding(a => a.CheckpointMasters).Where(a => a.Id == input.Id).FirstOrDefaultAsync();
//            var inspectionChecklistCheckpoints = await _checkpointRepository.GetAll().Where(x => x.InspectionChecklistId == input.Id).ToListAsync();
//            await DeleteCheckpointsAsync(inspectionChecklistCheckpoints);
//            await _inspectionChecklistRepository.DeleteAsync(inspectionChecklist).ConfigureAwait(false);
//        }
//        public async Task DeleteCheckpointsAsync(ICollection<CheckpointMaster> checkpoints)
//        {
//            foreach (var checkpointToDelete in checkpoints)
//            {
//                await _checkpointRepository.DeleteAsync(checkpointToDelete);
//            }
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.InspectionChecklist_SubModule + "." + PMMSPermissionConst.Approver)]
//        public async Task ApproveOrRejectInspectionCheklistAsync(ApprovalStatusDto input)
//        {
//            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
//            {
//                if (await IsCheckPointAssociatedWithAnyTransaction(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.InspectionCheckListRejected);
//                }
//            }
//            var equipment = await _inspectionChecklistRepository.GetAsync(input.Id);

//            equipment.ApprovalStatusId = input.ApprovalStatusId;
//            equipment.ApprovalStatusDescription = input.Description;
//            await _inspectionChecklistRepository.UpdateAsync(equipment);
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<InspectionChecklistListDto> ApplySorting(IQueryable<InspectionChecklistListDto> query, PagedInspectionChecklistResultRequestDto input)
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
//        protected IQueryable<InspectionChecklistListDto> ApplyPaging(IQueryable<InspectionChecklistListDto> query, PagedInspectionChecklistResultRequestDto input)
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

//        protected InspectionChecklistListDto MapToEntityDto(InspectionChecklistMaster entity)
//        {
//            return ObjectMapper.Map<InspectionChecklistListDto>(entity);
//        }

//        protected IQueryable<InspectionChecklistListDto> CreateUserListFilteredQuery(PagedInspectionChecklistResultRequestDto input)
//        {
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var query = from checklist in _inspectionChecklistRepository.GetAll()
//                        join plant in _plantRepository.GetAll()
//                        on checklist.PlantId equals plant.Id into ps
//                        from plant in ps.DefaultIfEmpty()
//                        join subModule in _submoduleRepository.GetAll()
//                        on checklist.SubModuleId equals subModule.Id into ms
//                        from subModule in ms.DefaultIfEmpty()
//                        join approvalStatus in _approvalStatusRepository.GetAll()
//                        on checklist.ApprovalStatusId equals approvalStatus.Id into paStatus
//                        from approvalStatus in paStatus.DefaultIfEmpty()
//                        select new InspectionChecklistListDto
//                        {
//                            Id = checklist.Id,
//                            ChecklistCode = checklist.ChecklistCode,
//                            PlantId = plant.Id,
//                            SubModuleId = checklist.SubModuleId,
//                            UserEnteredPlantId = plant.PlantId,
//                            UserEnteredSubModuleId = subModule.DisplayName,
//                            IsActive = checklist.IsActive,
//                            ApprovalStatusId = checklist.ApprovalStatusId,
//                            UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
//                        };
//            if (input.PlantId != null)
//            {
//                query = query.Where(x => x.PlantId == input.PlantId);
//            }
//            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            {
//                query = query.Where(x => x.PlantId == Convert.ToInt32(plantId));
//            }
//            if (input.ModuleId != null)
//            {
//                query = query.Where(x => x.SubModuleId == input.ModuleId);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                query = query.Where(x => x.ChecklistCode.Contains(input.Keyword));
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    query = query.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    query.Where(x => x.IsActive);
//                }
//            }
//            if (input.ApprovalStatusId != null)
//            {
//                query = query.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
//            }
//            return query;
//        }

//        private async Task<bool> IsCheckPointAssociatedWithAnyTransaction(int id)
//        {
//            return await (from inspectionCheckList in _inspectionChecklistRepository.GetAll()
//                          join vehicleInspectionHeader in _vehicleInspectionHeaderRepository.GetAll()
//                          on inspectionCheckList.Id equals vehicleInspectionHeader.InspectionChecklistId into checkPoints
//                          from checkpoint in checkPoints.DefaultIfEmpty()

//                          join miRelationHeader in _materialCheckListDetailRepository.GetAll()
//                            on inspectionCheckList.Id equals miRelationHeader.InspectionChecklistId into miCheckPoints
//                          from miCheckPoint in miCheckPoints.DefaultIfEmpty()

//                          where inspectionCheckList.Id == id
//                          select new
//                          {
//                              viInspectionCheckId = checkpoint.InspectionChecklistId,
//                              miInspectionCheckId = miCheckPoint.InspectionChecklistId
//                          })
//                           .AnyAsync(x => x.viInspectionCheckId > 0 || x.miInspectionCheckId > 0);
//        }
//    }
//}