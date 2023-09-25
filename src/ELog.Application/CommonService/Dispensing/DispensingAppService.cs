using Abp.Application.Services;
using Abp.Domain.Repositories;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing.Dto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Dispensing.MaterialDispensing.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Modules;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.CommonService.Dispensing
{
    [PMMSAuthorize]
    public class DispensingAppService : ApplicationService, IDispensingAppService
    {
        public const string NotInUse = "Not in Use";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<CubicleCleaningTypeMaster> _cubicleCleaningTypeMasterRepository;
        private readonly IRepository<EquipmentCleaningTypeMaster> _equipmentCleaningTypeMasterRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IRepository<EquipmentCleaningTransaction> _equipmentCleaningTransactionRepository;
        private readonly IRepository<CubicleCleaningDailyStatus> _cubicleCleaningDailyStatusRepository;
        private readonly IRepository<EquipmentCleaningStatus> _equipmentCleaningStatusRepository;
        private readonly IRepository<LineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly string startedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Started).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Verified).ToLower();
        private readonly string cleanedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Cleaned).ToLower();
        private readonly string lineClearanceApprovedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Approved).ToLower();
        private readonly string lineClearanceVerifiedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Verified).ToLower();
        private readonly string lineClearanceRejectedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Rejected).ToLower();
        private readonly string lineClearanceCancelledStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Cancelled).ToLower();
        private readonly string lineClearanceStartedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Started).ToLower();

        private readonly string StageOutInProgressStatus = nameof(StageOutStatus.InProgress).ToLower();
        private readonly string StageOutCompletedStatus = nameof(StageOutStatus.Completed).ToLower();
        private readonly IRepository<StageOutHeader> _stageOutHeaderRepository;

        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<WMCalibratedLatestMachineDetail> _wmcalibratedMachineRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementMasterRepository;
        private readonly IRepository<UnitOfMeasurementTypeMaster> _unitOfMeasurementTypeMasterRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly string PackUOMType = nameof(UOMType.Pack).ToLower();
        private readonly string openStatus = nameof(PMMSEnums.CubicleAssignmentGroupStatus.Open).ToLower();

        public DispensingAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<CubicleMaster> cubicleRepository, IRepository<AreaMaster> areaRepository,
            IRepository<CubicleCleaningTypeMaster> cubicleCleaningTypeMasterRepository,
            IRepository<CheckpointMaster> checkpointRepository,
            IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
            IRepository<EquipmentCleaningTypeMaster> equipmentCleaningTypeMasterRepository,
            IRepository<EquipmentMaster> equipmentRepository,
            IRepository<DepartmentMaster> departmentRepository,
            IModuleAppService moduleAppService,
            IRepository<SubModuleMaster> subModuleRepository,
            IRepository<StatusMaster> statusMasterRepository, IRepository<StageOutHeader> stageOutHeaderRepository,
            IRepository<EquipmentCleaningTransaction> equipmentCleaningTransactionRepository,
            IRepository<CubicleCleaningDailyStatus> cubicleCleaningDailyStatusRepository, IRepository<EquipmentCleaningStatus> equipmentCleaningStatusRepository,
            IRepository<LineClearanceTransaction> lineClearanceTransactionRepository,
            IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository, IRepository<WeighingMachineMaster> weighingMachineRepository,
             IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository, IRepository<WMCalibratedLatestMachineDetail> wmcalibratedMachineRepository,
            IRepository<MaterialMaster> materialMasterRepository, IRepository<UnitOfMeasurementMaster> unitOfMeasurementMasterRepository,
            IRepository<UnitOfMeasurementTypeMaster> unitOfMeasurementTypeMasterRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _cubicleRepository = cubicleRepository;
            _areaRepository = areaRepository;
            _checkpointRepository = checkpointRepository;
            _inspectionChecklistRepository = inspectionChecklistRepository;
            _cubicleCleaningTypeMasterRepository = cubicleCleaningTypeMasterRepository;
            _equipmentCleaningTypeMasterRepository = equipmentCleaningTypeMasterRepository;
            _subModuleRepository = subModuleRepository;
            _equipmentRepository = equipmentRepository;
            _departmentRepository = departmentRepository;
            _moduleAppService = moduleAppService;
            _equipmentCleaningTransactionRepository = equipmentCleaningTransactionRepository;
            _statusMasterRepository = statusMasterRepository;
            _cubicleCleaningDailyStatusRepository = cubicleCleaningDailyStatusRepository;
            _equipmentCleaningStatusRepository = equipmentCleaningStatusRepository;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _wmcalibratedMachineRepository = wmcalibratedMachineRepository;
            _materialMasterRepository = materialMasterRepository;
            _unitOfMeasurementMasterRepository = unitOfMeasurementMasterRepository;
            _unitOfMeasurementTypeMasterRepository = unitOfMeasurementTypeMasterRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _stageOutHeaderRepository = stageOutHeaderRepository;
        }

        public async Task<List<CubicleBarcodeDto>> GetAllCubicleBarcodeAsync(string input)
        {

            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var cubicleQuery = from cubicle in _cubicleRepository.GetAll()
                               join area in _areaRepository.GetAll()
                               on cubicle.AreaId equals area.Id
                               where cubicle.ApprovalStatusId == approvedApprovalStatusId && cubicle.IsActive
                               select new CubicleBarcodeDto
                               {
                                   Id = cubicle.Id,
                                   Value = cubicle.CubicleCode,
                                   PlantId = cubicle.PlantId,
                                   AreaCode = area.AreaCode + " - " + area.AreaName
                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input)))
            {
                cubicleQuery = cubicleQuery.Where(x => x.Value.Contains(input.ToLower()));
            }


            return await cubicleQuery.ToListAsync() ?? default;
        }

        public async Task<List<EquipmentCleaningBarcodeDto>> GetAllEquipmentBarcodeAsync(string input, bool isSampling)
        {
            var equipments = new List<EquipmentCleaningBarcodeDto>();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
                var status = (from equipment in _equipmentRepository.GetAll()
                              join equipmentStatus in _equipmentCleaningStatusRepository.GetAll()
                              on equipment.Id equals equipmentStatus.EquipmentId
                              join statusMaster in _statusMasterRepository.GetAll()
                              on equipmentStatus.StatusId equals statusMaster.Id
                              where equipmentStatus.IsSampling == isSampling && equipment.EquipmentCode.ToLower() == input.ToLower()
                              select statusMaster.Status == EquipmentCleaningHeaderStatus.Verified.ToString() ? NotInUse :
                              statusMaster.Status == EquipmentCleaningHeaderStatus.Cleaned.ToString() ? EquipmentCleaningHeaderStatus.Cleaned.ToString() :
                              statusMaster.Status == EquipmentCleaningHeaderStatus.Started.ToString() ? EquipmentCleaningHeaderStatus.Uncleaned.ToString() :
                              NotInUse).ToList();
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
                                         AreaId = area.Id,
                                         AreaBarcode = area.AreaCode + " - " + area.AreaName,
                                         EquipmentTypeId = equipment.IsPortable == true ? (int)PMMSEnums.EquipementType.Portable : (int)PMMSEnums.EquipementType.Fixed,
                                         Status = status.Count == 0 ? NotInUse : status[0]
                                     };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    equipmentQuery = equipmentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }

                equipments = await equipmentQuery.ToListAsync();
                foreach (var item in equipments)
                {
                    var cubicle = await (from equipment in _equipmentCleaningTransactionRepository.GetAll()
                                         join cubicleMaster in _cubicleRepository.GetAll()
                                         on equipment.CubicleId equals cubicleMaster.Id
                                         where equipment.EquipmentId == item.EquipmentId && equipment.VerifiedTime == null && equipment.IsSampling == isSampling
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

        public async Task<List<SelectListDto>> GetAllCubicleCleaningType()
        {
            return await _cubicleCleaningTypeMasterRepository.GetAll().OrderBy(x => x.Value)
                  .Select(x => new SelectListDto { Id = x.Id, Value = x.Value })?
                  .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllEquipmentCleaningType()
        {
            return await _equipmentCleaningTypeMasterRepository.GetAll().OrderBy(x => x.Value)
                  .Select(x => new SelectListDto { Id = x.Id, Value = x.Value })?
                  .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllGroupCodeAsync(int cubicleId, bool isSampling)
        {
            var lineClearanceCompletedId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, verifiedStatus);
            var checkInProgressCubicle = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.StatusId == lineClearanceCompletedId && x.IsSampling == isSampling);
            var checkInProgressCubicleGroupIds = checkInProgressCubicle.Select(a => a.GroupId).ToList();

            var groupQuery = from cubicleAssignmentHeader in _cubicleAssignmentHeaderRepository.GetAll()
                             join cubicleAssignementDetail in _cubicleAssignmentDetailRepository.GetAll()
                             on cubicleAssignmentHeader.Id equals cubicleAssignementDetail.CubicleAssignmentHeaderId
                             where !checkInProgressCubicleGroupIds.Contains(cubicleAssignmentHeader.Id) &&
                             cubicleAssignmentHeader.IsSampling == isSampling && cubicleAssignementDetail.CubicleId == cubicleId
                             select new SelectListDto
                             {
                                 Id = cubicleAssignmentHeader.Id,
                                 Value = cubicleAssignmentHeader.GroupId,
                             };
            return await groupQuery.Distinct().ToListAsync() ?? default;
        }

        public async Task<List<CheckpointDto>> GetCheckpointsBySubModuleIdIdAsync(string subModuleName, int modeId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var subModuleId = subModuleName != null ? await _subModuleRepository.FirstOrDefaultAsync(a => a.Name.ToLower() == subModuleName.ToLower()) : null;
            var checkPointQuery = from inspectionChecklist in _inspectionChecklistRepository.GetAll()
                                  join checkpoint in _checkpointRepository.GetAll()
                                  on inspectionChecklist.Id equals checkpoint.InspectionChecklistId
                                  where inspectionChecklist.ApprovalStatusId == approvedApprovalStatusId && inspectionChecklist.IsActive
                                  && inspectionChecklist.SubModuleId == subModuleId.Id
                                  select new CheckpointDto
                                  {
                                      CheckPointId = checkpoint.Id,
                                      CheckpointName = checkpoint.CheckpointName,
                                      ValueTag = checkpoint.ValueTag,
                                      AcceptanceValue = checkpoint.AcceptanceValue,
                                      CheckpointTypeId = checkpoint.CheckpointTypeId,
                                      ModeId = checkpoint.ModeId,
                                      Observation = null,
                                      DiscrepancyRemark = null,
                                      PlantId = inspectionChecklist.PlantId
                                  };
            if (modeId != 0)
            {
                checkPointQuery = checkPointQuery.Where(x => x.ModeId == modeId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                checkPointQuery = checkPointQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await checkPointQuery.ToListAsync() ?? default;
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

        public async Task<BarcodeValidationDto> IsCubicleCleaned(int cubicleId, bool isSampling)
        {
            var allowedStatusList = new List<string> { verifiedStatus, nameof(LineClearanceHeaderStatus.Rejected).ToLower(), nameof(LineClearanceHeaderStatus.Cancelled).ToLower() };
            var lineClearanceStatusList = await GetStatusListByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
            var lineClearanceInProgressStatusList = lineClearanceStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
            var checkInProgressLineClearanceTransaction = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.CubicleId == cubicleId && !lineClearanceInProgressStatusList.Contains(x.StatusId) && x.IsSampling == isSampling);
            var barcodeValidationDto = new BarcodeValidationDto();
            if (checkInProgressLineClearanceTransaction.Any())
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
            var localDate = DateTime.Now;
            var cubicleCleaningVerifiedStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleCleaningSubModule, verifiedStatus);

            var cubicleCleaningVerifiedStatusEntry = await _cubicleCleaningDailyStatusRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.CleaningDate.Day == localDate.Day
           && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.StatusId == cubicleCleaningVerifiedStatusId && x.IsSampling == isSampling);
            bool isAssignedEquipmentVerfied = true;
            var availableEquipmentStatusIdForCubicle = from equipmentCleaningTransaction in _equipmentCleaningTransactionRepository.GetAll()
                                                       join equipCleanStatus in _equipmentCleaningStatusRepository.GetAll()
                                                       on equipmentCleaningTransaction.EquipmentId equals equipCleanStatus.EquipmentId
                                                       where equipmentCleaningTransaction.CubicleId == cubicleId && equipmentCleaningTransaction.IsSampling == isSampling && equipCleanStatus.IsSampling == isSampling
                                                       select equipCleanStatus.StatusId;
            if (availableEquipmentStatusIdForCubicle.Any())
            {
                var equipmentCleaningVerifiedStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, verifiedStatus);
                isAssignedEquipmentVerfied = availableEquipmentStatusIdForCubicle.Any(x => x == equipmentCleaningVerifiedStatusId);
            }
            if (cubicleCleaningVerifiedStatusEntry.Any() && isAssignedEquipmentVerfied)
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
            else
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = isAssignedEquipmentVerfied ? PMMSValidationConst.CubicleUncleaned : PMMSValidationConst.LineClearanceEuipementNotClean;
                return barcodeValidationDto;
            }
        }

        public async Task RejectLineClearanceTransaction(int? cubicleId, int? groupId, bool isSampling)
        {
            var lineClearanceCompleted = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, nameof(PMMSEnums.LineClearanceHeaderStatus.Rejected).ToLower());
            var lineClearanceRejected = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, verifiedStatus);
            var cancelledStatus = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, nameof(LineClearanceHeaderStatus.Cancelled).ToLower());
            var lineClearanceTransaction = await _lineClearanceTransactionRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleId && x.IsSampling == isSampling && x.GroupId == groupId && (x.StatusId != lineClearanceCompleted || x.StatusId != lineClearanceRejected || x.StatusId != cancelledStatus));
            if (lineClearanceTransaction != null)
            {
                lineClearanceTransaction.StatusId = cancelledStatus;
                await _lineClearanceTransactionRepository.UpdateAsync(lineClearanceTransaction);
            }
        }

        public async Task<List<StatusMaster>> GetStatusListByModuleSubModuleName(string module, string submodule)
        {
            var moduleId = await _moduleAppService.GetModuleByName(module);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submodule);
            return await _statusMasterRepository.GetAll().Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId).Select(x => new StatusMaster { Id = x.Id, Status = x.Status })?
                        .ToListAsync() ?? default;
        }

        public HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, string ValidationError)
        {
            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
            responseDto.Error = ValidationError;
            return responseDto;
        }

        private async Task<float> GetConvertedQuantityByUOM(string materialCode, string baseUOM, string ConversionUOM, float quantity)
        {
            if (baseUOM == ConversionUOM)
            {
                return quantity;
            }
            var numeratorDenominatorValue = await _materialMasterRepository.GetAll().Where(x =>
              x.MaterialCode == materialCode &&
              x.BaseUOM == baseUOM &&
              x.ConversionUOM == ConversionUOM)
                 .Select(x => new { x.Numerator, x.Denominator }).FirstOrDefaultAsync();
            if (numeratorDenominatorValue != null)
            {
                return (numeratorDenominatorValue.Numerator / numeratorDenominatorValue.Denominator) * quantity;
            }
            return quantity;
        }

        public async Task<float> GetConvertedQuantityByUOM(string materialCode, int baseUOMId, int ConversionUOMId, float quantity)
        {
            var uomDictionary = await _unitOfMeasurementMasterRepository.GetAll().Where(x => x.Id == baseUOMId | x.Id == ConversionUOMId)
                   .Select(x => new { x.Id, x.UnitOfMeasurement })
                   .ToDictionaryAsync(x => x.Id, x => x.UnitOfMeasurement);
            return await GetConvertedQuantityByUOM(materialCode, uomDictionary[baseUOMId], uomDictionary[ConversionUOMId], quantity);
        }
        /// <summary>
        /// Used for getting Uom code under selected material code..
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MaterialDispensingInternalDto>> GetAllUOMByMaterialCodeAsync(string input)
        {
            var materialUomQuery = from materialMaster in _materialMasterRepository.GetAll()
                                   join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                                   on materialMaster.BaseUOM equals uomMaster.UnitOfMeasurement
                                   join uomTypemaster in _unitOfMeasurementTypeMasterRepository.GetAll()
                                   on uomMaster.UnitOfMeasurementTypeId equals uomTypemaster.Id
                                   where materialMaster.MaterialCode.ToLower() == input.ToLower()
                                   select new MaterialDispensingInternalDto
                                   {
                                       ConversionUOMName = materialMaster.BaseUOM + " - " + uomMaster.Name,
                                       Denominator = materialMaster.Denominator,
                                       Numerator = materialMaster.Numerator,
                                       UomId = uomMaster.Id,
                                       UnitOfMeasurementTypeId = uomTypemaster.Id,
                                       UOMType = uomTypemaster.UnitOfMeasurementTypeName,
                                       IsPackUOM = uomTypemaster.UnitOfMeasurementTypeName == PackUOMType ? true : false,
                                   };
            return await materialUomQuery.ToListAsync() ?? default;
        }
        /// <summary>
        /// Used for getting calibrated weighing balance.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetBalanceByBarcode(string input, int uomId)
        {
            var responseDto = new HTTPResponseDto();
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var weighingMachineQuery = _weighingMachineRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.IsActive
            && x.WeighingMachineCode.ToLower() == input.ToLower()).OrderBy(x => x.WeighingMachineCode)
                    .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.WeighingMachineCode, PlantId = x.SubPlantId });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                weighingMachineQuery = weighingMachineQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var scanBalanceResult = await weighingMachineQuery.FirstOrDefaultAsync();
            if (scanBalanceResult != null && uomId != 0)
            {
                int scanBalanceId = Convert.ToInt32(scanBalanceResult.Id);
                //Get all suggested scan balance
                var suggestedBalance = await GetAllSuggestedBalancesAsync(uomId);
                //Check valid balance scanned
                var validBalanceFromSuggestedBalance = suggestedBalance.Where(a => (int)a.Id == scanBalanceId)
                    .Select(x => new SelectListDto { Id = (int)x.Id, Value = x.Value }).ToList();
                if (validBalanceFromSuggestedBalance.Any())
                {
                    responseDto.ResultObject = validBalanceFromSuggestedBalance;
                    return responseDto;
                }
                else
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.BalanceIsNotFromSuggestedBalanceValidation);
                }
            }
            else
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.BalanceIdNotFound);
            }
        }
        public async Task<List<SelectListDto>> GetAllSuggestedBalancesAsync(int uomId)
        {
            var currentDateTime = DateTime.Now;
            var weighingMachineBalanceList = await (from weighingMachineMaster in _weighingMachineRepository.GetAll()
                                                    where weighingMachineMaster.UnitOfMeasurementId == uomId
                                                    select new SelectListDto
                                                    {
                                                        Id = weighingMachineMaster.Id,
                                                        Value = weighingMachineMaster.WeighingMachineCode
                                                    }).ToListAsync();
            if (weighingMachineBalanceList.Any())
            {
                return weighingMachineBalanceList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }
            return default;
        }
        /// <summary>
        /// Used for getting Uom code under selected material code,sapBtachNumber,
        /// </summary>
        /// <param name="materialCode"></param>
        /// <param name="sapBatchNumber"></param>
        /// <param name="inspectionLotId"></param>
        /// <returns></returns>
        public async Task<List<MaterialDispensingInternalDto>> GetAllSamplingUOMByMaterialCodeAsync(string materialCode, string sapBatchNumber, int inspectionLotId)
        {
            var baseUnitOfMeasurement = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == materialCode.ToLower() &&
              x.SAPBatchNo == sapBatchNumber.ToLower() && x.InspectionLotId == inspectionLotId).Select(x => x.UnitOfMeasurement)
             .FirstOrDefaultAsync();

            if (baseUnitOfMeasurement != null)
            {
                var materialUomQuery = from materialMaster in _materialMasterRepository.GetAll()
                                       join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                                       on materialMaster.ConversionUOM.ToLower() equals uomMaster.UnitOfMeasurement.ToLower()
                                       join uomTypemaster in _unitOfMeasurementTypeMasterRepository.GetAll()
                                       on uomMaster.UnitOfMeasurementTypeId equals uomTypemaster.Id
                                       where materialMaster.MaterialCode.ToLower() == materialCode.ToLower()
                                       && materialMaster.BaseUOM.ToLower() == baseUnitOfMeasurement.ToLower()
                                       select new MaterialDispensingInternalDto
                                       {
                                           ConversionUOMName = materialMaster.ConversionUOM,
                                           Denominator = materialMaster.Denominator,
                                           Numerator = materialMaster.Numerator,
                                           UomId = uomMaster.Id,
                                           UnitOfMeasurementTypeId = uomTypemaster.Id,
                                           UOMType = uomTypemaster.UnitOfMeasurementTypeName,
                                           IsPackUOM = uomTypemaster.UnitOfMeasurementTypeName == PackUOMType,
                                       };
                return await materialUomQuery.ToListAsync() ?? default;
            }

            return default;
        }
        public async Task<DispensingUnitOfMeasurementDto> GetSamplingBaseUOMAsync(string materialCode, int? inspectionLotId, string SAPBatchNo)
        {
            return await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                          join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                          on processOrderMaterial.UnitOfMeasurement equals uomMaster.UnitOfMeasurement
                          where processOrderMaterial.ItemCode.ToLower() == materialCode.ToLower()
                          && processOrderMaterial.InspectionLotId == inspectionLotId
                          && processOrderMaterial.SAPBatchNo.ToLower() == SAPBatchNo.ToLower()
                          select new DispensingUnitOfMeasurementDto { Id = uomMaster.Id, UnitOfMeasurement = uomMaster.UnitOfMeasurement })
                          .FirstOrDefaultAsync();
        }
        public async Task<string> GetCubicleCleaningStatus(int cubicleId, bool isSampling)
        {
            var localDate = DateTime.Now;
            var cubicleStatus = PMMSEnums.CubicleStatus.NotInUse.GetAttribute<DisplayAttribute>().Name;
            var uncleanStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleCleaningSubModule, nameof(PMMSEnums.CubicleCleaningHeaderStatus.Uncleaned).ToLower());
            var cubicleCleaningCleaningEntry = await _cubicleCleaningDailyStatusRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.CleaningDate.Day == localDate.Day
                                                && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.IsSampling == isSampling && x.StatusId != uncleanStatusId);

            if (cubicleCleaningCleaningEntry.Any())
            {
                var cubicleCleaningStatusList = await GeSubmoduleAllStatusList(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleCleaningSubModule);
                if (cubicleCleaningCleaningEntry.FirstOrDefault().StatusId == cubicleCleaningStatusList.FirstOrDefault(a => a.Status.ToLower() == startedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else if (cubicleCleaningCleaningEntry.FirstOrDefault().StatusId == cubicleCleaningStatusList.FirstOrDefault(a => a.Status.ToLower() == cleanedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Cleaned.GetAttribute<DisplayAttribute>().Name;
                }
            }
            return cubicleStatus;
        }
        public async Task<string> GetCubicleLineClearanceStatus(int cubicleId, bool isSampling)
        {
            var localDate = DateTime.Now;
            var cubicleStatus = PMMSEnums.CubicleStatus.NotInUse.GetAttribute<DisplayAttribute>().Name;
            var lineClearanceStatusList = await GeSubmoduleAllStatusList(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
            var lineClearanceRejectedId = lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceRejectedStatus).Id;
            var lineClearanceApprovedStatusEntry = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.ClearanceDate.Day == localDate.Day
                                                            && x.ClearanceDate.Month == localDate.Month && x.ClearanceDate.Year == localDate.Year && x.IsSampling == isSampling && x.CubicleId == cubicleId
                                                            && x.StatusId != lineClearanceRejectedId);

            if (lineClearanceApprovedStatusEntry.Any())
            {

                if (lineClearanceApprovedStatusEntry.FirstOrDefault().StatusId == lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceStartedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else if (lineClearanceApprovedStatusEntry.FirstOrDefault().StatusId == lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceApprovedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Cleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.NotInUse.GetAttribute<DisplayAttribute>().Name;
                }
            }
            return cubicleStatus;
        }

        public async Task<string> GetCubicleOverallStatus(int cubicleId, bool isSampling)
        {
            var cubicleStatus = PMMSEnums.CubicleStatus.NotInUse.GetAttribute<DisplayAttribute>().Name;
            var cubicleMaster = await _cubicleRepository.GetAll().Where(x => x.Id == cubicleId).FirstOrDefaultAsync();
            if (cubicleMaster == null)
            {
                return "Cubicle is not found";
            }
            var localDate = DateTime.Now;
            var groupOpenStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, openStatus);
            var cubicleAssigement = from cubicleAssignmentHeader in _cubicleAssignmentHeaderRepository.GetAll()
                                    join cubicleAssignementDetail in _cubicleAssignmentDetailRepository.GetAll()
                                    on cubicleAssignmentHeader.Id equals cubicleAssignementDetail.CubicleAssignmentHeaderId
                                    where cubicleAssignmentHeader.IsSampling == isSampling && cubicleAssignementDetail.CubicleId == cubicleId
                                    && cubicleAssignmentHeader.GroupStatusId == groupOpenStatusId
                                    select cubicleAssignmentHeader;
            if (cubicleAssigement.Any())
            {
                cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;
            }
            var cubicleCleaningCleaningEntry = await _cubicleCleaningDailyStatusRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.CleaningDate.Day == localDate.Day
                                                && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.IsSampling == isSampling && x.CubicleId == cubicleId);

            if (cubicleCleaningCleaningEntry.Any())
            {
                var cubicleCleaningStatusList = await GeSubmoduleAllStatusList(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleCleaningSubModule);

                if (cubicleCleaningCleaningEntry.FirstOrDefault().StatusId == cubicleCleaningStatusList.FirstOrDefault(a => a.Status.ToLower() == startedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Cleaned.GetAttribute<DisplayAttribute>().Name;
                }
            }
            var lineClearanceStatusList = await GeSubmoduleAllStatusList(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);

            var lineClearanceRejectedId = lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceRejectedStatus).Id;

            var lineClearanceApprovedStatusEntry = await _lineClearanceTransactionRepository.GetAllListAsync(x => x.CubicleId == cubicleId && x.ClearanceDate.Day == localDate.Day
                                                          && x.ClearanceDate.Month == localDate.Month && x.ClearanceDate.Year == localDate.Year && x.IsSampling == isSampling && x.CubicleId == cubicleId
                                                          && x.StatusId != lineClearanceRejectedId);

            if (lineClearanceApprovedStatusEntry.Any())
            {


                if (lineClearanceApprovedStatusEntry.FirstOrDefault().StatusId == lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceStartedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Cleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else if (lineClearanceApprovedStatusEntry.FirstOrDefault().StatusId == lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceApprovedStatus).Id ||
                    lineClearanceApprovedStatusEntry.FirstOrDefault().StatusId == lineClearanceStatusList.FirstOrDefault(a => a.Status.ToLower() == lineClearanceVerifiedStatus).Id)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.InProcess.GetAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;//ask to show cleaned or uncleaned
                }
            }
            var stageoutCompletedEntry = await _stageOutHeaderRepository.GetAllListAsync(x => x.IsSampling == isSampling && x.CubicleId == cubicleId);
            if (stageoutCompletedEntry.Any())
            {
                var stageoutCompletedStatusId = await GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StageOutSubModule, StageOutCompletedStatus);

                if (stageoutCompletedEntry.FirstOrDefault().StatusId == stageoutCompletedStatusId)
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.Uncleaned.GetAttribute<DisplayAttribute>().Name;
                }
                else
                {
                    cubicleStatus = PMMSEnums.CubicleStatus.InProcess.GetAttribute<DisplayAttribute>().Name;
                }
            }
            return cubicleStatus;
        }
    }
}