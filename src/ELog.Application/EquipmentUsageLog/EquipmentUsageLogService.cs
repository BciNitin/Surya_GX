using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonService.Dispensing.Dto;
using ELog.Application.EquipmentUsageLog.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Modules;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
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


namespace ELog.Application.EquipmentUsageLog
{

    public class EquipmentUsageLogService : ApplicationService, IEquipmentUsageLogService
    {
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.EquipmentUsageLogStatus.Started).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.EquipmentUsageLogStatus.Verified).ToLower();
        private readonly string approvedStatus = nameof(PMMSEnums.EquipmentUsageLogStatus.Approved).ToLower();
        private readonly string rejectedStatus = nameof(PMMSEnums.EquipmentUsageLogStatus.Rejected).ToLower();
        private readonly string cancelledStatus = nameof(PMMSEnums.EquipmentUsageLogStatus.Cancelled).ToLower();
        private readonly IRepository<ELog.Core.Entities.EquipmentUsageLog> _equipmentUsageLogrepo;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<EquipmentUsageLogList> _equipmentUsageLogListRepository;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<ActivityMaster> _activityMasterRepository;

        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<ELog.Core.Entities.EquipmentUsageLog> _equipmentUsageLog;
        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
        private readonly IRepository<ChecklistTypeMaster> _checklistTypeMasterRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        public EquipmentUsageLogService(IRepository<ELog.Core.Entities.EquipmentUsageLog> equipmentUsageLogrepo,
           IMasterCommonRepository masterCommonRepository, IRepository<CheckpointMaster> checkpointRepository
            , IRepository<EquipmentUsageLogList> equipmentUsageLogListRepository
            , IRepository<ModeMaster> modeRepository
            , IRepository<ActivityMaster> activityMasterRepository
            , IRepository<User, long> userRepository
            , IRepository<EquipmentMaster> equipmentRepository
            , IRepository<CubicleMaster> cubicleRepository
            , IRepository<ELog.Core.Entities.EquipmentUsageLog> equipmentUsageLog,
            IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
            IRepository<ChecklistTypeMaster> checklistTypeMasterRepository,
            IRepository<DepartmentMaster> departmentRepository,
            IRepository<AreaMaster> areaRepository,
            IHttpContextAccessor httpContextAccessor,

                 IModuleAppService moduleAppService,
                 IRepository<StatusMaster> statusMasterRepository
            )

        {
            _equipmentUsageLogrepo = equipmentUsageLogrepo;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _checkpointRepository = checkpointRepository;
            _equipmentUsageLogListRepository = equipmentUsageLogListRepository;
            _modeRepository = modeRepository;
            _activityMasterRepository = activityMasterRepository;
            _userRepository = userRepository;
            _equipmentRepository = equipmentRepository;
            _cubicleRepository = cubicleRepository;
            _equipmentUsageLog = equipmentUsageLog;
            _inspectionChecklistRepository = inspectionChecklistRepository;
            _checklistTypeMasterRepository = checklistTypeMasterRepository;
            _departmentRepository = departmentRepository;
            _areaRepository = areaRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _moduleAppService = moduleAppService;
            _statusMasterRepository = statusMasterRepository;
        }

        public async Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId)
        {
            var checkPointQuery = _checkpointRepository.GetAll().Where(a => a.InspectionChecklistId == 1).OrderBy(x => x.CheckpointName)
                                                         .Select(checkpoint => new CheckpointDto
                                                         {
                                                             CheckPointId = checkpoint.Id,
                                                             InspectionChecklistId = checklistId,
                                                             CheckpointName = checkpoint.CheckpointName,
                                                             ValueTag = checkpoint.ValueTag,
                                                             AcceptanceValue = checkpoint.AcceptanceValue,
                                                             CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                             ModeId = checkpoint.ModeId,
                                                             Observation = null,
                                                             DiscrepancyRemark = null,
                                                         });
            if (modeId != 0)
            {
                checkPointQuery = checkPointQuery.Where(x => x.ModeId == modeId);
            }
            return await checkPointQuery.ToListAsync() ?? default;
        }





        [PMMSAuthorize(Permissions = "EquipmentCleaning.Add")]
        public async Task<EquipmentUsageLogDto> CreateAsync(CreateEquipmentUsageLogDto input)
        {
            var activity = ObjectMapper.Map<ELog.Core.Entities.EquipmentUsageLog>(input);
            var currentDate = DateTime.UtcNow;
            activity.StartTime = currentDate;
            activity.EndTime = null;
            activity.Id = await _equipmentUsageLog.GetAll().Where(x => x.EquipmentBracodeId == activity.EquipmentBracodeId && x.EndTime == null).Select(x => x.Id).FirstOrDefaultAsync();

            if (activity.Id != 0)
            {
                activity.Id = 0;
                return ObjectMapper.Map<EquipmentUsageLogDto>(activity);
            }
            else
            {
                await _equipmentUsageLogrepo.InsertAsync(activity);
            }

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<EquipmentUsageLogDto>(activity);
        }

        //[PMMSAuthorize(Permissions = "EquipmentCleaning.View")]
        //public async Task<EquipmentUsageLogDto> GetAsync(EntityDto<int> input)
        //{
        //    var entity = await _equipmentUsageLogrepo.GetAsync(input.Id);
        //    return ObjectMapper.Map<EquipmentUsageLogDto>(entity);
        //}

        [PMMSAuthorize(Permissions = "EquipmentCleaning.View")]
        public async Task<EquipmentUsageLogDto> GetAsync(EntityDto<int> input)
        {
            //var entity = await _equipmentUsageLogrepo.GetAsync(input.Id);
            //return ObjectMapper.Map<EquipmentUsageLogDto>(entity);
            var entity = await _equipmentUsageLogrepo.GetAsync(input.Id);
            var vehicleInspection = ObjectMapper.Map<EquipmentUsageLogDto>(entity);
            var isControllerMode = await _modeRepository.FirstOrDefaultAsync(a => a.IsController);
            vehicleInspection.CubicalCode = await _cubicleRepository.GetAll().Where(x => x.Id == entity.ProcessBarcodeId).Select(x => x.CubicleCode).FirstOrDefaultAsync();
            vehicleInspection.EquipmentCode = await _equipmentRepository.GetAll().Where(x => x.Id == entity.EquipmentBracodeId).Select(x => x.EquipmentCode).FirstOrDefaultAsync();
            var vehicleInspectionDetails = await (from detail in _equipmentUsageLogListRepository.GetAll()
                                                  join checkpoint in _checkpointRepository.GetAll()
                                                  on detail.CheckpointId equals checkpoint.Id
                                                  join inspectionCheckList in _inspectionChecklistRepository.GetAll()
                                                  on checkpoint.InspectionChecklistId equals inspectionCheckList.Id
                                                  join checkListType in _checklistTypeMasterRepository.GetAll()
                                                  on inspectionCheckList.ChecklistTypeId equals checkListType.Id
                                                  where detail.EquipmentUsageHeaderId == entity.Id
                                                  select new CheckpointDto
                                                  {
                                                      Id = detail.Id,
                                                      CheckPointId = checkpoint.Id,
                                                      //InspectionChecklistId = vehicleInspection.InspectionChecklistId,
                                                      CheckpointName = checkpoint.CheckpointName,
                                                      ValueTag = checkpoint.ValueTag,
                                                      AcceptanceValue = checkpoint.AcceptanceValue,
                                                      CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                      ModeId = checkpoint.ModeId,
                                                      Observation = detail.Observation,
                                                      DiscrepancyRemark = detail.DiscrepancyRemark,
                                                      IsControllerMode = isControllerMode != null && isControllerMode.Id == checkpoint.ModeId,
                                                      CheckListTypeId = checkListType.Id
                                                  }).ToListAsync() ?? default;

            var startedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.EquipmentCleaningSubModule, startedStatus);
            var approvedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.EquipmentCleaningSubModule, approvedStatus);
            var startedAndApprovedStatusList = new List<int> { startedStatusMasterId, approvedStatusMasterId };
            vehicleInspection.CreatorName = _userRepository.FirstOrDefault(x => x.Id == vehicleInspection.CreatorUserId).FullName;
            if (vehicleInspection.StatusId == startedStatusMasterId)
            {
                vehicleInspection.CanApproved = vehicleInspection.CreatorUserId != (int)AbpSession.UserId;
            }
            else if (vehicleInspection.StatusId == approvedStatusMasterId)
            {
                vehicleInspection.ApprovedByName = _userRepository.FirstOrDefault(x => x.Id == vehicleInspection.ApprovedBy).FullName;
                //AreaUsageInspection.CanVerified = AreaUsageInspection.ApprovedBy != (int)AbpSession.UserId && lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
            }

            vehicleInspection.EquipmentUsageLogLists = vehicleInspectionDetails;
            return vehicleInspection;

        }

        [PMMSAuthorize(Permissions = "EquipmentCleaning.Edit")]
        public async Task<EquipmentUsageLogDto> UpdateAsync(UpdateEquipmentUsageLogDto input)
        {

            var allowedStatusList = new List<string> { nameof(EquipmentUsageLogStatus.Started).ToLower() };
            var approvedStatusList = new List<string> { nameof(EquipmentUsageLogStatus.Approved).ToLower() };
            var rejectedStatusList = new List<string> { nameof(EquipmentUsageLogStatus.Rejected).ToLower() };

            var equipmentUsageList = await GetStatusListByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.EquipmentCleaningSubModule);
            var startedStatusId = equipmentUsageList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();
            var approvedStatusId = equipmentUsageList.Where(x => approvedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();
            var rejectedStatusId = equipmentUsageList.Where(x => rejectedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();

            var equipmentUsageLog = await _equipmentUsageLogrepo.GetAsync(input.Id);
            var details = await _equipmentUsageLogListRepository.GetAllListAsync(x => x.EquipmentUsageHeaderId == input.Id);
            equipmentUsageLog.Remarks = input.Remarks;
            var currentDate = DateTime.UtcNow;
            if (input.IsApproved)
            {
                equipmentUsageLog.StatusId = Convert.ToInt32(approvedStatusId);
                equipmentUsageLog.Status = true;
                equipmentUsageLog.ApprovedBy = (int)AbpSession.UserId;
                equipmentUsageLog.ApprovedTime = currentDate;
            }
            else if (input.IsRejected)
            {
                equipmentUsageLog.StatusId = Convert.ToInt32(rejectedStatusId);
                equipmentUsageLog.IsDeleted = true;
                equipmentUsageLog.Remarks = input.Remarks;
                equipmentUsageLog.DeleterUserId = (int)AbpSession.UserId;
                equipmentUsageLog.DeletionTime = currentDate;
            }
            else
            {
                equipmentUsageLog.StatusId = Convert.ToInt32(startedStatusId);
                equipmentUsageLog.EndTime = currentDate;
            }

            equipmentUsageLog.EquipmentUsageLogLists = details;
            if (input.EquipmentUsageLogLists != null)
            {
                foreach (var checkpoint in input.EquipmentUsageLogLists.Where(x => x.Id == 0))
                {
                    var checkpointToInsert = ObjectMapper.Map<EquipmentUsageLogList>(checkpoint);
                    checkpointToInsert.EquipmentUsageHeaderId = input.Id;
                    checkpointToInsert.CheckpointId = checkpoint.CheckPointId;
                    equipmentUsageLog.EquipmentUsageLogLists.Add(checkpointToInsert);
                }
                await _equipmentUsageLogrepo.UpdateAsync(equipmentUsageLog);
            }
            return await GetAsync(input);


        }


        [PMMSAuthorize(Permissions = "EquipmentCleaning.View")]
        public async Task<PagedResultDto<EquipmentUsageLogListDto>> GetAllAsync(PagedEquipmentUsageLogResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<EquipmentUsageLogListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<EquipmentUsageLogListDto> CreateUserListFilteredQuery(PagedEquipmentUsageLogResultRequestDto input)
        {

            var EquipmentUsageLogQuery = from eqUsage in _equipmentUsageLogrepo.GetAll()
                                         join activity in _activityMasterRepository.GetAll()
                                         on eqUsage.ActivityId equals activity.Id into a
                                         from activity in a.DefaultIfEmpty()
                                             //join usr in _userRepository.GetAll()
                                             //on eqUsage.OperatorId equals usr.Id into o
                                             //from usr in o.DefaultIfEmpty()
                                         join equp in _equipmentRepository.GetAll()
                                         on eqUsage.EquipmentBracodeId equals equp.Id into e
                                         from equp in e.DefaultIfEmpty()
                                         join cub in _cubicleRepository.GetAll()
                                         on eqUsage.ProcessBarcodeId equals cub.Id into c
                                         from cub in c.DefaultIfEmpty()

                                         select new EquipmentUsageLogListDto
                                         {
                                             Id = eqUsage.Id,
                                             ActivityId = activity.Id,
                                             ActivityName = activity.ActivityName,
                                             OperatorName = eqUsage.OperatorName,
                                             EquipmentType = eqUsage.EquipmentType,
                                             equipmentBracodeId = equp.Id,
                                             equipmentBracodeName = equp.EquipmentCode,
                                             processBarcodeId = cub.Id,
                                             processBarcodeName = cub.CubicleCode,
                                             StartTime = eqUsage.StartTime,
                                             EndtTime = eqUsage.EndTime,
                                         };
            if (input.ActivityId != null)
            {
                EquipmentUsageLogQuery = EquipmentUsageLogQuery.Where(x => x.ActivityId == input.ActivityId);
            }
            if (input.equipmentBracodeId != null)
            {
                EquipmentUsageLogQuery = EquipmentUsageLogQuery.Where(x => x.equipmentBracodeId == input.equipmentBracodeId);
            }
            if (input.processBarcodeId != null)
            {
                EquipmentUsageLogQuery = EquipmentUsageLogQuery.Where(x => x.processBarcodeId == input.processBarcodeId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                EquipmentUsageLogQuery = EquipmentUsageLogQuery.Where(x => x.OperatorName.Contains(input.Keyword) || x.EquipmentType.Contains(input.Keyword));
            }
            return EquipmentUsageLogQuery;
        }

        protected IQueryable<EquipmentUsageLogListDto> ApplySorting(IQueryable<EquipmentUsageLogListDto> query, PagedEquipmentUsageLogResultRequestDto input)
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
        protected IQueryable<EquipmentUsageLogListDto> ApplyPaging(IQueryable<EquipmentUsageLogListDto> query, PagedEquipmentUsageLogResultRequestDto input)
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
        public async Task<List<CheckpointDto>> GetCheckpointsListAsync(int checklistId)
        {
            return await (from inspectionCheckList in _inspectionChecklistRepository.GetAll().Where(x => x.ChecklistTypeId == checklistId)
                          join checkPoint in _checkpointRepository.GetAll()
                          on inspectionCheckList.Id equals checkPoint.InspectionChecklistId

                          select new CheckpointDto
                          {
                              CheckListTypeId = checklistId,
                              CheckPointId = checkPoint.Id,
                              InspectionChecklistId = checkPoint.InspectionChecklistId,
                              CheckpointName = checkPoint.CheckpointName,
                              ValueTag = checkPoint.ValueTag,
                              AcceptanceValue = checkPoint.AcceptanceValue,
                              CheckpointTypeId = checkPoint.CheckpointTypeId,
                              ModeId = checkPoint.ModeId,
                              Observation = null,
                              DiscrepancyRemark = null,

                          }).ToListAsync() ?? default;
        }

        public async Task<List<EquipmentCleaningBarcodeDto>> GetAllEquipmentBarcodeAsync(string input, bool isFxdPort)
        {
            var equipments = new List<EquipmentCleaningBarcodeDto>();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

                var equipmentQuery = from equipment in _equipmentRepository.GetAll()
                                     join department in _departmentRepository.GetAll()
                                     on equipment.SLOCId equals department.Id
                                     join area in _areaRepository.GetAll()
                                     on equipment.SLOCId equals area.DepartmentId
                                     where (equipment.IsPortable == null ? false : equipment.IsPortable) == isFxdPort && equipment.ApprovalStatusId == approvedApprovalStatusId && equipment.IsActive
                                     && equipment.EquipmentCode.ToLower() == input.ToLower()
                                     select new EquipmentCleaningBarcodeDto
                                     {
                                         EquipmentId = equipment.Id,
                                         EquipmentBarcode = string.IsNullOrEmpty(equipment.Name) ? equipment.EquipmentCode : $"{equipment.EquipmentCode} - {equipment.Name}",
                                         PlantId = equipment.PlantId,
                                         EquipmentTypeId = equipment.IsPortable == true ? (int)PMMSEnums.EquipementType.Portable : (int)PMMSEnums.EquipementType.Fixed,

                                     };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    equipmentQuery = equipmentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }

                equipments = await equipmentQuery.ToListAsync();
                foreach (var item in equipments)
                {
                    var cubicle = await (from equipment in _equipmentUsageLog.GetAll()
                                         join cubicleMaster in _cubicleRepository.GetAll()
                                         on equipment.ProcessBarcodeId equals cubicleMaster.Id
                                         where equipment.EquipmentBracodeId == item.EquipmentId
                                         select cubicleMaster).FirstOrDefaultAsync() ?? default;
                    if (cubicle != null)
                    {
                        item.CubicleBarcode = cubicle.CubicleCode;
                        item.CubicleId = cubicle.Id;
                    }
                }
                return equipments;
            }
            return equipments;
        }


        public async Task<int> GetStatusByModuleSubModuleName(string module, string submodule, string status)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == status).Select(x => x.Id).FirstOrDefaultAsync();
        }
        public async Task<List<StatusMaster>> GeSubmoduleAllStatusList(string module, string submodule)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId).ToListAsync();
        }

        public async Task<List<StatusMaster>> GetStatusListByModuleSubModuleName(string module, string submodule)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId).Select(x => new StatusMaster { Id = x.Id, Status = x.Status })?
                        .ToListAsync() ?? default;
        }
    }
}
