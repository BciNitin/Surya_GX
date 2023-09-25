using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.ReturnToVendor.Dto;
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

namespace ELog.Application.Dispensing.ReturnToVendor
{
    [PMMSAuthorize]
    public class ReturnToVendorAppService : ApplicationService, IReturnToVendorAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<SAPReturntoMaterial> _sapReturnaToMaterialRepository;
        private readonly IRepository<SAPQualityControlDetail> _sapQcDetailRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingContainerBarcodeRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<ReturnToVendorHeader> _returnToVendorHeaderRepository;
        private readonly IRepository<ReturnToVendorDetail> _returnToVendorDetailRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly string notPostedStatus = nameof(ReturnToVendorStatus.NotPosted).ToLower();
        private readonly string postedStatus = nameof(ReturnToVendorStatus.Posted).ToLower();

        #endregion fields

        #region constructor

        public ReturnToVendorAppService(IHttpContextAccessor httpContextAccessor, IRepository<SAPReturntoMaterial> sapReturnaToMaterialRepository,
            IRepository<SAPQualityControlDetail> sapQcDetailRepository, IDispensingAppService dispensingAppService, IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingContainerBarcodeRepository,
             IRepository<GRNDetail> grnDetailRepository, IRepository<Material> materialRepository, IRepository<ReturnToVendorHeader> returnToVendorHeaderRepository,
             IRepository<ReturnToVendorDetail> returnToVendorDetailRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
              IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository, IRepository<ProcessOrder> processOrderRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _sapReturnaToMaterialRepository = sapReturnaToMaterialRepository;
            _sapQcDetailRepository = sapQcDetailRepository;
            _dispensingAppService = dispensingAppService;
            _grnLabelPrintingContainerBarcodeRepository = grnLabelPrintingContainerBarcodeRepository;
            _grnDetailRepository = grnDetailRepository;
            _materialRepository = materialRepository;
            _returnToVendorHeaderRepository = returnToVendorHeaderRepository;
            _returnToVendorDetailRepository = returnToVendorDetailRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _processOrderRepository = processOrderRepository;
        }

        #endregion constructor

        #region public

        /// <summary>
        /// Used for getting material document No.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetMaterialDocumentNoAutoCompleteAsync(string input)
        {
            var responseDto = new HTTPResponseDto();
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            input = input.Trim();
            var materialDocumentQuery = from returnMaterial in _sapReturnaToMaterialRepository.GetAll()
                                        join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                        on new { MaterialCode = returnMaterial.ItemCode, SAPBatchNumber = returnMaterial.SAPBatchNo } equals
                                        new { MaterialCode = processOrderMaterial.ItemCode, SAPBatchNumber = processOrderMaterial.SAPBatchNo }
                                        join processOrder in _processOrderRepository.GetAll()
                                        on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                        where returnMaterial.MaterialDocumentNo.ToLower().Contains(input.ToLower())
                                        select new SelectListDtoWithPlantId
                                        {

                                            Value = returnMaterial.MaterialDocumentNo,
                                            PlantId = processOrder.PlantId
                                        };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                materialDocumentQuery = materialDocumentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var materialDocumentList = await materialDocumentQuery.ToListAsync() ?? default;
            materialDocumentList = materialDocumentList.GroupBy(x => x.Value).Select(x => x.First()).ToList();
            if (materialDocumentList?.Count() == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.MaterialDocumentNotFoundValidation);
            }
            responseDto.ResultObject = materialDocumentList;
            return responseDto;
        }

        /// <summary>
        /// Used for getting rejected material under selected material document no.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetMaterialCodeByDocumentNo(string input)
        {
            var responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var materialSelectList = await (from returnMaterial in _sapReturnaToMaterialRepository.GetAll()
                                                join qcDetail in _sapQcDetailRepository.GetAll()
                                                on returnMaterial.ItemCode.ToLower() equals qcDetail.ItemCode.ToLower()
                                                where returnMaterial.MaterialDocumentNo.ToLower() == input.ToLower() && qcDetail.BatchStockStatus.ToLower() == PMMSConsts.RejectedStatus.ToLower()
                                                select new SelectListDto
                                                {
                                                    Id = returnMaterial.Id,
                                                    Value = qcDetail.ItemCode
                                                }).ToListAsync();

                if (materialSelectList?.Count == 0)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialRejected);
                }
                responseDto.ResultObject = new { MaterialSelectList = materialSelectList };
            }
            return responseDto;
        }

        /// <summary>
        /// Used for getting sap batch number under selected material code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetSapBatchNumberByMaterialCodeAsync(string input)
        {
            var responseDto = new HTTPResponseDto();
            var sapBatchNumbers = await _sapQcDetailRepository.GetAll().Where(x => x.ItemCode.ToLower() == input.ToLower())
                                                .Select(x => new SelectListDto { Id = x.Id, Value = x.SAPBatchNo }).Distinct().ToListAsync();

            if (sapBatchNumbers?.Count == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoSAPAvailableUnderGroup);
            }
            responseDto.ResultObject = sapBatchNumbers;
            return responseDto;
        }

        /// <summary>
        /// Used for updating ArNo,UOM and Quantity.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> UpdateReturnToVendorDtoAsync(ReturnToVendorDto input)
        {
            var responseDto = new HTTPResponseDto();
            if (input != null)
            {
                var returnToMaterialData = await (from returnMaterial in _sapReturnaToMaterialRepository.GetAll()
                                                  join qcDetail in _sapQcDetailRepository.GetAll()
                                                  on returnMaterial.ItemCode.ToLower() equals qcDetail.ItemCode.ToLower()
                                                  join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                  on new { MaterialCode = returnMaterial.ItemCode, SAPBatchNumber = returnMaterial.SAPBatchNo } equals
                                                   new { MaterialCode = processOrderMaterial.ItemCode, SAPBatchNumber = processOrderMaterial.SAPBatchNo }
                                                  where returnMaterial.MaterialDocumentNo.ToLower() == input.MaterialDocumentId.ToLower()
                                                && qcDetail.BatchStockStatus == "Rejected" && returnMaterial.ItemCode.ToLower() == input.MaterialCode.ToLower()
                                                && returnMaterial.SAPBatchNo.ToLower() == qcDetail.SAPBatchNo.ToLower()
                                                  select new
                                                  {
                                                      Quantity = returnMaterial.Qty,
                                                      UOM = returnMaterial.UOM,
                                                      ArNo = processOrderMaterial.ARNo
                                                  }).FirstOrDefaultAsync();

                if (returnToMaterialData != null)
                {
                    input.UOM = returnToMaterialData.UOM;
                    input.Quantity = (float?)returnToMaterialData.Quantity;
                    input.ArNo = returnToMaterialData.ArNo;
                }
                else
                {
                    _dispensingAppService.UpdateErrorResponse(responseDto, "No UOM and Qty exists for selected combination");
                }
                //return to vendor already exists
                var returnToHeader = await _returnToVendorHeaderRepository.GetAll().Where(x => x.MaterialCode == input.MaterialCode
                                                                                       && x.MaterialDocumentNo == input.MaterialDocumentId
                                                                                       && x.SAPBatchNumber == input.SAPBatchNumber).FirstOrDefaultAsync();

                if (returnToHeader != null)
                {
                    input.Id = returnToHeader.Id;
                    input.ScanQty = await _returnToVendorDetailRepository.GetAll()
                                                 .Where(x => x.ReturnToVendorHeaderId == returnToHeader.Id).SumAsync(x => x.Qty);
                    input.StatusId = returnToHeader.StatusId;
                }
                else
                {
                    input.Id = 0;
                    input.ScanQty = 0;
                    input.StatusId = 0;
                }
                var postedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.ReturnToVendorSubModule, postedStatus);
                responseDto.ResultObject = new { Status = input.StatusId != postedStatusId ? "Not Posted" : "Posted", ReturnToVendorDto = input };
            }
            return responseDto;
        }

        /// <summary>
        /// Used for save return to vendor data on each material bar code scan.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.ReturnToVendor_SubModule + "." + PMMSPermissionConst.Add
            + "," + PMMSPermissionConst.ReturnToVendor_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> SaveReturnToVendorAsync(ReturnToVendorDto input)
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
               && grnDetail.SAPBatchNumber == input.SAPBatchNumber
               select new
               {
                   ContainerLabelId = grnContainerLabels.Id,
                   SAPBatchNo = grnDetail.SAPBatchNumber,
                   Quantity = grnContainerLabels.BalanceQuantity,
               }).FirstOrDefaultAsync();
            if (materialLabelContainer == null)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFound);
            }
            if (!await ValidateQuantity(input, materialLabelContainer.Quantity, responseDto))
            {
                return responseDto;
            }
            int returnToHeaderId = await GetReturnToVendorHeaderId(input);
            var alreadyExistPreStageContainerId = await (from containerDetail in _returnToVendorDetailRepository.GetAll()
                                                         where containerDetail.ReturnToVendorHeaderId == returnToHeaderId
                                                         && containerDetail.ContainerMaterialBarcode.ToLower() == input.MaterialContainerBarCode.ToLower()
                                                         select containerDetail.Id).FirstOrDefaultAsync();
            if (alreadyExistPreStageContainerId > 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
            }
            var returnToVendorDetails = new ReturnToVendorDetail();
            returnToVendorDetails.ReturnToVendorHeaderId = returnToHeaderId;
            returnToVendorDetails.ContainerMaterialBarcode = input.MaterialContainerBarCode;
            returnToVendorDetails.Qty = input.ConvertedQty;
            returnToVendorDetails.UOM = input.UOM;
            await _returnToVendorDetailRepository.InsertAndGetIdAsync(returnToVendorDetails);
            input.ScanQty = await _returnToVendorDetailRepository.GetAll()
                .Where(x => x.ReturnToVendorHeaderId == returnToHeaderId).SumAsync(x => x.Qty);
            input.Id = returnToHeaderId;
            //Update balance quantity from containerbarcodetable
            var containerForBalanceQuantity = await _grnLabelPrintingContainerBarcodeRepository.GetAsync(materialLabelContainer.ContainerLabelId);
            containerForBalanceQuantity.BalanceQuantity -= materialLabelContainer.Quantity;
            await _grnLabelPrintingContainerBarcodeRepository.UpdateAsync(containerForBalanceQuantity);
            await CurrentUnitOfWork.SaveChangesAsync();
            responseDto.ResultObject = input;
            return responseDto;
        }

        /// <summary>
        /// Used for postingdata.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.ReturnToVendor_SubModule + "." + PMMSPermissionConst.Add
          + "," + PMMSPermissionConst.ReturnToVendor_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> PostReturnToVendorAsync(ReturnToVendorDto input)
        {
            var responseDto = new HTTPResponseDto();
            if (input.Quantity != input.ScanQty)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.QuantityNotmatched);
            }
            else
            {
                //Get return To Vendor not posted records
                var postedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.ReturnToVendorSubModule, postedStatus);
                var notPostedReturnToVendorHeader = await _returnToVendorHeaderRepository.GetAll().Where(x => x.MaterialDocumentNo == input.MaterialDocumentId && x.StatusId != postedStatusId).ToListAsync();
                foreach (var result in notPostedReturnToVendorHeader)
                {
                    //TODO: post data to sap
                    var scanQuantity = await _returnToVendorDetailRepository.GetAll().Where(x => x.ReturnToVendorHeaderId == result.Id).SumAsync(x => x.Qty);
                    if (result.Qty == scanQuantity)
                    {
                        result.StatusId = postedStatusId;
                        await _returnToVendorHeaderRepository.UpdateAsync(result);
                    }
                }
            }
            responseDto.ResultObject = input;
            return responseDto;
        }

        #endregion public

        #region private

        private async Task<int> GetReturnToVendorHeaderId(ReturnToVendorDto input)
        {
            //ReturnToVendor header already exist
            var returnToVendorHeaderId = await _returnToVendorHeaderRepository.GetAll().Where(x => x.MaterialDocumentNo.ToLower() == input.MaterialDocumentId.ToLower()
            && x.MaterialCode.ToLower() == input.MaterialCode.ToLower() && x.SAPBatchNumber.ToLower() == input.SAPBatchNumber.ToLower()
             ).Select(x => x.Id).FirstOrDefaultAsync();
            if (returnToVendorHeaderId == 0)
            {
                var returnToHeader = new ReturnToVendorHeader();
                returnToHeader.MaterialDocumentNo = input.MaterialDocumentId;
                returnToHeader.MaterialCode = input.MaterialCode;
                returnToHeader.SAPBatchNumber = input.SAPBatchNumber;
                returnToHeader.ARNo = input.ArNo;
                returnToHeader.UOM = input.UOM;
                var notPostedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.ReturnToVendorSubModule, notPostedStatus);
                returnToHeader.StatusId = notPostedStatusId;
                returnToHeader.TenantId = AbpSession.TenantId;
                returnToHeader.Qty = input.Quantity;
                returnToVendorHeaderId = await _returnToVendorHeaderRepository.InsertAndGetIdAsync(returnToHeader);
            }
            return returnToVendorHeaderId;
        }
        /// <summary>
        /// Used for getting base uom of scan material container barcode.
        /// </summary>
        /// <param name="materialCode"></param>
        /// <param name="SAPBatchNo"></param>
        /// <returns></returns>
        private async Task<ReturnToVendorUnitOfMeasurementDto> GetBaseUOMByMaterialCode(string materialCode, string SAPBatchNo)
        {
            return await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                          join uomMaster in _unitOfMeasurementRepository.GetAll()
                          on processOrderMaterial.UnitOfMeasurement equals uomMaster.UnitOfMeasurement
                          where processOrderMaterial.ItemCode.ToLower() == materialCode.ToLower()
                          && processOrderMaterial.SAPBatchNo.ToLower() == SAPBatchNo.ToLower()
                          select new ReturnToVendorUnitOfMeasurementDto { Id = uomMaster.Id, UnitOfMeasurement = uomMaster.UnitOfMeasurement })
                          .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Used for getting uom id of SAPReturntoMaterial UOM.
        /// </summary>
        /// <param name="uom"></param>
        /// <returns></returns>
        private async Task<int> GetReturnToMaterialUomId(string uom)
        {
            return await (from uomMaster in _unitOfMeasurementRepository.GetAll()
                          where uomMaster.UnitOfMeasurement.ToLower() == uom
                          select uomMaster.Id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Used for getting converted qty in return to material uom and also used to validate qty.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="containerQty"></param>
        /// <param name="responseDto"></param>
        /// <returns></returns>
        private async Task<bool> ValidateQuantity(ReturnToVendorDto input, float? containerQty, HTTPResponseDto responseDto)
        {
            var baseUnitOfMeasurement = await GetBaseUOMByMaterialCode(input.MaterialCode, input.SAPBatchNumber);
            input.BaseUom = baseUnitOfMeasurement.UnitOfMeasurement;
            input.ConvertedQty = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, await GetReturnToMaterialUomId(input.UOM), baseUnitOfMeasurement.Id, containerQty.Value);
            if (input.Quantity < (input.ScanQty + input.ConvertedQty))
            {
                //Not valid
                _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.scanQtyGreaterThanBalanceQuantity);
                return false;
            }
            return true;
        }

        #endregion private
    }
}