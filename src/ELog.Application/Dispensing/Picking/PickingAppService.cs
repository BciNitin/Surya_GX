using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.Picking.Dto;
using ELog.Application.SelectLists.Dto;
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
using System.Net;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.Picking
{
    [PMMSAuthorize]
    public class PickingAppService : ApplicationService, IPickingAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTransferRepository;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContainerDetailRepository;
        private readonly IRepository<LineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingContainerBarcodeRepository;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly string GroupOpenStatus = nameof(CubicleAssignmentGroupStatus.Open).ToLower();
        private readonly string PickingInProgressStatus = nameof(PickingHeaderStatus.InProgress).ToLower();
        private readonly string CubicleAssignmentInProgressStatus = nameof(CubicleAssignementDetailStatus.InProgress).ToLower();
        private readonly string CompletedStatus = nameof(PickingHeaderStatus.Completed).ToLower();
        private readonly string VerifiedStatus = nameof(LineClearanceHeaderStatus.Verified).ToLower();
        private readonly int PickingMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Picking;

        public PickingAppService(
          IHttpContextAccessor httpContextAccessor,
             IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
             IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository,
             IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
             IRepository<Material> materialRepository,
             IRepository<CubicleMaster> cubicleRepository,
             IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTransferRepository,
             IRepository<LocationMaster> locationRepository,
             IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository,
             IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContainerDetailRepository,
             IDispensingAppService dispensingAppService, IRepository<LineClearanceTransaction> lineClearanceTransactionRepository,
             IRepository<GRNDetail> grnDetailRepository,
             IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingContainerBarcodeRepository,
             IRepository<Palletization> palletizationRepository,
             IMasterCommonRepository masterCommonRepository
             )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _materialRepository = materialRepository;
            _cubicleRepository = cubicleRepository;
            _putAwayBinToBinTransferRepository = putAwayBinToBinTransferRepository;
            _locationRepository = locationRepository;
            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
            _materialBatchDispensingContainerDetailRepository = materialBatchDispensingContainerDetailRepository;
            _dispensingAppService = dispensingAppService;
            _grnDetailRepository = grnDetailRepository;
            _grnLabelPrintingContainerBarcodeRepository = grnLabelPrintingContainerBarcodeRepository;
            _palletizationRepository = palletizationRepository;
            _masterCommonRepository = masterCommonRepository;
        }

        /// <summary>
        /// Used for getting cubicle by barcode under dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Picking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> GetCubicleByBarcode(string input)
        {
            return await GetCubicleByBarcodeInternalAsync(input, false);
        }

        /// <summary>
        /// Used for getting cubicle by barcode under sampling
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPicking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> GetCubicleForSamplingAsync(string input)
        {
            return await GetCubicleByBarcodeInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> GetCubicleByBarcodeInternalAsync(string input, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
                //Get all cubicle by barcode
                var allCubicles = from cubicle in _cubicleRepository.GetAll()
                                  where cubicle.IsActive && cubicle.ApprovalStatusId == approvedApprovalStatusId
                                  && cubicle.IsActive && cubicle.CubicleCode.ToLower() == input.ToLower()
                                  select new PickingValidationDto
                                  {
                                      Id = cubicle.Id,
                                      Value = cubicle.CubicleCode,
                                      PlantId = cubicle.PlantId,
                                  };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    allCubicles = allCubicles.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }
                var scanedCubicle = await allCubicles.FirstOrDefaultAsync();
                if (scanedCubicle == null)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
                }
                //Getting all assigned group with verified line clearance under scanned cubicle
                var cubicleWithAssignedGroups = from cubicleHeader in _cubicleAssignmentHeaderRepository.GetAll()
                                                join details in _cubicleAssignmentDetailRepository.GetAll()
                                                on cubicleHeader.Id equals details.CubicleAssignmentHeaderId
                                                join cubicle in _cubicleRepository.GetAll()
                                                on details.CubicleId equals cubicle.Id
                                                where details.CubicleId == scanedCubicle.Id && cubicleHeader.IsSampling == isSampling
                                                select new PickingValidationDto
                                                {
                                                    Id = cubicleHeader.Id,
                                                    Value = cubicle.CubicleCode,
                                                    PlantId = cubicle.PlantId,
                                                    AssignedGroups = null,
                                                    GroupStatusId = cubicleHeader.GroupStatusId
                                                };

                if (cubicleWithAssignedGroups.Any())
                {
                    var groupOpenStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, GroupOpenStatus);
                    var cubicleWithAssignedOpenGroup = await cubicleWithAssignedGroups.Where(a => a.GroupStatusId == groupOpenStatusId).FirstOrDefaultAsync() ?? default;
                    if (cubicleWithAssignedOpenGroup != null)
                    {
                        var lineClearanceCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, VerifiedStatus);
                        var lineClearanceCompletedCubicles = await (from lineClearanceCubicle in _lineClearanceTransactionRepository.GetAll()
                                                                    join detail in _cubicleAssignmentDetailRepository.GetAll()
                                                                    on lineClearanceCubicle.CubicleId equals detail.CubicleId
                                                                    join cubicleAssignHeader in _cubicleAssignmentHeaderRepository.GetAll()
                                                                    on detail.CubicleAssignmentHeaderId equals cubicleAssignHeader.Id
                                                                    where lineClearanceCubicle.CubicleId == scanedCubicle.Id && lineClearanceCubicle.StatusId == lineClearanceCompletedStatus
                                                                    && cubicleAssignHeader.IsSampling == isSampling && lineClearanceCubicle.IsSampling == isSampling && lineClearanceCubicle.GroupId == cubicleAssignHeader.Id
                                                                    select cubicleAssignHeader).ToListAsync() ?? default;

                        if (lineClearanceCompletedCubicles.Any())
                        {
                            var lineClearanceCompletedCubicleAssignedOpenGroups = lineClearanceCompletedCubicles.Select(a => new SelectListDto { Id = a.Id, Value = a.GroupId }).ToList();
                            cubicleWithAssignedOpenGroup.AssignedGroups = lineClearanceCompletedCubicleAssignedOpenGroups.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                        }
                        else
                        {
                            return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleAssignGroupLineClearanceNotDone);
                        }
                        responseDto.ResultObject = cubicleWithAssignedOpenGroup;
                        return responseDto;
                    }
                    else
                    {
                        return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleClosedGroupValidation);
                    }
                }
                else
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNoGroupAssignedValidation);
                }
            }
            return responseDto;
        }

        /// <summary>
        /// Used for validating bin barcode from suggested bins
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<HTTPResponseDto> UpdatePickingDtoByBinBarcode(PickingDto input)
        {
            var responseDto = new HTTPResponseDto();
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var locationQuery = _locationRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.IsActive
            && x.LocationCode.ToLower() == input.LocationBarCode.ToLower()).OrderBy(x => x.LocationCode)
                      .Select(x => new PickingValidationDto { Id = x.Id, Value = x.LocationCode, PlantId = x.PlantId });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                locationQuery = locationQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var scanBinResult = await locationQuery.FirstOrDefaultAsync();
            if (scanBinResult != null)
            {
                var scanBinId = scanBinResult.Id;
                //Get all suggested bins
                var suggestedBins = await GetSuggestedBins(input.MaterialCode, input.SAPBatchNo);

                var validBinsFromSuggestedBins = suggestedBins.Where(a => (int)a.Id == scanBinId)
                    .Select(x => new SelectListDto { Id = (int)x.Id, Value = x.Value }).ToList();
                //Check valid bin scanned
                if (validBinsFromSuggestedBins.Any())
                {
                    responseDto.ResultObject = validBinsFromSuggestedBins;
                    return responseDto;
                }
                else
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.BinIsNotFromSuggestedBinValidation);
                }
            }
            else
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.InActiveAndNotApprovedBinValidation);
            }
        }

        /// <summary>
        /// Used for getting all material codes under selected group
        /// </summary>
        /// <param name="cubicleAssignmentHeaderId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetMaterialCodeByGroupId(int cubicleAssignmentHeaderId)
        {
            var responseDto = new HTTPResponseDto();
            var inprogressDetailStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, CubicleAssignmentInProgressStatus);
            //Get all material code for in progress cubicle assignment
            var cubicleAssignMaterials = await (from cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                on cubicleAssignmentDetail.ProcessOrderMaterialId equals processOrderMaterial.Id
                                                where cubicleAssignmentDetail.CubicleAssignmentHeaderId == cubicleAssignmentHeaderId
                                                && cubicleAssignmentDetail.StatusId == inprogressDetailStatus
                                                orderby processOrderMaterial.ExpiryDate ascending
                                                select new SelectListDto { Id = processOrderMaterial.ItemCode, Value = processOrderMaterial.ItemCode })?.ToListAsync() ?? default;
            cubicleAssignMaterials = cubicleAssignMaterials.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (cubicleAssignMaterials?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderGroup);
            }
            responseDto.ResultObject = cubicleAssignMaterials;
            return responseDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Picking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> UpdateSAPBatchNoByMaterialCodeAsync(PickingDto input)
        {
            return await UpdateSAPBatchNoByMaterialCodeInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPicking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> UpdateSAPBatchNoByMaterialCodeForSamplingAsync(PickingDto input)
        {
            return await UpdateSAPBatchNoByMaterialCodeInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> UpdateSAPBatchNoByMaterialCodeInternalAsync(PickingDto input, bool IsSampling)
        {
            var responseDto = new HTTPResponseDto();
            var inprogressDetailStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, CubicleAssignmentInProgressStatus);
            var cubicleAssignMaterials = await (from cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                on cubicleAssignmentDetail.ProcessOrderMaterialId equals processOrderMaterial.Id
                                                where cubicleAssignmentDetail.CubicleAssignmentHeaderId == input.CubicleAssignmentHeaderId
                                                && processOrderMaterial.ItemCode == input.MaterialCode
                                                && cubicleAssignmentDetail.StatusId == inprogressDetailStatus
                                                orderby processOrderMaterial.CreationTime
                                                select processOrderMaterial.SAPBatchNo).Distinct().ToListAsync() ?? default;

            if (cubicleAssignMaterials?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderGroup);
            }
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            var materialStatus = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.CubicleCode == input.CubicleCode
                                         && x.MaterialCode == input.MaterialCode
                                         && x.GroupCode == input.GroupId
                                         && x.IsSampling == IsSampling
                                         && x.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType)
                                             .Select(x => new { x.StatusId, x.Id }).FirstOrDefaultAsync();
            var StatusId = 0;
            var PickingMaterialId = 0;
            if (materialStatus != null)
            {
                StatusId = materialStatus.StatusId;
                PickingMaterialId = materialStatus.Id;
            }
            responseDto.ResultObject = new { Status = StatusId != completedStatusId ? "In Progress" : "Completed", PickingMaterialId, Selectlist = cubicleAssignMaterials.Select(x => new SelectListDto { Id = x, Value = x }) };
            return responseDto;
        }

        /// <summary>
        /// Used when selecting material code to update picking details on UI for dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Picking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PickingDto> UpdatePickingDetailAsync(PickingDto input)
        {
            return await UpdatePickingDetailInternalAsync(input, false);
        }

        /// <summary>
        /// Used when selecting material code to update picking details on UI for sampling
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPicking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PickingDto> UpdatePickingDetailForSamplingAsync(PickingDto input)
        {
            return await UpdatePickingDetailInternalAsync(input, true);
        }

        private async Task<PickingDto> UpdatePickingDetailInternalAsync(PickingDto input, bool IsSampling)
        {
            var inprogressDetailStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, CubicleAssignmentInProgressStatus);
            var requiredQuantitySum = await (from cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                             join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                             on cubicleAssignmentDetail.ProcessOrderMaterialId equals processOrderMaterial.Id
                                             where cubicleAssignmentDetail.CubicleAssignmentHeaderId == input.CubicleAssignmentHeaderId
                                             && processOrderMaterial.ItemCode == input.MaterialCode
                                             && processOrderMaterial.SAPBatchNo == input.SAPBatchNo
                                             && cubicleAssignmentDetail.StatusId == inprogressDetailStatus
                                             select processOrderMaterial.OrderQuantity
                                               ).SumAsync() ?? default;

            input.SuggestedBins = await GetSuggestedBins(input.MaterialCode, input.SAPBatchNo);
            //Picking header already exist

            var pickingHeader = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
            && x.CubicleCode == input.CubicleCode
            && x.MaterialCode == input.MaterialCode
            && x.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
            && x.SAPBatchNumber == input.SAPBatchNo
            && x.IsSampling == IsSampling).FirstOrDefaultAsync();
            if (pickingHeader != null)
            {
                input.Id = pickingHeader.Id;
                var pickingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
                input.IsCompletePickingAllowed = pickingHeader.StatusId != pickingCompletedStatus;
                input.ContainerCount = await _materialBatchDispensingContainerDetailRepository.GetAll()
                                             .Where(x => x.MaterialBatchDispensingHeaderId == pickingHeader.Id).CountAsync();
                input.Quantity = await _materialBatchDispensingContainerDetailRepository.GetAll()
                                       .Where(x => x.MaterialBatchDispensingHeaderId == pickingHeader.Id).SumAsync(x => x.Quantity);
            }
            else
            {
                input.ContainerCount = 0;
                input.Quantity = 0;
                input.IsCompletePickingAllowed = true;
            }
            input.RequiredQty = requiredQuantitySum;
            return input;
        }

        private async Task<List<SelectListDto>> GetSuggestedBins(string materialCode, string sapBatchNumber)
        {
            var lstPalletId = await (from material in _materialRepository.GetAll()
                                     join pallet in _palletizationRepository.GetAll()
                                     on material.Id equals pallet.MaterialId
                                     join grnDetail in _grnDetailRepository.GetAll()
                                     on material.Id equals grnDetail.MaterialId
                                     where material.ItemCode == materialCode && !pallet.IsUnloaded
                                     && grnDetail.SAPBatchNumber == sapBatchNumber
                                     select pallet.PalletId).Distinct().ToListAsync();

            var putAwayBinQuery = from putAwayBinToBinTransfer in _putAwayBinToBinTransferRepository.GetAll()
                                  join materials in _materialRepository.GetAll() on
                                  putAwayBinToBinTransfer.MaterialId equals materials.Id into putawayMaterials
                                  from materials in putawayMaterials.DefaultIfEmpty()
                                  join location in _locationRepository.GetAll() on
                                 putAwayBinToBinTransfer.LocationId equals location.Id
                                  join grnDetail in _grnDetailRepository.GetAll()
                                      on materials.Id equals grnDetail.MaterialId into grnDetailMaterials
                                  from grnDetail in grnDetailMaterials.DefaultIfEmpty()
                                  select new
                                  {
                                      LocationId = location.Id,
                                      LocationCode = location.LocationCode,
                                      PalletId = putAwayBinToBinTransfer.PalletId,
                                      ItemCode = materials.ItemCode,
                                      IsUnloaded = putAwayBinToBinTransfer.IsUnloaded,
                                      grnDetail.SAPBatchNumber
                                  };

            if (lstPalletId?.Count() > 0)
            {
                putAwayBinQuery = putAwayBinQuery.Where(x => (x.ItemCode == materialCode && x.SAPBatchNumber == sapBatchNumber && !x.IsUnloaded) || lstPalletId.Contains(x.PalletId));
            }
            else
            {
                putAwayBinQuery = putAwayBinQuery.Where(x => x.ItemCode == materialCode && x.SAPBatchNumber == sapBatchNumber && !x.IsUnloaded);
            }
            var putAwayResult = await putAwayBinQuery.Select(x => new SelectListDto { Id = x.LocationId, Value = x.LocationCode }).ToListAsync();
            if (putAwayResult != null)
            {
                return putAwayResult.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Picking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> SaveMaterialContainerFromBinAsync(PickingDto input)
        {
            return await SaveMaterialContainerFromBinInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPicking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> SaveMaterialContainerFromBinForSamplingAsync(PickingDto input)
        {
            return await SaveMaterialContainerFromBinInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> SaveMaterialContainerFromBinInternalAsync(PickingDto input, bool IsSampling)
        {
            var responseDto = new HTTPResponseDto();
            var materialLabelContainer = await (
                from grnContainerLabels in _grnLabelPrintingContainerBarcodeRepository.GetAllIncluding(x => x.Palletizations, x => x.PutAwayBinToBinTransfers)
                join grnDetail in _grnDetailRepository.GetAll()
                on grnContainerLabels.GRNDetailId equals grnDetail.Id
                join materials in _materialRepository.GetAll()
                on grnDetail.MaterialId equals materials.Id
                where grnContainerLabels.MaterialLabelContainerBarCode == input.MaterialContainerBarCode
                && materials.ItemCode == input.MaterialCode
                && grnContainerLabels.BalanceQuantity > 0
                && grnDetail.SAPBatchNumber == input.SAPBatchNo
                select new
                {
                    ContainerLabelId = grnContainerLabels.Id,
                    SAPBatchNo = grnDetail.SAPBatchNumber,
                    Quantity = grnContainerLabels.Quantity,
                    LstPalletId = grnContainerLabels.Palletizations.Select(x => x.Id),
                    LstPutawayBin = grnContainerLabels.PutAwayBinToBinTransfers.Select(x => x.Id),
                    PurchaseOrdermaterialId = grnDetail.MaterialId
                }).FirstOrDefaultAsync();
            if (materialLabelContainer == null)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFound);
            }
            if (materialLabelContainer.LstPalletId?.Count() > 0)
            {
                var matchedPalletId = await (from pallet in _palletizationRepository.GetAll()
                                             join putAway in _putAwayBinToBinTransferRepository.GetAll()
                                             on pallet.PalletId equals putAway.PalletId
                                             where materialLabelContainer.LstPalletId.Contains(pallet.Id)
                                             && putAway.LocationId == input.LocationId
                                             select putAway.PalletId).FirstOrDefaultAsync();
                if (matchedPalletId == 0)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFoundForBin);
                }
                var matchedPutAwayBin = await (from pallet in _palletizationRepository.GetAll()
                                               join putAway in _putAwayBinToBinTransferRepository.GetAll()
                                               on pallet.PalletId equals putAway.PalletId
                                               where pallet.PalletId == matchedPalletId
                                               && putAway.LocationId == input.LocationId
                                               select putAway.Id).ToListAsync();
                if (matchedPutAwayBin?.Count() == 1)
                {
                    await _putAwayBinToBinTransferRepository.DeleteAsync(matchedPutAwayBin.First());
                }
                foreach (var palletId in materialLabelContainer.LstPalletId)
                {
                    await _palletizationRepository.DeleteAsync(palletId);
                }
            }
            if (materialLabelContainer.LstPutawayBin?.Count() > 0)
            {
                if (!await _putAwayBinToBinTransferRepository.GetAll().AnyAsync(x => materialLabelContainer.LstPutawayBin.Contains(x.Id)
                 && x.LocationId == input.LocationId))
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFoundForBin);
                }
                foreach (var putAwayId in materialLabelContainer.LstPutawayBin)
                {
                    await _putAwayBinToBinTransferRepository.DeleteAsync(putAwayId);
                }
            }
            int pickingHeaderId = await GetPickingHeaderId(input, IsSampling);

            var alreadyExistPickedContainerId = await (from containerDetail in _materialBatchDispensingContainerDetailRepository.GetAll()
                                                       where containerDetail.MaterialBatchDispensingHeaderId == pickingHeaderId
                                                       && containerDetail.ContainerBarCode == input.MaterialContainerBarCode
                                                       select containerDetail.Id).FirstOrDefaultAsync();
            if (alreadyExistPickedContainerId > 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
            }

            var materialBatchContainerDetail = new MaterialBatchDispensingContainerDetail();
            materialBatchContainerDetail.MaterialBatchDispensingHeaderId = pickingHeaderId;
            materialBatchContainerDetail.SAPBatchNumber = materialLabelContainer.SAPBatchNo;
            materialBatchContainerDetail.ContainerBarCode = input.MaterialContainerBarCode;
            materialBatchContainerDetail.ContainerPickingTime = DateTime.Now;
            materialBatchContainerDetail.Quantity = materialLabelContainer.Quantity;
            await _materialBatchDispensingContainerDetailRepository.InsertAndGetIdAsync(materialBatchContainerDetail);
            await CurrentUnitOfWork.SaveChangesAsync();

            input.SuggestedBins = await GetSuggestedBins(input.MaterialCode, input.SAPBatchNo);
            input.ContainerCount = await _materialBatchDispensingContainerDetailRepository.GetAll()
                .Where(x => x.MaterialBatchDispensingHeaderId == pickingHeaderId).CountAsync();
            input.Quantity = await _materialBatchDispensingContainerDetailRepository.GetAll()
                .Where(x => x.MaterialBatchDispensingHeaderId == pickingHeaderId).SumAsync(x => x.Quantity);
            input.Id = pickingHeaderId;
            if (input.Quantity >= input.RequiredQty)
            {
                var pickingHeader = await _materialBatchDispensingHeaderRepository.GetAsync(input.Id);
                pickingHeader.BatchPickingStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
                await _materialBatchDispensingHeaderRepository.UpdateAsync(pickingHeader);
            }

            responseDto.ResultObject = input;
            return responseDto;
        }

        private HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, String ValidationError)
        {
            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
            responseDto.Error = ValidationError;
            return responseDto;
        }

        private async Task<int> GetPickingHeaderId(PickingDto input, bool IsSampling)
        {
            //Picking header already exist
            var pickingHeaderId = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
            && x.CubicleCode == input.CubicleCode
            && x.MaterialCode == input.MaterialCode
            && x.IsSampling == IsSampling
            && x.SAPBatchNumber == input.SAPBatchNo
            && x.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType).Select(x => x.Id).FirstOrDefaultAsync();
            if (pickingHeaderId == 0)
            {
                var pickingHeader = new MaterialBatchDispensingHeader();
                pickingHeader.CubicleCode = input.CubicleCode;
                pickingHeader.GroupCode = input.GroupId;
                pickingHeader.MaterialCode = input.MaterialCode;
                pickingHeader.SAPBatchNumber = input.SAPBatchNo;
                pickingHeader.PickingTime = DateTime.Now;
                pickingHeader.MaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Picking;
                var inProgressStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, PickingInProgressStatus);
                pickingHeader.StatusId = inProgressStatusId;
                pickingHeader.BatchPickingStatusId = inProgressStatusId;
                pickingHeader.IsSampling = IsSampling;
                pickingHeader.TenantId = AbpSession.TenantId;
                pickingHeaderId = await _materialBatchDispensingHeaderRepository.InsertAndGetIdAsync(pickingHeader);
            }

            return pickingHeaderId;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Picking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> CompletePickingAsync(PickingDto input)
        {
            return await CompletePickingInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPicking_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<HTTPResponseDto> CompletePickingForSamplingAsync(PickingDto input)
        {
            return await CompletePickingInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> CompletePickingInternalAsync(PickingDto input, bool IsSampling)
        {
            var cartesianJoinBatch = input.SAPBatchNumbers.Select(x => new PickingCompleteCartesianDto { CubicleCode = input.CubicleCode, MaterialCode = input.MaterialCode, SAPBatchNumber = x });
            var responseDto = new HTTPResponseDto();
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            var pickingHeaders = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
                                         && x.CubicleCode == input.CubicleCode
                                         && x.MaterialCode == input.MaterialCode
                                         && x.IsSampling == IsSampling
                                         && x.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType)
                                        .ToListAsync();

            var SAPBatchPickingPendingEntry = cartesianJoinBatch.Where(a => !pickingHeaders.Any(b => a.CubicleCode == b.CubicleCode
                                                                                                            && a.MaterialCode == b.MaterialCode
                                                                                                            && a.SAPBatchNumber == b.SAPBatchNumber));
            if (!IsSampling && (SAPBatchPickingPendingEntry?.Count() > 0 || pickingHeaders.Any(x => x.BatchPickingStatusId != completedStatusId)))
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerUnderpickingNotAllowed);
            }

            pickingHeaders.ForEach(x => x.StatusId = completedStatusId);
            await _masterCommonRepository.BulkUpdateMaterialBatchDispensingHeader(pickingHeaders);

            input.IsCompletePickingAllowed = false;
            responseDto.ResultObject = input;
            return responseDto;
        }
    }
}