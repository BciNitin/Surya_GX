using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.MaterialStatusLabel.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.MaterialStatusLabel
{
    [PMMSAuthorize]
    public class MaterialStatusLabelAppService : ApplicationService, IMaterialStatusLabelAppService
    {
        #region fields

        private readonly IRepository<SAPQualityControlDetail> _sAPQualityControlDetailRepository;
        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentRepository;
        private readonly IRepository<MaterialInspectionRelationDetail> _materialInspectionRelationDetailRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<GRNHeader> _grnHeaderRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<IssueToProduction> _issueToProductionRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingBarcodeRepository;
        private readonly IRepository<GRNMaterialLabelPrintingHeader> _grnMaterialLabelPrintingHeaderRepository;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;
        private readonly IDispensingAppService _dispensingAppService;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly string SamplingCompletedStatus = nameof(SamplingHeaderStatus.Completed).ToLower();

        #endregion fields

        #region constructor

        public MaterialStatusLabelAppService(IRepository<SAPQualityControlDetail> sAPQualityControlDetailRepository, IRepository<MaterialConsignmentDetail> materialConsignmentRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository, IRepository<DispensingHeader> dispensingHeaderRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository, IHttpContextAccessor httpContextAccessor, IRepository<MaterialInspectionRelationDetail> materialInspectionRelationDetailRepository,
            IRepository<Material> materialRepository, IRepository<GRNDetail> grnDetailRepository, IRepository<GRNHeader> grnHeaderRepository,
            IRepository<InvoiceDetail> invoiceDetailRepository, IRepository<IssueToProduction> issueToProductionRepository, IRepository<InspectionLot> inspectionLotRepository,
            IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingBarcodeRepository, IRepository<GRNMaterialLabelPrintingHeader> grnMaterialLabelPrintingHeaderRepository,
            IDispensingAppService dispensingAppService
            )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _grnHeaderRepository = grnHeaderRepository;
            _sAPQualityControlDetailRepository = sAPQualityControlDetailRepository;
            _materialConsignmentRepository = materialConsignmentRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _materialInspectionRelationDetailRepository = materialInspectionRelationDetailRepository;
            _materialRepository = materialRepository;
            _grnDetailRepository = grnDetailRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _issueToProductionRepository = issueToProductionRepository;
            _grnLabelPrintingBarcodeRepository = grnLabelPrintingBarcodeRepository;
            _grnMaterialLabelPrintingHeaderRepository = grnMaterialLabelPrintingHeaderRepository;
            _inspectionLotRepository = inspectionLotRepository;
            _dispensingAppService = dispensingAppService;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialStatusLabel_Submodule + "." + PMMSPermissionConst.View)]
        public async Task<HTTPResponseDto> GetMaterialStatusLabelByMaterialBarcode(string materialContainerBarCode)
        {
            var responseDto = new HTTPResponseDto();
            var grnMaterialLabelPrintingContainerBarcode = await _grnLabelPrintingBarcodeRepository.FirstOrDefaultAsync(x =>
            x.MaterialLabelContainerBarCode.ToLower() == materialContainerBarCode.ToLower());
            if (grnMaterialLabelPrintingContainerBarcode == null)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.MaterialLabelContainerNotFound);
            }
            var commonDetails = await GetCommonMaterilDetailByIdAsync(grnMaterialLabelPrintingContainerBarcode.Id);
            if (commonDetails == null)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.MaterialBarcoodeNotExist);
            }
            responseDto.ResultObject = await GetMaterialCurrentStatus(commonDetails);
            return responseDto;
        }

        public async Task<MaterialStatusLabelDto> GetCommonMaterilDetailByIdAsync(int grnMaterilLabelprintingId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialStatusQuery = from grnContainerBarcode in _grnLabelPrintingBarcodeRepository.GetAll()
                                      join grnLabel in _grnMaterialLabelPrintingHeaderRepository.GetAll()
                                      on grnContainerBarcode.GRNMaterialLabelPrintingHeaderId equals grnLabel.Id
                                      join grnDetail in _grnDetailRepository.GetAll()
                                      on grnLabel.GRNDetailId equals grnDetail.Id
                                      join grnHeader in _grnHeaderRepository.GetAll()
                                      on grnDetail.GRNHeaderId equals grnHeader.Id
                                      join material in _materialRepository.GetAll()
                                      on grnDetail.MaterialId equals material.Id
                                      join purchaseOrder in _purchaseOrderRepository.GetAll()
                                       on material.PurchaseOrderId equals purchaseOrder.Id
                                      join invoice in _invoiceDetailRepository.GetAll()
                                      on purchaseOrder.Id equals invoice.PurchaseOrderId
                                      join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
                                      on material.Id equals materialRelation.MaterialId
                                      join consignment in _materialConsignmentRepository.GetAll()
                                      on materialRelation.Id equals consignment.MaterialRelationId
                                      where grnContainerBarcode.Id == grnMaterilLabelprintingId
                                      select new MaterialStatusLabelDto
                                      {
                                          MaterialCode = material.ItemCode,
                                          MaterialDescription = material.ItemDescription,
                                          MfgDate = consignment.ManufacturedDate,
                                          MfgrRetestDate = consignment.RetestDate,
                                          ExpDate = consignment.ExpiryDate,
                                          SapBatchNo = grnDetail.SAPBatchNumber,
                                          GRNNo = grnHeader.GRNNumber,
                                          NoOfContainer = grnDetail.NoOfContainer,
                                          ManufacturerBatchNo = consignment.ManufacturedBatchNo,
                                          ManfactureCode = invoice.Manufacturer,
                                          InvoiceDate = invoice.InvoiceDate,
                                          VendorCode = invoice.VendorCode,
                                          PackSize = grnLabel.PackDetails,
                                          GrnPreparedBy = grnHeader.CreatorUser.FullName,
                                          TotalQtyReceived = $"{grnContainerBarcode.Quantity} {material.UnitOfMeasurement}",
                                          ExpiredQty = $"{grnContainerBarcode.BalanceQuantity} {material.UnitOfMeasurement}",
                                          PlantId = purchaseOrder.PlantId
                                      };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                materialStatusQuery = materialStatusQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            return result;
        }

        #endregion public

        #region private

        private HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, string ValidationError)
        {
            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
            responseDto.Error = ValidationError;
            return responseDto;
        }

        private async Task<MaterialStatusLabelDto> GetMaterialCurrentStatus(MaterialStatusLabelDto materialStatusLabelDto)
        {
            var inspectionLot = await (from inspectionLotData in _inspectionLotRepository.GetAll()
                                       join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                       on inspectionLotData.Id equals processOrderMaterial.InspectionLotId
                                       where processOrderMaterial.ItemCode.ToLower() == materialStatusLabelDto.MaterialCode &&
                                       processOrderMaterial.SAPBatchNo.ToLower() == materialStatusLabelDto.SapBatchNo
                                       select new
                                       {
                                           inspectionLotData.InspectionLotNumber,
                                           processOrderMaterial.OrderQuantity,
                                           processOrderMaterial.InspectionLotId,
                                           processOrderMaterial.ExpiryDate,
                                           processOrderMaterial.ARNo,
                                           processOrderMaterial.RetestDate,
                                           processOrderMaterial.UnitOfMeasurement
                                       }).FirstOrDefaultAsync();
            if (inspectionLot != null)
            {
                materialStatusLabelDto.InspectionLotNo = inspectionLot.InspectionLotNumber;
                materialStatusLabelDto.ArNo = inspectionLot.ARNo;
                materialStatusLabelDto.ExpDate = inspectionLot.ExpiryDate;
                if (inspectionLot.ExpiryDate < System.DateTime.Now)
                {
                    materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.Expired.GetAttribute<DisplayAttribute>().Name;
                    materialStatusLabelDto.StatusColorCode = PMMSConsts.ExpiredColorCode;
                    materialStatusLabelDto.IsExpired = true;
                    return materialStatusLabelDto;
                }
                var sapQcDeatils = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode.ToLower() == materialStatusLabelDto.MaterialCode.ToLower() && x.SAPBatchNo.ToLower() == materialStatusLabelDto.SapBatchNo.ToLower()).FirstOrDefaultAsync();
                if (sapQcDeatils != null)
                {
                    if (sapQcDeatils.RetestDate != inspectionLot.RetestDate)
                    {
                        materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.Retest.GetAttribute<DisplayAttribute>().Name;
                        materialStatusLabelDto.StatusColorCode = PMMSConsts.RetestColorCode;
                    }
                    else if (sapQcDeatils.BatchStockStatus.ToLower() == PMMSConsts.RejectedStatus.ToLower())
                    {
                        materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.Rejected.GetAttribute<DisplayAttribute>().Name;
                        materialStatusLabelDto.RejectedQty = $"{sapQcDeatils.ReleasedQty} {inspectionLot.UnitOfMeasurement}";
                        materialStatusLabelDto.StatusColorCode = PMMSConsts.RejectedColorCode;
                        materialStatusLabelDto.IsRejected = true;
                    }
                    else
                    {
                        materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.Released.GetAttribute<DisplayAttribute>().Name;
                        materialStatusLabelDto.ReleasedQty = $"{sapQcDeatils.ReleasedQty} {inspectionLot.UnitOfMeasurement}";
                        materialStatusLabelDto.StatusColorCode = PMMSConsts.ReleasedColorCode;
                        materialStatusLabelDto.InHouseRetestDate = sapQcDeatils.RetestDate;
                        materialStatusLabelDto.IsQcReleased = true;
                    }
                    return materialStatusLabelDto;
                }
                var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
                var sampingCompleted = await _dispensingHeaderRepository.GetAll().Where(x => x.InspectionLotId == inspectionLot.InspectionLotId && x.IsSampling && x.StatusId == completedStatusId).FirstOrDefaultAsync();
                if (sampingCompleted != null)
                {
                    materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.Sampled.GetAttribute<DisplayAttribute>().Name;
                    materialStatusLabelDto.StatusColorCode = PMMSConsts.SampledColorCode;
                }
                else
                {
                    materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.UnderTest.GetAttribute<DisplayAttribute>().Name;
                    materialStatusLabelDto.StatusColorCode = PMMSConsts.UnderTestColorCode;
                }
                return materialStatusLabelDto;
            }
            else
            {
                materialStatusLabelDto.Status = PMMSEnums.MaterialStatusLabel.UnderTest.GetAttribute<DisplayAttribute>().Name;
                materialStatusLabelDto.StatusColorCode = PMMSConsts.UnderTestColorCode;
            }

            return materialStatusLabelDto;
        }

        #endregion private
    }
}