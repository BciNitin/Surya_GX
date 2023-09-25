using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.AreaCleaning.Dto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Modules;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.AreaCleaning
{
    [PMMSAuthorize]
    public class AreaUsageLogService : ApplicationService, IAreaUsageLogService
    {

        //private readonly IRepository<ELog.Core.Entities.AreaUsageListLog> _arealistLogrepo;
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.AreaUsageLogStatus.Started).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.AreaUsageLogStatus.Verified).ToLower();
        private readonly string approvedStatus = nameof(PMMSEnums.AreaUsageLogStatus.Approved).ToLower();
        private readonly string rejectedStatus = nameof(PMMSEnums.AreaUsageLogStatus.Rejected).ToLower();
        private readonly string cancelledStatus = nameof(PMMSEnums.AreaUsageLogStatus.Cancelled).ToLower();
        private readonly IRepository<ELog.Core.Entities.AreaUsageLog> _areaUsageLogrepo;
        private readonly IRepository<ELog.Core.Entities.ActivityMaster> _activityrepo;
        private readonly IRepository<ELog.Core.Entities.CubicleMaster> _cubicalrepo;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<AreaUsageListLog> _areaUsageLogListRepository;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
        private readonly IRepository<ChecklistTypeMaster> _checklistTypeMasterRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public AreaUsageLogService(IRepository<ELog.Core.Entities.AreaUsageLog> areaUsageLogrepo,
            IRepository<ELog.Core.Entities.ActivityMaster> activityRepository,
            IRepository<ELog.Core.Entities.CubicleMaster> cubicalRepository,
           IMasterCommonRepository masterCommonRepository, IRepository<CheckpointMaster> checkpointRepository,
            IRepository<AreaUsageListLog> areaUsageLogListRepository, IRepository<ModeMaster> modeRepository,
             IRepository<ModuleMaster> moduleRepository, IRepository<SubModuleMaster> subModuleRepository,
             IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
             IRepository<ChecklistTypeMaster> checklistTypeMasterRepository,
                IRepository<User, long> userRepository,
                 IModuleAppService moduleAppService,
                 IRepository<StatusMaster> statusMasterRepository

            )
        {

            _areaUsageLogrepo = areaUsageLogrepo;
            _moduleAppService = moduleAppService;
            _statusMasterRepository = statusMasterRepository;
            // _arealistLogrepo = areaListLogRepository;
            _activityrepo = activityRepository;
            _cubicalrepo = cubicalRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _checkpointRepository = checkpointRepository;
            _areaUsageLogListRepository = areaUsageLogListRepository;
            _modeRepository = modeRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            _inspectionChecklistRepository = inspectionChecklistRepository;
            _checklistTypeMasterRepository = checklistTypeMasterRepository;
            _userRepository = userRepository;
        }
        [PMMSAuthorize(Permissions = "AreaUsageLog.View")]
        public async Task<AreaUsageLogDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _areaUsageLogrepo.GetAsync(input.Id);
            var AreaUsageInspection = ObjectMapper.Map<AreaUsageLogDto>(entity);
            AreaUsageInspection.CubicalCode = await _cubicalrepo.GetAll().Where(x => x.Id == entity.CubicalId).Select(x => x.CubicleCode).FirstOrDefaultAsync();
            var isControllerMode = await _modeRepository.FirstOrDefaultAsync(a => a.IsController);

            var areausageInspectionDetails = await (from detail in _areaUsageLogListRepository.GetAll()
                                                    join checkpoint in _checkpointRepository.GetAll()
                                                    on detail.CheckpointId equals checkpoint.Id
                                                    join inspectionCheckList in _inspectionChecklistRepository.GetAll()
                                                   on checkpoint.InspectionChecklistId equals inspectionCheckList.Id
                                                    join checkListType in _checklistTypeMasterRepository.GetAll()
                                                    on inspectionCheckList.ChecklistTypeId equals checkListType.Id
                                                    where detail.AreaUsageHeaderId == entity.Id
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

            //if (AreaUsageInspection.StopTime == null)
            //{
            //    AreaUsageInspection.AreaUsageLogLists = areausageInspectionDetails;
            //    if (!areausageInspectionDetails.Any(a => a.IsControllerMode))
            //    {
            //        var qaCheckpoints = await GetCheckpointsByChecklistIdAsync(1,3);
            //        AreaUsageInspection.AreaUsageLogLists.AddRange(qaCheckpoints);
            //    }
            //    return AreaUsageInspection;
            //}
            var startedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule, startedStatus);
            var approvedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule, approvedStatus);
            var startedAndApprovedStatusList = new List<int> { startedStatusMasterId, approvedStatusMasterId };
            AreaUsageInspection.CreatorName = _userRepository.FirstOrDefault(x => x.Id == AreaUsageInspection.CreatorUserId).FullName;
            if (AreaUsageInspection.StatusId == startedStatusMasterId)
            {
                AreaUsageInspection.CanApproved = AreaUsageInspection.CreatorUserId != (int)AbpSession.UserId;
            }
            else if (AreaUsageInspection.StatusId == approvedStatusMasterId)
            {
                AreaUsageInspection.ApprovedByName = _userRepository.FirstOrDefault(x => x.Id == AreaUsageInspection.ApprovedBy).FullName;
                //AreaUsageInspection.CanVerified = AreaUsageInspection.ApprovedBy != (int)AbpSession.UserId && lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
            }

            AreaUsageInspection.AreaUsageLogLists = areausageInspectionDetails;
            return AreaUsageInspection;

            // return ObjectMapper.Map<AreaUsageLogDto>(entity);
        }

        [PMMSAuthorize(Permissions = "AreaUsageLog.View")]
        public async Task<PagedResultDto<AreaUsageLogListDto>> GetAllAsync(PagedAreaUsageLogResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<AreaUsageLogListDto>(
                totalCount,
                entities
            );
        }

        public async Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId)
        {
            var checkPointQuery = _checkpointRepository.GetAll().Where(a => a.InspectionChecklistId == checklistId).OrderBy(x => x.CheckpointName)
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

        [PMMSAuthorize(Permissions = "AreaUsageLog.Add")]
        public async Task<AreaUsageLogDto> CreateAsync(CreateAreaUsageLogDto input)
        {
            var activity = ObjectMapper.Map<ELog.Core.Entities.AreaUsageLog>(input);

            var currentDate = DateTime.UtcNow;

            activity.StartTime = currentDate;
            activity.StopTime = null;
            activity.CubicalId = await _cubicalrepo.GetAll().Where(x => x.CubicleCode == input.CubicalCode).Select(x => x.Id).FirstOrDefaultAsync();
            activity.Id = await _areaUsageLogrepo.GetAll().Where(x => x.CubicalId == activity.CubicalId && x.StopTime == null).Select(x => x.Id).FirstOrDefaultAsync();

            //var allowedStatusList = new List<string> { verifiedStatus, nameof(AreaUsageLogStatus.Rejected).ToLower(), nameof(AreaUsageLogStatus.Cancelled).ToLower() };
            //var areaUsageStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule);
            //var startedStatusId = areaUsageStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
            if (activity.Id != 0)
            {
                activity.Id = 0;
                return ObjectMapper.Map<AreaUsageLogDto>(activity);
            }
            else
            {
                await _areaUsageLogrepo.InsertAsync(activity);
            }
            //await _areaUsageLogrepo.InsertAsync(activity);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AreaUsageLogDto>(activity);
        }

        [PMMSAuthorize(Permissions = "AreaUsageLog.Edit")]

        public async Task<AreaUsageLogDto> UpdateAsync(UpdateAreaUsageLogDto input)
        {
            var allowedStatusList = new List<string> { nameof(AreaUsageLogStatus.Started).ToLower() };
            var approvedStatusList = new List<string> { nameof(AreaUsageLogStatus.Approved).ToLower() };
            var rejectedStatusList = new List<string> { nameof(AreaUsageLogStatus.Rejected).ToLower() };

            var areaUsageStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule);
            var startedStatusId = areaUsageStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();
            var approvedStatusId = areaUsageStatusList.Where(x => approvedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();
            var rejectedStatusId = areaUsageStatusList.Where(x => rejectedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id).FirstOrDefault();


            var areaUsageLog = await _areaUsageLogrepo.GetAsync(input.Id);
            var details = await _areaUsageLogListRepository.GetAllListAsync(x => x.AreaUsageHeaderId == input.Id);

            var currentDate = DateTime.UtcNow;

            if (input.IsApproved)
            {
                areaUsageLog.StatusId = Convert.ToInt32(approvedStatusId);
                areaUsageLog.Status = true;
                areaUsageLog.ApprovedBy = (int)AbpSession.UserId;
                areaUsageLog.ApprovedTime = currentDate;
            }
            else if (input.IsRejected)
            {
                areaUsageLog.StatusId = Convert.ToInt32(rejectedStatusId);
                areaUsageLog.IsDeleted = true;
                // areaUsageLog.VerifiedBy = (int)AbpSession.UserId;
                areaUsageLog.Remarks = input.Remarks;
                areaUsageLog.DeleterUserId = (int)AbpSession.UserId;
                areaUsageLog.DeletionTime = currentDate;
            }
            else
            {
                areaUsageLog.StatusId = Convert.ToInt32(startedStatusId);
                areaUsageLog.StopTime = currentDate;
            }
            // areaUsageLog.Status = true;
            areaUsageLog.AreaUsageLogLists = details;
            if (input.AreaUsageLogLists != null)
            {
                foreach (var checkpoint in input.AreaUsageLogLists.Where(x => x.Id == 0))
                {
                    var checkpointToInsert = ObjectMapper.Map<AreaUsageListLog>(checkpoint);
                    checkpointToInsert.AreaUsageHeaderId = input.Id;
                    checkpointToInsert.CheckpointId = checkpoint.CheckPointId;
                    areaUsageLog.AreaUsageLogLists.Add(checkpointToInsert);
                }
                await _areaUsageLogrepo.UpdateAsync(areaUsageLog);
            }



            return await GetAsync(input);
        }


        [PMMSAuthorize(Permissions = "AreaUsageLog.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var activity = await _areaUsageLogrepo.GetAsync(input.Id).ConfigureAwait(false);
            await _areaUsageLogrepo.DeleteAsync(activity).ConfigureAwait(false);
        }

        protected IQueryable<AreaUsageLogListDto> ApplySorting(IQueryable<AreaUsageLogListDto> query, PagedAreaUsageLogResultRequestDto input)
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
        protected IQueryable<AreaUsageLogListDto> ApplyPaging(IQueryable<AreaUsageLogListDto> query, PagedAreaUsageLogResultRequestDto input)
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
        protected AreaUsageLogListDto MapToEntityDto(AreaUsageLog entity)
        {
            return ObjectMapper.Map<AreaUsageLogListDto>(entity);
        }



        protected IQueryable<AreaUsageLogListDto> CreateUserListFilteredQuery(PagedAreaUsageLogResultRequestDto input)
        {
            var areausagelogQuery = from areaUsageLog in _areaUsageLogrepo.GetAll()
                                    join activity in _activityrepo.GetAll()
                                    on areaUsageLog.ActivityID equals activity.Id into areaUsageLogms
                                    from activity in areaUsageLogms.DefaultIfEmpty()
                                    join cubical in _cubicalrepo.GetAll()
                                    on areaUsageLog.CubicalId equals cubical.Id into areaUsageLogsms
                                    from cubical in areaUsageLogsms.DefaultIfEmpty()
                                    select new AreaUsageLogListDto
                                    {
                                        Id = areaUsageLog.Id,
                                        OperatorName = areaUsageLog.OperatorName,
                                        StartTime = areaUsageLog.StartTime,
                                        StopTime = areaUsageLog.StopTime,
                                        ActivityID = activity.Id,
                                        UserEnteredActivityId = activity.ActivityName,
                                        CubicalId = cubical.Id,
                                        UserEnteredCubicalId = cubical.CubicleCode,
                                        IsActive = activity.IsActive,
                                    };

            if (input.ActivityID != null)
            {
                areausagelogQuery = areausagelogQuery.Where(x => x.ActivityID == input.ActivityID);
            }
            if (input.CubicalId != null)
            {
                areausagelogQuery = areausagelogQuery.Where(x => x.CubicalId == input.CubicalId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                // areausagelogQuery = areausagelogQuery.Where(x => x.OperatorName.Contains(input.Keyword));
                areausagelogQuery = areausagelogQuery.Where(x => x.OperatorName.Contains(input.Keyword) || x.UserEnteredActivityId.Contains(input.Keyword) || x.UserEnteredCubicalId.Contains(input.Keyword));

            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == 0)
                {
                    areausagelogQuery = areausagelogQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == 1)
                {
                    areausagelogQuery = areausagelogQuery.Where(x => x.IsActive);
                }
            }
            return areausagelogQuery;
        }
        public async Task<List<SelectListDto>> GetAllActivity()
        {
            //return await _activityrepo.GetAll().Where(x => x.IsActive).OrderBy(x => x.ActivityCode)
            //              join module in _moduleRepository.GetAll()
            //                    on areaUsageLog.ActivityID equals activity.Id into areaUsageLogms
            //                    from activity in areaUsageLogms.DefaultIfEmpty()
            //                    join cubical in _cubicalrepo.GetAll()
            //          .Select(x => new SelectListDto { Id = x.Id, Value = x.ActivityName })?
            //          .ToListAsync() ?? default;
            var result = await (from activity in _activityrepo.GetAll().Where(x => x.IsActive).OrderBy(x => x.ActivityCode)
                                join module in _moduleRepository.GetAll()
                                on activity.ModuleId equals module.Id
                                join submodule in _subModuleRepository.GetAll()
                                on activity.SubModuleId equals submodule.Id
                                where module.Name == "WIP" && module.IsActive && submodule.IsActive
                                && submodule.Name == "AreaUsageLog"
                                select new SelectListDto { Id = activity.Id, Value = activity.ActivityName }).ToListAsync() ?? default;


            return result;
        }
        public async Task<List<CheckpointDto>> GetCheckpointsListAsync(int checklistId)
        {
            return await (from inspectionCheckList in _inspectionChecklistRepository.GetAll().Where(x => x.ChecklistTypeId == checklistId)
                          join checkPoint in _checkpointRepository.GetAll()
                          on inspectionCheckList.Id equals checkPoint.InspectionChecklistId

                          select new CheckpointDto
                          {
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

        public async Task<BarcodeValidationDto> IsCheckProcessOrderAlreadyUsedAsync(int activityID)
        {
            var allowedStatusList = new List<string> { verifiedStatus, nameof(AreaUsageLogStatus.Rejected).ToLower(), nameof(AreaUsageLogStatus.Cancelled).ToLower() };
            var areaUsageStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule);
            var areaUsageInProgressStatusList = areaUsageStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
            var checkInProgressareaUsageTransaction = await _areaUsageLogrepo.GetAllListAsync(x => x.ActivityID == activityID && !areaUsageInProgressStatusList.Contains((int)x.StatusId));
            var barcodeValidationDto = new BarcodeValidationDto();
            if (checkInProgressareaUsageTransaction.Any())
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }

            var rejectStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule, rejectedStatus);

            var usedProcessOrder = await _areaUsageLogrepo.GetAllListAsync(x => x.ActivityID == activityID && x.StatusId != rejectStatusId);

            if (usedProcessOrder.Any())
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.AreaUsageLogAlreadyDone;
                return barcodeValidationDto;
            }
            else
            {

                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
        }
        public async Task<AreaUsageLogDto> ValidateAreaUsageLogAsync(int areaUsageLogId)
        {
            var startedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule, startedStatus);
            var approvedStatusMasterId = await GetStatusByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule, approvedStatus);
            var startedAndApprovedStatusList = new List<int> { startedStatusMasterId, approvedStatusMasterId };
            var areaUsageTransactionData = await _areaUsageLogrepo.FirstOrDefaultAsync(x => x.Id == areaUsageLogId && startedAndApprovedStatusList.Contains((int)x.StatusId));

            if (areaUsageTransactionData != null)
            {
                var allowedStatusList = new List<string> { verifiedStatus, rejectedStatus, cancelledStatus };
                var areaUsageStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.WIPModule, PMMSConsts.AreaUsageLogSubModule);
                var areaUsageNotInProgressStatusList = areaUsageStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
                var areaUsageTransaction = await _areaUsageLogrepo.FirstOrDefaultAsync(x => x.Id == areaUsageLogId && !areaUsageNotInProgressStatusList.Contains((int)x.StatusId));
                if (areaUsageTransaction != null)
                {
                    var data = ObjectMapper.Map<AreaUsageLogDto>(areaUsageTransaction);
                    data.CreatorName = _userRepository.FirstOrDefault(x => x.Id == areaUsageTransaction.CreatorUserId).FullName;
                    if (data.StatusId == startedStatusMasterId)
                    {
                        data.CanApproved = areaUsageTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    else if (data.StatusId == approvedStatusMasterId)
                    {
                        data.ApprovedByName = _userRepository.FirstOrDefault(x => x.Id == areaUsageTransaction.ApprovedBy).FullName;
                        data.CanVerified = data.ApprovedBy != (int)AbpSession.UserId && areaUsageTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    data.Id = areaUsageTransactionData.Id;
                    data.CubicalCode = Convert.ToString(areaUsageTransactionData.CubicalId);
                    data.CubicalId = areaUsageTransactionData.CubicalId;
                    data.OperatorName = areaUsageTransactionData.OperatorName;
                    data.ActivityID = areaUsageTransactionData.ActivityID;
                    data.StartTime = areaUsageTransactionData.StartTime;
                    data.StopTime = areaUsageTransactionData.StopTime;
                    // GetareaUsageCheckPoints(data);
                    return data;
                }
                else
                {
                    return new AreaUsageLogDto
                    {
                        IsInValidTransaction = true,
                        // areaUsageCheckpoints = new List<CheckpointDto>()
                    };
                }
            }
            var areaUsageTransactionDto = new AreaUsageLogDto
            {
                AreaUsageCheckpoints = new List<CheckpointDto>()
            };
            return areaUsageTransactionDto;
        }

        public async Task UpdateAreaUsageLogTransaction(AreaUsageLogDto input)
        {
            var AreaUsageLogTransaction = await _areaUsageLogrepo.GetAsync(input.Id);
            if (input.IsVerified)
            {
                var verfiedStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.AreaUsageLogSubModule, verifiedStatus);
                input.StatusId = verfiedStatusId;
                input.VerifiedBy = (int)AbpSession.UserId;
                input.StopTime = DateTime.Now;
            }
            else if (input.IsApproved)
            {
                var approvedStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.AreaUsageLogSubModule, approvedStatus);
                input.ApprovedBy = (int)AbpSession.UserId;
                input.StatusId = approvedStatusId;
                input.ApprovedTime = DateTime.Now;
            }
            else
            {
                var rejectStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.AreaUsageLogSubModule, rejectedStatus);
                input.StatusId = rejectStatusId;
                input.StopTime = DateTime.Now;
                var areaUsageLog = await _areaUsageLogrepo.GetAllListAsync(x => x.CubicalId == input.CubicalId && x.StopTime != null);
                var area = areaUsageLog.OrderByDescending(x => x.StopTime).Take(1).ToList().FirstOrDefault();
                if (area != null)
                {
                    area.Status = false;
                    await _areaUsageLogrepo.UpdateAsync(area);
                }
                //var equipmentUsageLog = await _equipmentUsageLogRepository.GetAllListAsync(x => x.EquipmentBracodeId == input.EquipmentBarcodeId && x.EndTime != null);
                //var equipment = equipmentUsageLog.OrderByDescending(x => x.EndTime).Take(1).ToList().FirstOrDefault();
                //if (equipment != null)
                //{
                //    equipment.Status = false;
                //    await _equipmentUsageLogRepository.UpdateAsync(equipment);
                //}

                //var cubicleAssignment = await _cubicleAssignmentsRepository.GetAllListAsync(x => x.CubicleBarcodeId == input.CubicleBarcodeId && x.EquipmentBarcodeId == input.EquipmentBarcodeId && x.ProcessOrderId == input.ProcessOrderId);
                //var assignement = cubicleAssignment.OrderByDescending(x => x.Id).Take(1).ToList().FirstOrDefault();
                //if (assignement != null)
                //{
                //    assignement.Status = false;
                //    await _cubicleAssignmentsRepository.UpdateAsync(assignement);
                //}
            }
            ObjectMapper.Map(input, AreaUsageLogTransaction);
            await _areaUsageLogrepo.UpdateAsync(AreaUsageLogTransaction);
        }

    }

}
