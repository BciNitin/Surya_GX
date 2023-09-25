using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.StageOut.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.StageOut
{
    [PMMSAuthorize]
    public class StageOutAppService : ApplicationService, IStageOutAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<EquipmentAssignment> _equipmentAssignmentRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _materialLabelContainerRepository;
        private readonly IRepository<StageOutHeader> _stageOutHeaderRepository;
        private readonly IRepository<StageOutDetail> _stageOutDetailRepository;
        private readonly IRepository<CubicleCleaningDailyStatus> _cubicleCleaningDailyStatusRepository;

        private readonly string StageOutInProgressStatus = nameof(StageOutStatus.InProgress).ToLower();
        private readonly string StageOutCompletedStatus = nameof(StageOutStatus.Completed).ToLower();
        private readonly string DispensingCompletedStatus = nameof(DispensingHeaderStatus.Completed).ToLower();
        private readonly string SamplingCompletedStatus = nameof(SamplingHeaderStatus.Completed).ToLower();
        private readonly string CubicleUncleanedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Uncleaned).ToLower();
        private readonly string CubicleVerifiedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Verified).ToLower();

        #endregion fields

        #region constructor

        public StageOutAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<DispensingHeader> dispensingHeaderRepository,
             IRepository<DispensingDetail> dispensingDetailRepository,
             IRepository<EquipmentAssignment> equipmentAssignmentRepository,
            IRepository<CubicleMaster> cubicleRepository,
            IDispensingAppService dispensingAppService,
            IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository,
            IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
            IRepository<InspectionLot> inspectionLotRepository,
            IRepository<GRNMaterialLabelPrintingContainerBarcode> materialLabelContainerRepository,
            IRepository<StageOutHeader> stageOutHeaderRepository,
            IRepository<StageOutDetail> stageOutDetailRepository,
            IRepository<CubicleCleaningDailyStatus> cubicleCleaningDailyStatusRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;

            _httpContextAccessor = httpContextAccessor;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _dispensingDetailRepository = dispensingDetailRepository;
            _equipmentAssignmentRepository = equipmentAssignmentRepository;
            _cubicleRepository = cubicleRepository;
            _dispensingAppService = dispensingAppService;
            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _inspectionLotRepository = inspectionLotRepository;
            _materialLabelContainerRepository = materialLabelContainerRepository;
            _stageOutHeaderRepository = stageOutHeaderRepository;
            _stageOutDetailRepository = stageOutDetailRepository;
            _cubicleCleaningDailyStatusRepository = cubicleCleaningDailyStatusRepository;
        }

        #endregion constructor

        #region public

        public async Task<HTTPResponseDto> GetGroupIdByCubicleBarcodeAsync(string input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                CubicleStageOutInternalDto scannedEquipment = await GetCubiclesAsync(input);
                if (scannedEquipment == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
                }
                else
                {
                    var dispensingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.DispensingSubModule, DispensingCompletedStatus);

                    var lstGroups = await (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                           join equipmentAssignment in _equipmentAssignmentRepository.GetAll()
                                           on dispensingHeader.RLAFId equals equipmentAssignment.EquipmentId
                                           join cubicleAssignment in _cubicleAssignmentHeaderRepository.GetAll()
                                           on equipmentAssignment.CubicleAssignmentHeaderId equals cubicleAssignment.Id
                                           where equipmentAssignment.Cubicleid == scannedEquipment.CubicleId
                                           && dispensingHeader.StatusId == dispensingCompletedStatusId
                                           && !dispensingHeader.IsSampling
                                           && !equipmentAssignment.IsSampling
                                           && !cubicleAssignment.IsSampling
                                           select new SelectListDto { Id = equipmentAssignment.CubicleAssignmentHeaderId, Value = cubicleAssignment.GroupId })
                                           .ToListAsync();
                    if (lstGroups?.Count() > 0)
                    {
                        scannedEquipment.lstGroupOrInspectionSelectList = lstGroups.GroupBy(x => x.Id)
                            .Select(x => new SelectListDto { Id = x.First().Id, Value = x.First().Value }).ToList();
                        responseDto.ResultObject = scannedEquipment;
                    }
                    else
                    {
                        //No Group Found.
                        return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoGroupFoundUnderCubicle);
                    }
                }
            }
            else
            {
                //Cubicle code not valid.
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
            }

            return responseDto;
        }

        public async Task<HTTPResponseDto> GetGroupIdByCubicleBarcodeForSamplingAsync(string input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                CubicleStageOutInternalDto scannedEquipment = await GetCubiclesAsync(input);
                if (scannedEquipment == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
                }
                else
                {
                    var samplingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);

                    var lstGroups = await (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                           join equipmentAssignment in _equipmentAssignmentRepository.GetAll()
                                           on dispensingHeader.RLAFId equals equipmentAssignment.EquipmentId
                                           join cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                           on equipmentAssignment.CubicleAssignmentHeaderId equals cubicleAssignmentDetail.CubicleAssignmentHeaderId
                                           join cubicleAssignment in _cubicleAssignmentHeaderRepository.GetAll()
                                             on equipmentAssignment.CubicleAssignmentHeaderId equals cubicleAssignment.Id
                                           join inspectionLot in _inspectionLotRepository.GetAll()
                                                       on cubicleAssignmentDetail.InspectionLotId equals inspectionLot.Id
                                           where equipmentAssignment.Cubicleid == scannedEquipment.CubicleId
                                           && dispensingHeader.StatusId == samplingCompletedStatusId
                                           && dispensingHeader.IsSampling
                                           && equipmentAssignment.IsSampling
                                           && cubicleAssignment.IsSampling
                                           select new SelectListDto { Id = inspectionLot.Id, Value = inspectionLot.InspectionLotNumber })
                                           .ToListAsync();
                    if (lstGroups?.Count() > 0)
                    {
                        scannedEquipment.lstGroupOrInspectionSelectList = lstGroups.GroupBy(x => x.Id)
                            .Select(x => new SelectListDto { Id = x.First().Id, Value = x.First().Value }).ToList();
                        responseDto.ResultObject = scannedEquipment;
                    }
                    else
                    {
                        //No Group Found.
                        return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoInspectionLotFoundUnderCubicle);
                    }
                }
            }
            else
            {
                //Cubicle code not valid.
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
            }
            return responseDto;
        }

        public async Task<HTTPResponseDto> GetMaterialCodeByGroupIdAsync(int cubicleAssignmentHeaderId)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            var dispensingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.DispensingSubModule, DispensingCompletedStatus);

            var lstMaterials = await (
                                      from cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                      join dispensingHeader in _dispensingHeaderRepository.GetAll()
                                      on cubicleAssignmentDetail.ProcessOrderId equals dispensingHeader.ProcessOrderId
                                      where
                                      !dispensingHeader.IsSampling
                                      && dispensingHeader.StatusId == dispensingCompletedStatusId
                                      && cubicleAssignmentDetail.CubicleAssignmentHeaderId == cubicleAssignmentHeaderId
                                      select new SelectListDto { Id = dispensingHeader.MaterialCodeId, Value = dispensingHeader.MaterialCodeId })
                    .ToListAsync();
            if (lstMaterials?.Count() > 0)
            {
                responseDto.ResultObject = lstMaterials.GroupBy(x => x.Id).Select(x => new SelectListDto
                {
                    Id = x.First().Id,
                    Value = x.First().Value
                });
            }
            else
            {
                //No Material Found
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderGroup);
            }
            return responseDto;
        }

        public async Task<HTTPResponseDto> GetMaterialCodeByGroupIdForSamplingAsync(int inspectionLotId)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            var samplingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
            var lstMaterials = await (
                                      from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                      where
                                       dispensingHeader.IsSampling
                                       && dispensingHeader.StatusId == samplingCompletedStatusId
                                       && dispensingHeader.InspectionLotId == inspectionLotId
                                      select new SelectListDto { Id = dispensingHeader.MaterialCodeId, Value = dispensingHeader.MaterialCodeId })
                   .ToListAsync();
            if (lstMaterials?.Count() > 0)
            {
                responseDto.ResultObject = lstMaterials.GroupBy(x => x.Id).Select(x => new SelectListDto
                {
                    Id = x.First().Id,
                    Value = x.First().Value
                });
            }
            else
            {
                //No Material Found
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderInspectionLotNo);
            }
            return responseDto;
        }

        public async Task<HTTPResponseDto> UpdateStageOutSAPBatchNoMaterialCodeAsync(StagingOutDto input)
        {
            var responseDto = await UpdateStageOutSAPBatchNoMaterialCodeInternalAsync(input, false);
            var stageOutHeaderDetail = _stageOutHeaderRepository.GetAll().Where(x =>
            !x.IsSampling
            && x.MaterialCode == input.MaterialCode
            && x.CubicleId == input.CubicleId
            && x.GroupId == input.GroupId).Select(x => Tuple.Create(x.Id, x.StatusId)).FirstOrDefault();
            var stageOutCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StageOutSubModule, StageOutCompletedStatus);

            string StageOutStatus = "Not Started";
            if (stageOutHeaderDetail != null)
            {
                StageOutStatus = stageOutHeaderDetail.Item2 == stageOutCompletedStatusId ? "Completed" : "In Progress";
                input.Id = stageOutHeaderDetail.Item1;
            }
            responseDto.ResultObject = new { Status = StageOutStatus, StagingOutDto = input, SAPBatchSelectList = responseDto.ResultObject };
            return responseDto;
        }

        public async Task<HTTPResponseDto> UpdateStageOutSAPBatchNoMaterialCodeForSamplingAsync(StagingOutDto input)
        {
            var responseDto = await UpdateStageOutSAPBatchNoMaterialCodeInternalAsync(input, true);
            var stageOutHeaderDetail = await _stageOutHeaderRepository.GetAll().Where(x =>
             x.IsSampling
             && x.MaterialCode == input.MaterialCode
             && x.CubicleId == input.CubicleId
             && x.InspectionLotId == input.InspectionLotId).Select(x => Tuple.Create(x.Id, x.StatusId)).FirstOrDefaultAsync();
            var stageOutCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.StageOutSubModule, StageOutCompletedStatus);

            string StageOutStatus = "Not Started";
            if (stageOutHeaderDetail != null)
            {
                StageOutStatus = stageOutHeaderDetail.Item2 == stageOutCompletedStatusId ? "Completed" : "In Progress";
                input.Id = stageOutHeaderDetail.Item1;
            }
            responseDto.ResultObject = new { Status = StageOutStatus, StagingOutDto = input, SAPBatchSelectList = responseDto.ResultObject };
            return responseDto;
        }

        public async Task<HTTPResponseDto> UpdateMaterialContainerByBarcodeAsync(StagingOutDto input)
        {
            return await UpdateMaterialContainerByBarcodeInternalAsync(input, false);
        }

        public async Task<StagingOutDto> UpdateStageOutContainerCountAndBalanceQuantity(StagingOutDto input)
        {
            await UpdateContainerQuantityAndSum(input);
            return input;
        }

        public async Task<HTTPResponseDto> UpdateMaterialContainerByBarcodeForSamplingAsync(StagingOutDto input)
        {
            return await UpdateMaterialContainerByBarcodeInternalAsync(input, true);
        }

        public async Task<HTTPResponseDto> CompleteStageOutAsync(StagingOutDto input)
        {
            return await CompleteStagedOutInternalAsync(input, false);
        }

        public async Task<HTTPResponseDto> CompleteStageOutForSamplingAsync(StagingOutDto input)
        {
            return await CompleteStagedOutInternalAsync(input, true);
        }

        #endregion public

        #region private

        private async Task<CubicleStageOutInternalDto> GetCubiclesAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            //Get all Cubicle by input
            var allCubicle = from cubicle in _cubicleRepository.GetAll()
                             where cubicle.IsActive && cubicle.ApprovalStatusId == approvedApprovalStatusId &&
                                   cubicle.CubicleCode.ToLower() == input.ToLower()
                             select new CubicleStageOutInternalDto
                             {
                                 CubicleId = cubicle.Id,
                                 CubicleCode = cubicle.CubicleCode,
                                 PlantId = cubicle.PlantId,
                             };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                allCubicle = allCubicle.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await allCubicle.FirstOrDefaultAsync();
        }

        private async Task<HTTPResponseDto> UpdateStageOutSAPBatchNoMaterialCodeInternalAsync(StagingOutDto input, bool isSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();

            List<Tuple<string, string>> lstSAPBatchNo = await GetAllSAPBatchesWithContainer(input, isSampling);
            if (lstSAPBatchNo?.Count() > 0)
            {
                responseDto.ResultObject = lstSAPBatchNo.GroupBy(x => x.Item1).Select(x => new SelectListDto { Id = x.First().Item1, Value = x.First().Item1 });
            }
            else
            {
                //No loose containers for this material
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoLooseContainerFound);
            }

            return responseDto;
        }

        private async Task UpdateContainerQuantityAndSum(StagingOutDto input)
        {
            //Update container count
            var balanceQuantitites = await _stageOutDetailRepository.GetAll().Where(x => x.StageOutHeaderId == input.Id && x.SAPBatchNo == input.SAPBatchNo).Select(x => x.BalanceQuantity).ToListAsync();
            if (balanceQuantitites?.Count() > 0)
            {
                input.ContainerCount = balanceQuantitites.Count;
                //Update Quantity
                input.Quantity = balanceQuantitites.Sum();
            }
        }

        private async Task<HTTPResponseDto> UpdateMaterialContainerByBarcodeInternalAsync(StagingOutDto input, bool IsSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            //Check if container is already scanned
            var isStageOutExist = await _stageOutDetailRepository.GetAll().Where(x => x.SAPBatchNo == input.SAPBatchNo
                    && x.MaterialContainerBarcode == input.MaterialContainerBarcode).AnyAsync();
            if (isStageOutExist)
            {
                //Material container already scanned
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
            }

            //Check if container barcode is from dispensingdetails only of selected SAP batch only
            Tuple<int, int, float?> dispensingDetail = null;
            if (IsSampling)
            {
                dispensingDetail = await (
              from dispensingHeader in _dispensingHeaderRepository.GetAll()
              join dispenseDetail in _dispensingDetailRepository.GetAll()
              on dispensingHeader.Id equals dispenseDetail.DispensingHeaderId

              join materialContainer in _materialLabelContainerRepository.GetAll()
                                        on dispenseDetail.ContainerMaterialBarcode equals materialContainer.MaterialLabelContainerBarCode
              where dispensingHeader.InspectionLotId == input.InspectionLotId
                     && dispensingHeader.MaterialCodeId == input.MaterialCode
                     && dispenseDetail.SAPBatchNumber == input.SAPBatchNo
                     && dispenseDetail.ContainerMaterialBarcode == input.MaterialContainerBarcode
                     && materialContainer.BalanceQuantity > 0
                     && dispensingHeader.IsSampling == IsSampling
              select Tuple.Create(dispenseDetail.Id, materialContainer.Id, materialContainer.BalanceQuantity)).FirstOrDefaultAsync();
            }
            else
            {
                dispensingDetail = await (
              from dispensingHeader in _dispensingHeaderRepository.GetAll()
              join dispenseDetail in _dispensingDetailRepository.GetAll()
              on dispensingHeader.Id equals dispenseDetail.DispensingHeaderId
              join cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
              on dispensingHeader.ProcessOrderId equals cubicleAssignmentDetail.ProcessOrderId
              join materialContainer in _materialLabelContainerRepository.GetAll()
                                        on dispenseDetail.ContainerMaterialBarcode equals materialContainer.MaterialLabelContainerBarCode
              where cubicleAssignmentDetail.CubicleAssignmentHeaderId == input.CubicleAssignmentHeaderId
                     && dispensingHeader.MaterialCodeId == input.MaterialCode
                     && dispenseDetail.SAPBatchNumber == input.SAPBatchNo
                     && dispenseDetail.ContainerMaterialBarcode == input.MaterialContainerBarcode
                     && materialContainer.BalanceQuantity > 0
                     && dispensingHeader.IsSampling == IsSampling
              select Tuple.Create(dispenseDetail.Id, materialContainer.Id, materialContainer.BalanceQuantity)).FirstOrDefaultAsync();
            }

            if (dispensingDetail == null)
            {
                //Material container not found in staged containers
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoLooseContainerFoundByBarcode);
            }

            //Check if stageout header exist for material if not insert in header.Insert to stageoutdetails
            StageOutDetail stageOutDetail = new StageOutDetail();
            stageOutDetail.BalanceQuantity = dispensingDetail.Item3.GetValueOrDefault();
            stageOutDetail.MaterialContainerBarcode = input.MaterialContainerBarcode;
            stageOutDetail.SAPBatchNo = input.SAPBatchNo;

            if (input.Id > 0)
            {
                stageOutDetail.StageOutHeaderId = input.Id;
                await _stageOutDetailRepository.InsertAsync(stageOutDetail);
            }
            else
            {
                var stageOutInProgressStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StageOutSubModule, StageOutInProgressStatus);

                StageOutHeader header = new StageOutHeader();
                header.CubicleId = input.CubicleId.GetValueOrDefault();
                if (IsSampling)
                {
                    header.InspectionLotId = input.InspectionLotId;
                }
                else
                {
                    header.GroupId = input.GroupId;
                }

                header.IsSampling = IsSampling;
                header.MaterialCode = input.MaterialCode;

                header.StatusId = stageOutInProgressStatusId;
                header.StageOutDetails = new List<StageOutDetail>();
                header.StageOutDetails.Add(stageOutDetail);

                input.Id = await _stageOutHeaderRepository.InsertAndGetIdAsync(header);
            }

            var materialLabelContainer = await _materialLabelContainerRepository.GetAsync(dispensingDetail.Item2);
            materialLabelContainer.IsLoosedContainer = true;

            await _materialLabelContainerRepository.UpdateAsync(materialLabelContainer);

            await CurrentUnitOfWork.SaveChangesAsync();

            await UpdateContainerQuantityAndSum(input);
            responseDto.ResultObject = input;
            return responseDto;
        }

        private async Task<List<Tuple<string, string>>> GetAllSAPBatchesWithContainer(StagingOutDto input, bool isSampling)
        {
            if (isSampling)
            {
                return await (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                              join dispensingDetail in _dispensingDetailRepository.GetAll()
                              on dispensingHeader.Id equals dispensingDetail.DispensingHeaderId
                              join materialContainer in _materialLabelContainerRepository.GetAll()
                              on dispensingDetail.ContainerMaterialBarcode equals materialContainer.MaterialLabelContainerBarCode
                              where dispensingHeader.MaterialCodeId == input.MaterialCode
                              && dispensingHeader.InspectionLotId == input.InspectionLotId
                              && materialContainer.BalanceQuantity > 0
                              && dispensingHeader.IsSampling == isSampling
                              select Tuple.Create(dispensingDetail.SAPBatchNumber, dispensingDetail.ContainerMaterialBarcode))
                    .ToListAsync();
            }
            else
            {
                return await (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                              join dispensingDetail in _dispensingDetailRepository.GetAll()
                              on dispensingHeader.Id equals dispensingDetail.DispensingHeaderId
                              join materialContainer in _materialLabelContainerRepository.GetAll()
                              on dispensingDetail.ContainerMaterialBarcode equals materialContainer.MaterialLabelContainerBarCode
                              join cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                              on dispensingHeader.ProcessOrderId equals cubicleAssignmentDetail.ProcessOrderId
                              where dispensingHeader.MaterialCodeId == input.MaterialCode
                              && cubicleAssignmentDetail.CubicleAssignmentHeaderId == input.CubicleAssignmentHeaderId
                              && materialContainer.BalanceQuantity > 0
                              && dispensingHeader.IsSampling == isSampling
                              select Tuple.Create(dispensingDetail.SAPBatchNumber, dispensingDetail.ContainerMaterialBarcode))
                    .ToListAsync();
            }
        }

        private async Task<HTTPResponseDto> CompleteStagedOutInternalAsync(StagingOutDto input, bool IsSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            List<Tuple<string, string>> lstAllDispensedSAPBatchNoWithContainer = await GetAllSAPBatchesWithContainer(input, IsSampling);
            if (lstAllDispensedSAPBatchNoWithContainer?.Count() > 0)
            {
                var allContainers = lstAllDispensedSAPBatchNoWithContainer.Select(x => x.Item2).Distinct();
                var allExistingContainers = await _stageOutDetailRepository.GetAll().
                    Where(x => x.StageOutHeaderId == input.Id).Select(x => x.MaterialContainerBarcode).Distinct().ToListAsync();
                if (allContainers.Except(allExistingContainers).Any())
                {
                    //Cannot complete some containers are pending
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.StageOutCannotCompleted);
                }
                else
                {
                    //Update completed status
                    var stageOutCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StageOutSubModule, StageOutCompletedStatus);

                    var stageOutHeader = await _stageOutHeaderRepository.GetAsync(input.Id);
                    stageOutHeader.StatusId = stageOutCompletedStatusId;
                    await _stageOutHeaderRepository.UpdateAsync(stageOutHeader);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    //Mark cubicle unclean
                    var localDate = DateTime.Now;
                    var IsAnyMaterialStageOutPending = await _stageOutHeaderRepository.GetAll().AnyAsync(x => x.IsSampling == IsSampling
                                                                                                             && x.CubicleId == input.CubicleId
                                                                                                             && x.GroupId == input.GroupId
                                                                                                             && x.StatusId != stageOutCompletedStatusId);
                    if (!IsAnyMaterialStageOutPending)
                    {
                        var CubicleCleaningUncleanedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
                                              PMMSConsts.CubicleCleaningSubModule, CubicleUncleanedStatus);
                        var CubicleCleaningVerifiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
                            PMMSConsts.CubicleCleaningSubModule, CubicleVerifiedStatus);

                        var dailyCubicleCleaningStatus = await _cubicleCleaningDailyStatusRepository.GetAll().Where(x => x.CubicleId == stageOutHeader.CubicleId
                        && x.CleaningDate.Day == localDate.Day
                        && x.CleaningDate.Month == localDate.Month
                        && x.CleaningDate.Year == localDate.Year
                        && x.StatusId == CubicleCleaningVerifiedStatusId
                        && x.IsSampling == IsSampling).FirstOrDefaultAsync();
                        if (dailyCubicleCleaningStatus != null)
                        {
                            dailyCubicleCleaningStatus.StatusId = CubicleCleaningUncleanedStatusId;
                            await _cubicleCleaningDailyStatusRepository.UpdateAsync(dailyCubicleCleaningStatus);
                        }
                    }
                }
            }
            else
            {
                //Cannot complete (No containers to be completed)
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoLooseContainerForCompletion);
            }
            responseDto.ResultObject = input;
            return responseDto;
        }

        #endregion private
    }
}