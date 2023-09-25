using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.CommonService.Dispensing.Dto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Modules;
using ELog.Application.WIP.WIPLineClearances.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.WIP.WIPLineClearances
{
    [PMMSAuthorize]
    public class WIPLineClearanceAppService : ApplicationService, IWIPLineClearanceAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<WIPLineClearanceCheckpoints> _lineClearanceCheckPointRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<WIPLineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Started).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Verified).ToLower();
        private readonly string approvedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Approved).ToLower();
        private readonly string rejectedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Rejected).ToLower();
        private readonly string cancelledStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Cancelled).ToLower();
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<CubicleAssignmentWIP> _cubicleAssignmentWIPRepository;
        private readonly IRepository<AreaUsageLog> _areaUsageLogRepository;
        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IRepository<ELog.Core.Entities.EquipmentUsageLog> _equipmentUsageLogRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<ELog.Core.Entities.AreaUsageLog> _areaUsageLogrepo;
        //private readonly IRepository<ELog.Core.Entities.EquipmentUsageLog> _equipmentUsageLog;
        private readonly IRepository<CubicleAssignmentWIP> _cubicleAssignmentsRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<MaterialMaster> _materialMaster;

        #endregion fields

        #region constructor

        public WIPLineClearanceAppService(
            IRepository<CheckpointMaster> checkpointRepository,
            IDispensingAppService dispensingAppService, IRepository<WIPLineClearanceTransaction> lineClearanceTransactionRepository,
            IRepository<WIPLineClearanceCheckpoints> lineClearanceCheckPointRepository, IRepository<User, long> userRepository,
             IRepository<ProcessOrderAfterRelease> processOrderRepository,
             IRepository<CubicleAssignmentWIP> cubicleAssignmentWIPRepository, IRepository<AreaUsageLog> areaUsageLogRepository,
             IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
              IModuleAppService moduleAppService, IRepository<StatusMaster> statusMasterRepository,
              IRepository<ELog.Core.Entities.EquipmentUsageLog> equipmentUsageLogRepository,
              IRepository<CubicleMaster> cubicleRepository, IRepository<EquipmentMaster> equipmentRepository,
               IRepository<ELog.Core.Entities.AreaUsageLog> areaUsageLogrepo,

               IRepository<CubicleAssignmentWIP> cubicleAssignmentsRepository, IHttpContextAccessor httpContextAccessor,
               IRepository<AreaMaster> areaRepository, IRepository<DepartmentMaster> departmentRepository,
                IRepository<MaterialMaster> materialmaster)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _checkpointRepository = checkpointRepository;
            _dispensingAppService = dispensingAppService;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _lineClearanceCheckPointRepository = lineClearanceCheckPointRepository;
            _processOrderRepository = processOrderRepository;
            _userRepository = userRepository;
            _cubicleAssignmentWIPRepository = cubicleAssignmentWIPRepository;
            _areaUsageLogRepository = areaUsageLogRepository;
            _inspectionChecklistRepository = inspectionChecklistRepository;
            _moduleAppService = moduleAppService;
            _statusMasterRepository = statusMasterRepository;
            _equipmentUsageLogRepository = equipmentUsageLogRepository;
            _cubicleRepository = cubicleRepository;
            _equipmentRepository = equipmentRepository;
            _areaUsageLogrepo = areaUsageLogrepo;
            //_equipmentUsageLog = equipmentUsageLog;
            _cubicleAssignmentsRepository = cubicleAssignmentsRepository;
            _httpContextAccessor = httpContextAccessor;
            _areaRepository = areaRepository;
            _departmentRepository = departmentRepository;
            _materialMaster = materialmaster;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.LineClearance_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<WIPLineClearanceTransactionDto> CreateAsync(CreateWIPLineClearanceDto input)
        {
            return await CreateLineClearanceTransaction(input, false);
        }



        [PMMSAuthorize(Permissions = PMMSPermissionConst.LineClearance_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateAsync(WIPLineClearanceTransactionDto input)
        {
            await UpdateLineClearanceTransaction(input);
        }


        public async Task<WIPLineClearanceTransactionDto> ValidateLineClearanceAsync(int processOrderId)
        {
            var startedStatusMasterId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, startedStatus);
            var approvedStatusMasterId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, approvedStatus);
            var startedAndApprovedStatusList = new List<int> { startedStatusMasterId, approvedStatusMasterId };
            var lineClearanceTransactionData = await _lineClearanceTransactionRepository.FirstOrDefaultAsync(x => x.ProcessOrderId == processOrderId && startedAndApprovedStatusList.Contains(x.StatusId));
            if (lineClearanceTransactionData != null)
            {
                var allowedStatusList = new List<string> { verifiedStatus, rejectedStatus, cancelledStatus };
                var lineClearanceStatusList = await _dispensingAppService.GetStatusListByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
                var lineClearanceNotInProgressStatusList = lineClearanceStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
                var lineClearanceTransaction = await _lineClearanceTransactionRepository.FirstOrDefaultAsync(x => x.ProcessOrderId == processOrderId && !lineClearanceNotInProgressStatusList.Contains(x.StatusId));
                if (lineClearanceTransaction != null)
                {
                    var data = ObjectMapper.Map<WIPLineClearanceTransactionDto>(lineClearanceTransaction);
                    data.CreatorName = _userRepository.FirstOrDefault(x => x.Id == lineClearanceTransaction.CreatorUserId).FullName;
                    if (data.StatusId == startedStatusMasterId)
                    {
                        data.CanApproved = lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    else if (data.StatusId == approvedStatusMasterId)
                    {
                        data.ApprovedByName = _userRepository.FirstOrDefault(x => x.Id == lineClearanceTransaction.ApprovedBy).FullName;
                        data.CanVerified = data.ApprovedBy != (int)AbpSession.UserId && lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    data.ProcessOrderId = lineClearanceTransactionData.ProcessOrderId;
                    data.EquipmentBarcodeId = lineClearanceTransactionData.EquipmentBarcodeId;
                    data.CubicleBarcodeId = lineClearanceTransactionData.CubicleBarcodeId;
                    data.ChecklistTypeId = lineClearanceTransactionData.ChecklistTypeId;
                    var cubicle = await _cubicleRepository.GetAsync(data.CubicleBarcodeId);
                    var equipment = await _equipmentRepository.GetAsync(data.EquipmentBarcodeId);
                    data.CubicleBarcode = cubicle.CubicleCode;
                    data.EquipmentBarcode = equipment.EquipmentCode;
                    GetLineClearanceCheckPoints(data);
                    return data;
                }
                else
                {
                    return new WIPLineClearanceTransactionDto
                    {
                        IsInValidTransaction = true,
                        LineClearanceCheckpoints = new List<CheckpointDto>()
                    };
                }
            }
            var lineClearanceTransactionDto = new WIPLineClearanceTransactionDto
            {
                LineClearanceCheckpoints = new List<CheckpointDto>()
            };
            return lineClearanceTransactionDto;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {

            var productCodes = from po in _processOrderRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   Id = po.ProductCodeId,
                                   PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };

            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }
        public string GetProductDesc(string input)
        {

            var productDesc = _materialMaster.GetAll().Where(a => a.MaterialCode == input.Trim()).Select(a => a.MaterialDescription).FirstOrDefault();

            return productDesc;
        }

        public async Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId)
        {
            var checkPointQuery = from inspectionCheckList in _inspectionChecklistRepository.GetAll().Where(x => x.ChecklistTypeId == checklistId)
                                  join checkPoint in _checkpointRepository.GetAll()
                                  on inspectionCheckList.Id equals checkPoint.InspectionChecklistId
                                  select new CheckpointDto
                                  {
                                      CheckPointId = checkPoint.Id,
                                      InspectionChecklistId = checklistId,
                                      CheckpointName = checkPoint.CheckpointName,
                                      ValueTag = checkPoint.ValueTag,
                                      AcceptanceValue = checkPoint.AcceptanceValue,
                                      CheckpointTypeId = checkPoint.CheckpointTypeId,
                                      ModeId = checkPoint.ModeId,
                                      Observation = null,
                                      DiscrepancyRemark = null,

                                  };

            if (modeId != 0)
            {
                checkPointQuery = checkPointQuery.Where(x => x.ModeId == modeId);
            }
            return await checkPointQuery.ToListAsync() ?? default;
        }

        public async Task<BarcodeValidationDto> IsAssignedProcessOrder(WIPLineClearanceTransactionDto input)
        {


            var allowedStatusList = new List<string> { verifiedStatus, nameof(LineClearanceHeaderStatus.Rejected).ToLower(), nameof(LineClearanceHeaderStatus.Cancelled).ToLower() };
            var lineClearanceStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
            var lineClearanceInProgressStatusList = lineClearanceStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
            var checkInProgressLineClearanceTransaction = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.ProcessOrderId == input.ProcessOrderId && !lineClearanceInProgressStatusList.Contains(x.StatusId));
            var barcodeValidationDto = new BarcodeValidationDto();
            if (checkInProgressLineClearanceTransaction.Any())
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }


            var cubicleCleaned = await _areaUsageLogRepository.GetAllListAsync(x => x.CubicalId == input.CubicleBarcodeId && x.StopTime != null && x.Status == true);
            var cubicle = cubicleCleaned.OrderByDescending(x => x.StopTime).Take(1).ToList().FirstOrDefault();
            var rejectStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, rejectedStatus);
            var processorderAssignedwithCubicleAndEquipment = await _cubicleAssignmentWIPRepository.GetAllListAsync(x => x.CubicleBarcodeId == input.CubicleBarcodeId && x.EquipmentBarcodeId == input.EquipmentBarcodeId && x.ProcessOrderId == input.ProcessOrderId && x.Status == true);
            var usedProcessOrder = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.ProcessOrderId == input.ProcessOrderId && x.StatusId != rejectStatusId);
            var equipmentCleaned = await _equipmentUsageLogRepository.GetAllListAsync(x => x.EquipmentBracodeId == input.EquipmentBarcodeId && x.EndTime != null && x.Status == true);
            var equipment = equipmentCleaned.OrderByDescending(x => x.EndTime).Take(1).ToList().FirstOrDefault();


            if (cubicle == null)
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.CubicleNotCleaned;
                return barcodeValidationDto;
            }
            else if (equipment == null)
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.EquipmentNotCleaned;
                return barcodeValidationDto;
            }
            else if (processorderAssignedwithCubicleAndEquipment.Count == 0)
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.EquipmentOrCubicleNotAssignedToProcessOrder;
                return barcodeValidationDto;
            }
            else if (usedProcessOrder.Any())
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.LineClearenceAlreadyDone;
                return barcodeValidationDto;
            }

            else
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
        }
        public async Task<BarcodeValidationDto> IsCheckProcessOrderAlreadyUsedAsync(int processOderId)
        {
            var allowedStatusList = new List<string> { verifiedStatus, nameof(LineClearanceHeaderStatus.Rejected).ToLower(), nameof(LineClearanceHeaderStatus.Cancelled).ToLower() };
            var lineClearanceStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
            var lineClearanceInProgressStatusList = lineClearanceStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
            var checkInProgressLineClearanceTransaction = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.ProcessOrderId == processOderId && !lineClearanceInProgressStatusList.Contains(x.StatusId));
            var barcodeValidationDto = new BarcodeValidationDto();
            if (checkInProgressLineClearanceTransaction.Any())
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }

            var rejectStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, rejectedStatus);

            var usedProcessOrder = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.ProcessOrderId == processOderId && x.StatusId != rejectStatusId);

            if (usedProcessOrder.Any())
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.LineClearenceAlreadyDone;
                return barcodeValidationDto;
            }
            else
            {

                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
        }

        public async Task<List<StatusMaster>> GetStatusListByModuleSubModuleName(string module, string submodule)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId).Select(x => new StatusMaster { Id = x.Id, Status = x.Status })?
                        .ToListAsync() ?? default;
        }

        public async Task<int> GetStatusByModuleSubModuleName(string module, string submodule, string status)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == status).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<List<EquipmentCleaningBarcodeDto>> GetAllEquipmentBarcodeAsync(string input)
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
                                     where equipment.ApprovalStatusId == approvedApprovalStatusId && equipment.IsActive
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
                    var cubicle = await (from equipment in _equipmentUsageLogRepository.GetAll()
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
        #endregion public

        #region private

        private async Task<WIPLineClearanceTransactionDto> CreateLineClearanceTransaction(CreateWIPLineClearanceDto input, bool isSampling)
        {
            var startedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, startedStatus);
            var lineClearanceTransaction = ObjectMapper.Map<WIPLineClearanceTransaction>(input);
            lineClearanceTransaction.TenantId = AbpSession.TenantId;
            lineClearanceTransaction.ClearanceDate = DateTime.Now;
            lineClearanceTransaction.StartTime = DateTime.Now;
            lineClearanceTransaction.StatusId = startedStatusId;
            lineClearanceTransaction.IsSampling = isSampling;
            await _lineClearanceTransactionRepository.InsertAsync(lineClearanceTransaction);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WIPLineClearanceTransactionDto>(lineClearanceTransaction);
        }

        private async Task UpdateLineClearanceTransaction(WIPLineClearanceTransactionDto input)
        {
            var lineClearanceTransaction = await _lineClearanceTransactionRepository.GetAsync(input.Id);
            if (input.IsVerified)
            {
                var verfiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, verifiedStatus);
                input.StatusId = verfiedStatusId;
                input.VerifiedBy = (int)AbpSession.UserId;
                input.StopTime = DateTime.Now;
            }
            else if (input.IsApproved)
            {
                var approvedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, approvedStatus);
                input.ApprovedBy = (int)AbpSession.UserId;
                input.StatusId = approvedStatusId;
                input.ApprovedTime = DateTime.Now;
            }
            else
            {
                var rejectStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, rejectedStatus);
                input.StatusId = rejectStatusId;
                input.StopTime = DateTime.Now;
                var areaUsageLog = await _areaUsageLogrepo.GetAllListAsync(x => x.CubicalId == input.CubicleBarcodeId && x.StopTime != null);
                var area = areaUsageLog.OrderByDescending(x => x.StopTime).Take(1).ToList().FirstOrDefault();
                if (area != null)
                {
                    area.Status = false;
                    await _areaUsageLogrepo.UpdateAsync(area);
                }
                var equipmentUsageLog = await _equipmentUsageLogRepository.GetAllListAsync(x => x.EquipmentBracodeId == input.EquipmentBarcodeId && x.EndTime != null);
                var equipment = equipmentUsageLog.OrderByDescending(x => x.EndTime).Take(1).ToList().FirstOrDefault();
                if (equipment != null)
                {
                    equipment.Status = false;
                    await _equipmentUsageLogRepository.UpdateAsync(equipment);
                }

                var cubicleAssignment = await _cubicleAssignmentsRepository.GetAllListAsync(x => x.CubicleBarcodeId == input.CubicleBarcodeId && x.EquipmentBarcodeId == input.EquipmentBarcodeId && x.ProcessOrderId == input.ProcessOrderId);
                var assignement = cubicleAssignment.OrderByDescending(x => x.Id).Take(1).ToList().FirstOrDefault();
                if (assignement != null)
                {
                    assignement.Status = false;
                    await _cubicleAssignmentsRepository.UpdateAsync(assignement);
                }
            }
            ObjectMapper.Map(input, lineClearanceTransaction);
            await _lineClearanceTransactionRepository.UpdateAsync(lineClearanceTransaction);
        }

        private void GetLineClearanceCheckPoints(WIPLineClearanceTransactionDto lineclearanceCheckpoint)
        {
            lineclearanceCheckpoint.LineClearanceCheckpoints = (from detail in _lineClearanceCheckPointRepository.GetAll()
                                                                join checkpoint in _checkpointRepository.GetAll()
                                                                on detail.CheckPointId equals checkpoint.Id
                                                                where detail.LineClearanceTransactionId == lineclearanceCheckpoint.Id
                                                                select new CheckpointDto
                                                                {
                                                                    Id = detail.Id,
                                                                    CheckPointId = checkpoint.Id,
                                                                    CheckpointName = checkpoint.CheckpointName,
                                                                    ValueTag = checkpoint.ValueTag,
                                                                    AcceptanceValue = checkpoint.AcceptanceValue,
                                                                    CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                                    ModeId = checkpoint.ModeId,
                                                                    Observation = detail.Observation,
                                                                    DiscrepancyRemark = detail.Remark,
                                                                }).ToList() ?? default;
        }

        #endregion private
    }
}
