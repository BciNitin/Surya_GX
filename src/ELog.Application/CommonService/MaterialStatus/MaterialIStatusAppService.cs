using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonService.MaterialStatus.Dto;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.MaterialStatus
{
    //[PMMSAuthorize]
    public class MaterialIStatusAppService : ApplicationService, IMaterialIStatusAppService
    {
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
        private readonly IRepository<MaterialReturn> _materialReturnRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<MaterialMaster> _materialmasterRepository;
        private readonly IRepository<SAPGRNPosting> _sapGRNPostingRepository;
        private readonly IRepository<User, long> _userRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public MaterialIStatusAppService(IRepository<SAPQualityControlDetail> sAPQualityControlDetailRepository, IRepository<MaterialConsignmentDetail> materialConsignmentRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository, IRepository<DispensingHeader> dispensingHeaderRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository, IHttpContextAccessor httpContextAccessor, IRepository<MaterialInspectionRelationDetail> materialInspectionRelationDetailRepository,
            IRepository<Material> materialRepository, IRepository<GRNDetail> grnDetailRepository, IRepository<GRNHeader> grnHeaderRepository,
            IRepository<InvoiceDetail> invoiceDetailRepository, IRepository<IssueToProduction> issueToProductionRepository,
            IRepository<MaterialReturn> materialReturnRepository, IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository, IRepository<MaterialMaster> materialmasterRepository, IRepository<SAPGRNPosting> sapGRNPostingRepository,
            IRepository<User, long> userRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _grnHeaderRepository = grnHeaderRepository;
            _sAPQualityControlDetailRepository = sAPQualityControlDetailRepository;
            _materialConsignmentRepository = materialConsignmentRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _httpContextAccessor = httpContextAccessor;
            _materialInspectionRelationDetailRepository = materialInspectionRelationDetailRepository;
            _materialRepository = materialRepository;
            _grnDetailRepository = grnDetailRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _issueToProductionRepository = issueToProductionRepository;
            _materialReturnRepository = materialReturnRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _materialmasterRepository = materialmasterRepository;
            _sapGRNPostingRepository = sapGRNPostingRepository;
            _userRepository = userRepository;
        }

        public async Task<List<MaterialStatusDto>> GetAllMaterilStatusListAsync()
        {
            var plantId = string.Empty; //_httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialStatusQuery = from material in _materialRepository.GetAll()
                                      join purchaseOrder in _purchaseOrderRepository.GetAll()
                                      on material.PurchaseOrderId equals purchaseOrder.Id
                                      join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
                                      on material.Id equals materialRelation.MaterialId
                                      join consignment in _materialConsignmentRepository.GetAll()
                                      on materialRelation.Id equals consignment.MaterialRelationId
                                      join grnDetail in _grnDetailRepository.GetAll()
                                      on material.Id equals grnDetail.MaterialId
                                      where consignment.ExpiryDate.HasValue && consignment.RetestDate.HasValue
                                      && consignment.ManufacturedDate.HasValue
                                      group new { material.Id, grnDetail.SAPBatchNumber } by new
                                      {
                                          Id = material.Id,
                                          MaterialCode = material.ItemCode,
                                          MaterialDescription = material.ItemDescription,
                                          MfgDate = consignment.ManufacturedDate,
                                          RetestDate = consignment.RetestDate,
                                          ExpiryDate = consignment.ExpiryDate,
                                          SapBatchNo = grnDetail.SAPBatchNumber,
                                          PlantId = purchaseOrder.PlantId,
                                          GrnDetailId = grnDetail.Id,
                                      } into gcs
                                      select new MaterialStatusDto
                                      {
                                          Id = gcs.Key.Id,
                                          MaterialCode = gcs.Key.MaterialCode,
                                          MaterialDescription = gcs.Key.MaterialDescription,
                                          MfgDate = gcs.Key.MfgDate,
                                          RetestDate = gcs.Key.RetestDate,
                                          ExpiryDate = gcs.Key.ExpiryDate,
                                          DaysLeftForExpiry = gcs.Key.ExpiryDate.HasValue ? (gcs.Key.ExpiryDate.Value - DateTime.Now).Days : 0,
                                          DaysLeftForRetest = gcs.Key.RetestDate.HasValue ? (gcs.Key.RetestDate.Value - DateTime.Now).Days : 0,
                                          SapBatchNo = gcs.Key.SapBatchNo,
                                          PlantId = gcs.Key.PlantId,
                                          GrnDetailId = gcs.Key.GrnDetailId
                                      };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                materialStatusQuery = materialStatusQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await materialStatusQuery.ToListAsync() ?? default;
            foreach (var materialData in result)
            {
                var qcDetails = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == materialData.MaterialCode).FirstOrDefaultAsync();
                if (qcDetails != null)
                {
                    materialData.RetestDate = qcDetails.RetestDate;
                    materialData.DaysLeftForRetest = (qcDetails.RetestDate.Value - DateTime.Now).Days;
                }
            }
            var data = result.Where(x => x.DaysLeftForExpiry < 15 || x.DaysLeftForRetest < 15).ToList();
            return data;
        }
        public async Task<MaterialDetailDto> GetMaterilDetailByIdAsync(int id, string sapBatchNumber)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialStatusQuery = from material in _materialRepository.GetAll()
                                      join purchaseOrder in _purchaseOrderRepository.GetAll()
                                      on material.PurchaseOrderId equals purchaseOrder.Id
                                      join invoice in _invoiceDetailRepository.GetAll()
                                      on purchaseOrder.Id equals invoice.PurchaseOrderId
                                      join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
                                      on material.Id equals materialRelation.MaterialId
                                      join consignment in _materialConsignmentRepository.GetAll()
                                      on materialRelation.Id equals consignment.MaterialRelationId
                                      join grnDetail in _grnDetailRepository.GetAll()
                                      on material.Id equals grnDetail.MaterialId
                                      join grnHeader in _grnHeaderRepository.GetAll()
                                      on grnDetail.GRNHeaderId equals grnHeader.Id
                                      join sapQcDeatils in _sAPQualityControlDetailRepository.GetAll()
                                      on material.ItemCode.ToLower() equals sapQcDeatils.ItemCode.ToLower() into sapQuality
                                      from qcDetails in sapQuality.DefaultIfEmpty()
                                      where material.Id == id && grnDetail.SAPBatchNumber == sapBatchNumber
                                      select new MaterialDetailDto
                                      {

                                          MaterialCode = material.ItemCode,
                                          MaterialDescription = material.ItemDescription,
                                          MgfDate = consignment.ManufacturedDate,
                                          RetestDate = qcDetails.RetestDate != null ? qcDetails.RetestDate : consignment.RetestDate,
                                          ExpiryDate = consignment.ExpiryDate,
                                          DaysLeftForExpiry = consignment.ExpiryDate.HasValue ? (consignment.ExpiryDate.Value - DateTime.Now).Days : 0,
                                          DaysLeftForRetest = qcDetails.RetestDate != null ? (qcDetails.RetestDate.Value - DateTime.Now).Days : (consignment.RetestDate.Value - DateTime.Now).Days,
                                          SapBatchNo = grnDetail.SAPBatchNumber,
                                          GRNNo = grnHeader.GRNNumber,
                                          NoOfContainer = grnDetail.NoOfContainer,
                                          MfgBatchNo = consignment.ManufacturedBatchNo,
                                          VendorCode = invoice.VendorCode,
                                          ManufacturerCode = invoice.Manufacturer,
                                          QCInvDate = invoice.InvoiceDate,
                                      };
            var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            var processOrderMaterial = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).FirstOrDefaultAsync();
            if (processOrderMaterial != null)
            {

                result.TotalQtyReceived = processOrderMaterial.OrderQuantity + " " + processOrderMaterial.UnitOfMeasurement;
                result.ArNo = processOrderMaterial.ARNo;
                result.QtyIssueToProduction = await _issueToProductionRepository.GetAll().Where(x => x.ProcessOrderNo == processOrderMaterial.ProcessOrderNo && x.MaterialCode == processOrderMaterial.ItemCode).Select(x => x.DispensedQty + "" + x.UOM).FirstOrDefaultAsync();
                result.Status = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).Select(x => x.BatchStockStatus).FirstOrDefaultAsync();
                var returnData = await _materialReturnRepository.GetAll().Where(x => x.ProcessOrderId == processOrderMaterial.ProcessOrderId.ToString() && x.IsActive).FirstOrDefaultAsync();
                if (returnData != null)
                {
                    var returnUom = await _unitOfMeasurementRepository.GetAll().Where(x => x.Id == returnData.UOMId).Select(x => x.UnitOfMeasurement).FirstOrDefaultAsync();
                    result.QtyReturnedFromProduction = $"{returnData.Quantity} {returnUom}";

                }

            }
            return result;
        }

        public async Task<MaterialDetailDto> GetMaterilDetailByIdOnlyAsync(string materialCode)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();



            var material = from mat in _materialmasterRepository.GetAll()
                               //join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
                               //on mat.Id equals materialRelation.MaterialId
                               //join consignment in _materialConsignmentRepository.GetAll()
                               //on materialRelation.Id equals consignment.MaterialRelationId
                           join grnDetail in _grnDetailRepository.GetAll()
                           on mat.Id equals grnDetail.MaterialId
                           join sap in _sapGRNPostingRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals sap.SAPBatchNo
                           //join qc in _sAPQualityControlDetailRepository.GetAll()
                           //on grnDetail.SAPBatchNumber equals qc.SAPBatchNo

                           where mat.MaterialCode == materialCode
                           select new MaterialDetailDto
                           {

                               MaterialCode = mat.MaterialCode,
                               MaterialDescription = mat.MaterialDescription,
                               //MfgBatchNo = sap.MfgBatchNo,
                               // RetestDate = consignment.RetestDate,
                               SapBatchNo = grnDetail.SAPBatchNumber,
                               NoOfContainer = grnDetail.NoOfContainer,
                               UOM = sap.UOM,
                               TotalQtyReceived = sap.NetQty,
                               TempStatus = mat.TempStatus,
                               //ContainerNo = grnDetail.NoOfContainer,
                               //RetestDate = qc.RetestDate,
                               //ReleaseDate = qc.ReleasedOn,






                           };

            var result = await material.FirstOrDefaultAsync() ?? default;

            //var materialStatusQuery = from material in _materialRepository.GetAll()
            //                          join purchaseOrder in _purchaseOrderRepository.GetAll()
            //                          on material.PurchaseOrderId equals purchaseOrder.Id
            //                          join invoice in _invoiceDetailRepository.GetAll()
            //                          on purchaseOrder.Id equals invoice.PurchaseOrderId
            //                          join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
            //                          on material.Id equals materialRelation.MaterialId
            //                          join consignment in _materialConsignmentRepository.GetAll()
            //                          on materialRelation.Id equals consignment.MaterialRelationId
            //                          join grnDetail in _grnDetailRepository.GetAll()
            //                          on material.Id equals grnDetail.MaterialId
            //                          join grnHeader in _grnHeaderRepository.GetAll()
            //                          on grnDetail.GRNHeaderId equals grnHeader.Id
            //                          join sapQcDeatils in _sAPQualityControlDetailRepository.GetAll()
            //                          on material.ItemCode.ToLower() equals sapQcDeatils.ItemCode.ToLower() into sapQuality
            //                          from qcDetails in sapQuality.DefaultIfEmpty()
            //                          where material.ItemCode == materialCode
            //                          //&& grnDetail.SAPBatchNumber == sapBatchNumber
            //                          select new MaterialDetailDto
            //                          {

            //                              MaterialCode = material.ItemCode,
            //                              MaterialDescription = material.ItemDescription,
            //                              MgfDate = consignment.ManufacturedDate,
            //                              RetestDate = qcDetails.RetestDate != null ? qcDetails.RetestDate : consignment.RetestDate,
            //                              ExpiryDate = consignment.ExpiryDate,
            //                              DaysLeftForExpiry = consignment.ExpiryDate.HasValue ? (consignment.ExpiryDate.Value - DateTime.Now).Days : 0,
            //                              DaysLeftForRetest = qcDetails.RetestDate != null ? (qcDetails.RetestDate.Value - DateTime.Now).Days : (consignment.RetestDate.Value - DateTime.Now).Days,
            //                              SapBatchNo = grnDetail.SAPBatchNumber,
            //                              GRNNo = grnHeader.GRNNumber,
            //                              NoOfContainer = grnDetail.NoOfContainer,
            //                              MfgBatchNo = consignment.ManufacturedBatchNo,
            //                              VendorCode = invoice.VendorCode,
            //                              ManufacturerCode = invoice.Manufacturer,
            //                              QCInvDate = invoice.InvoiceDate,
            //                          };
            //  var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            //var processOrderMaterial = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).FirstOrDefaultAsync();
            //if (processOrderMaterial != null)
            //{

            //    result.TotalQtyReceived = processOrderMaterial.OrderQuantity + " " + processOrderMaterial.UnitOfMeasurement;
            //    result.ArNo = processOrderMaterial.ARNo;
            //    result.QtyIssueToProduction = await _issueToProductionRepository.GetAll().Where(x => x.ProcessOrderNo == processOrderMaterial.ProcessOrderNo && x.MaterialCode == processOrderMaterial.ItemCode).Select(x => x.DispensedQty + "" + x.UOM).FirstOrDefaultAsync();
            //    result.Status = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).Select(x => x.BatchStockStatus).FirstOrDefaultAsync();
            //    var returnData = await _materialReturnRepository.GetAll().Where(x => x.ProcessOrderId == processOrderMaterial.ProcessOrderId.ToString() && x.IsActive).FirstOrDefaultAsync();
            //    if (returnData != null)
            //    {
            //        var returnUom = await _unitOfMeasurementRepository.GetAll().Where(x => x.Id == returnData.UOMId).Select(x => x.UnitOfMeasurement).FirstOrDefaultAsync();
            //        result.QtyReturnedFromProduction = $"{returnData.Quantity} {returnUom}";

            //    }

            //}
            return result;
        }

        public async Task<MaterialDetailDto> GetMaterilDetailByIdOnlyMfgDetailsAsync(string materialCode)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();



            var material = from mat in _materialmasterRepository.GetAll()
                               //join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
                               //on mat.Id equals materialRelation.MaterialId
                               //join consignment in _materialConsignmentRepository.GetAll()
                               //on materialRelation.Id equals consignment.MaterialRelationId
                           join grnDetail in _grnDetailRepository.GetAll()
                           on mat.Id equals grnDetail.MaterialId
                           join sap in _sapGRNPostingRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals sap.SAPBatchNo
                           join qc in _sAPQualityControlDetailRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals qc.SAPBatchNo

                           where mat.MaterialCode == materialCode
                           select new MaterialDetailDto
                           {

                               MaterialCode = mat.MaterialCode,
                               MaterialDescription = mat.MaterialDescription,
                               MfgBatchNo = sap.MfgBatchNo,
                               // RetestDate = consignment.RetestDate,
                               SapBatchNo = grnDetail.SAPBatchNumber,
                               NoOfContainer = grnDetail.NoOfContainer,
                               UOM = sap.UOM,
                               TotalQtyReceived = sap.NetQty,
                               ContainerNo = grnDetail.NoOfContainer,
                               RetestDate = qc.RetestDate,
                               ReleaseDate = qc.ReleasedOn,






                           };

            var result = await material.FirstOrDefaultAsync() ?? default;

            //var materialStatusQuery = from material in _materialRepository.GetAll()
            //                          join purchaseOrder in _purchaseOrderRepository.GetAll()
            //                          on material.PurchaseOrderId equals purchaseOrder.Id
            //                          join invoice in _invoiceDetailRepository.GetAll()
            //                          on purchaseOrder.Id equals invoice.PurchaseOrderId
            //                          join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
            //                          on material.Id equals materialRelation.MaterialId
            //                          join consignment in _materialConsignmentRepository.GetAll()
            //                          on materialRelation.Id equals consignment.MaterialRelationId
            //                          join grnDetail in _grnDetailRepository.GetAll()
            //                          on material.Id equals grnDetail.MaterialId
            //                          join grnHeader in _grnHeaderRepository.GetAll()
            //                          on grnDetail.GRNHeaderId equals grnHeader.Id
            //                          join sapQcDeatils in _sAPQualityControlDetailRepository.GetAll()
            //                          on material.ItemCode.ToLower() equals sapQcDeatils.ItemCode.ToLower() into sapQuality
            //                          from qcDetails in sapQuality.DefaultIfEmpty()
            //                          where material.ItemCode == materialCode
            //                          //&& grnDetail.SAPBatchNumber == sapBatchNumber
            //                          select new MaterialDetailDto
            //                          {

            //                              MaterialCode = material.ItemCode,
            //                              MaterialDescription = material.ItemDescription,
            //                              MgfDate = consignment.ManufacturedDate,
            //                              RetestDate = qcDetails.RetestDate != null ? qcDetails.RetestDate : consignment.RetestDate,
            //                              ExpiryDate = consignment.ExpiryDate,
            //                              DaysLeftForExpiry = consignment.ExpiryDate.HasValue ? (consignment.ExpiryDate.Value - DateTime.Now).Days : 0,
            //                              DaysLeftForRetest = qcDetails.RetestDate != null ? (qcDetails.RetestDate.Value - DateTime.Now).Days : (consignment.RetestDate.Value - DateTime.Now).Days,
            //                              SapBatchNo = grnDetail.SAPBatchNumber,
            //                              GRNNo = grnHeader.GRNNumber,
            //                              NoOfContainer = grnDetail.NoOfContainer,
            //                              MfgBatchNo = consignment.ManufacturedBatchNo,
            //                              VendorCode = invoice.VendorCode,
            //                              ManufacturerCode = invoice.Manufacturer,
            //                              QCInvDate = invoice.InvoiceDate,
            //                          };
            //  var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            //var processOrderMaterial = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).FirstOrDefaultAsync();
            //if (processOrderMaterial != null)
            //{

            //    result.TotalQtyReceived = processOrderMaterial.OrderQuantity + " " + processOrderMaterial.UnitOfMeasurement;
            //    result.ArNo = processOrderMaterial.ARNo;
            //    result.QtyIssueToProduction = await _issueToProductionRepository.GetAll().Where(x => x.ProcessOrderNo == processOrderMaterial.ProcessOrderNo && x.MaterialCode == processOrderMaterial.ItemCode).Select(x => x.DispensedQty + "" + x.UOM).FirstOrDefaultAsync();
            //    result.Status = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).Select(x => x.BatchStockStatus).FirstOrDefaultAsync();
            //    var returnData = await _materialReturnRepository.GetAll().Where(x => x.ProcessOrderId == processOrderMaterial.ProcessOrderId.ToString() && x.IsActive).FirstOrDefaultAsync();
            //    if (returnData != null)
            //    {
            //        var returnUom = await _unitOfMeasurementRepository.GetAll().Where(x => x.Id == returnData.UOMId).Select(x => x.UnitOfMeasurement).FirstOrDefaultAsync();
            //        result.QtyReturnedFromProduction = $"{returnData.Quantity} {returnUom}";

            //    }

            //}
            return result;
        }

        public async Task<MaterialDetailDto> GetMaterilDetailByIdOnlyPOAsync(string materialCode)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();



            var material = from mat in _materialmasterRepository.GetAll()
                           join grnDetail in _grnDetailRepository.GetAll()
                           on mat.Id equals grnDetail.MaterialId
                           join po in _processOrderMaterialRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals po.SAPBatchNo

                           where mat.MaterialCode == materialCode
                           select new MaterialDetailDto
                           {

                               MaterialCode = mat.MaterialCode,
                               MaterialDescription = mat.MaterialDescription,
                               MfgBatchNo = po.BatchNo,
                               //MfgBatchNo = sap.MfgBatchNo,
                               // RetestDate = consignment.RetestDate,
                               SapBatchNo = grnDetail.SAPBatchNumber,
                               NoOfContainer = grnDetail.NoOfContainer,
                               // UOM = sap.UOM,
                               // TotalQtyReceived = sap.NetQty,
                               ContainerNo = grnDetail.NoOfContainer,
                               ProcessOrder = po.ProcessOrderNo,

                               // RetestDate = qc.RetestDate,
                               // ReleaseDate = qc.ReleasedOn,
                               // DoneBy= user.UserName,
                               // SamplingDate=disp.StartTime,
                               // ReleasedBy= qc.CreatorUser.FullName,



                           };

            var result = await material.FirstOrDefaultAsync() ?? default;

            //var materialStatusQuery = from material in _materialRepository.GetAll()
            //                          join purchaseOrder in _purchaseOrderRepository.GetAll()
            //                          on material.PurchaseOrderId equals purchaseOrder.Id
            //                          join invoice in _invoiceDetailRepository.GetAll()
            //                          on purchaseOrder.Id equals invoice.PurchaseOrderId
            //                          join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
            //                          on material.Id equals materialRelation.MaterialId
            //                          join consignment in _materialConsignmentRepository.GetAll()
            //                          on materialRelation.Id equals consignment.MaterialRelationId
            //                          join grnDetail in _grnDetailRepository.GetAll()
            //                          on material.Id equals grnDetail.MaterialId
            //                          join grnHeader in _grnHeaderRepository.GetAll()
            //                          on grnDetail.GRNHeaderId equals grnHeader.Id
            //                          join sapQcDeatils in _sAPQualityControlDetailRepository.GetAll()
            //                          on material.ItemCode.ToLower() equals sapQcDeatils.ItemCode.ToLower() into sapQuality
            //                          from qcDetails in sapQuality.DefaultIfEmpty()
            //                          where material.ItemCode == materialCode
            //                          //&& grnDetail.SAPBatchNumber == sapBatchNumber
            //                          select new MaterialDetailDto
            //                          {

            //                              MaterialCode = material.ItemCode,
            //                              MaterialDescription = material.ItemDescription,
            //                              MgfDate = consignment.ManufacturedDate,
            //                              RetestDate = qcDetails.RetestDate != null ? qcDetails.RetestDate : consignment.RetestDate,
            //                              ExpiryDate = consignment.ExpiryDate,
            //                              DaysLeftForExpiry = consignment.ExpiryDate.HasValue ? (consignment.ExpiryDate.Value - DateTime.Now).Days : 0,
            //                              DaysLeftForRetest = qcDetails.RetestDate != null ? (qcDetails.RetestDate.Value - DateTime.Now).Days : (consignment.RetestDate.Value - DateTime.Now).Days,
            //                              SapBatchNo = grnDetail.SAPBatchNumber,
            //                              GRNNo = grnHeader.GRNNumber,
            //                              NoOfContainer = grnDetail.NoOfContainer,
            //                              MfgBatchNo = consignment.ManufacturedBatchNo,
            //                              VendorCode = invoice.VendorCode,
            //                              ManufacturerCode = invoice.Manufacturer,
            //                              QCInvDate = invoice.InvoiceDate,
            //                          };
            //  var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            //var processOrderMaterial = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).FirstOrDefaultAsync();
            //if (processOrderMaterial != null)
            //{

            //    result.TotalQtyReceived = processOrderMaterial.OrderQuantity + " " + processOrderMaterial.UnitOfMeasurement;
            //    result.ArNo = processOrderMaterial.ARNo;
            //    result.QtyIssueToProduction = await _issueToProductionRepository.GetAll().Where(x => x.ProcessOrderNo == processOrderMaterial.ProcessOrderNo && x.MaterialCode == processOrderMaterial.ItemCode).Select(x => x.DispensedQty + "" + x.UOM).FirstOrDefaultAsync();
            //    result.Status = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).Select(x => x.BatchStockStatus).FirstOrDefaultAsync();
            //    var returnData = await _materialReturnRepository.GetAll().Where(x => x.ProcessOrderId == processOrderMaterial.ProcessOrderId.ToString() && x.IsActive).FirstOrDefaultAsync();
            //    if (returnData != null)
            //    {
            //        var returnUom = await _unitOfMeasurementRepository.GetAll().Where(x => x.Id == returnData.UOMId).Select(x => x.UnitOfMeasurement).FirstOrDefaultAsync();
            //        result.QtyReturnedFromProduction = $"{returnData.Quantity} {returnUom}";

            //    }

            //}
            return result;
        }

        public async Task<MaterialDetailDto> GetMaterilDetailByIdOnlySamplingAsync(string materialCode)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();



            var material = from mat in _materialmasterRepository.GetAll()
                           join grnDetail in _grnDetailRepository.GetAll()
on mat.Id equals grnDetail.MaterialId
                           join sap in _sapGRNPostingRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals sap.SAPBatchNo
                           join qc in _sAPQualityControlDetailRepository.GetAll()
                           on grnDetail.SAPBatchNumber equals qc.SAPBatchNo
                           join disp in _dispensingHeaderRepository.GetAll()
                           on mat.MaterialCode equals disp.MaterialCodeId
                           join user in _userRepository.GetAll()
                            on disp.DoneBy equals Convert.ToInt32(user.Id)

                           where mat.MaterialCode == materialCode && disp.IsSampling
                           select new MaterialDetailDto
                           {

                               MaterialCode = mat.MaterialCode,
                               MaterialDescription = mat.MaterialDescription,
                               MfgBatchNo = sap.MfgBatchNo,
                               // RetestDate = consignment.RetestDate,
                               SapBatchNo = grnDetail.SAPBatchNumber,
                               NoOfContainer = grnDetail.NoOfContainer,
                               UOM = sap.UOM,
                               TotalQtyReceived = sap.NetQty,
                               ContainerNo = grnDetail.NoOfContainer,
                               RetestDate = qc.RetestDate,
                               ReleaseDate = qc.ReleasedOn,
                               DoneBy = user.UserName,
                               SamplingDate = disp.StartTime,
                               ReleasedBy = qc.CreatorUser.FullName,



                           };

            var result = await material.FirstOrDefaultAsync() ?? default;

            //var materialStatusQuery = from material in _materialRepository.GetAll()
            //                          join purchaseOrder in _purchaseOrderRepository.GetAll()
            //                          on material.PurchaseOrderId equals purchaseOrder.Id
            //                          join invoice in _invoiceDetailRepository.GetAll()
            //                          on purchaseOrder.Id equals invoice.PurchaseOrderId
            //                          join materialRelation in _materialInspectionRelationDetailRepository.GetAll()
            //                          on material.Id equals materialRelation.MaterialId
            //                          join consignment in _materialConsignmentRepository.GetAll()
            //                          on materialRelation.Id equals consignment.MaterialRelationId
            //                          join grnDetail in _grnDetailRepository.GetAll()
            //                          on material.Id equals grnDetail.MaterialId
            //                          join grnHeader in _grnHeaderRepository.GetAll()
            //                          on grnDetail.GRNHeaderId equals grnHeader.Id
            //                          join sapQcDeatils in _sAPQualityControlDetailRepository.GetAll()
            //                          on material.ItemCode.ToLower() equals sapQcDeatils.ItemCode.ToLower() into sapQuality
            //                          from qcDetails in sapQuality.DefaultIfEmpty()
            //                          where material.ItemCode == materialCode
            //                          //&& grnDetail.SAPBatchNumber == sapBatchNumber
            //                          select new MaterialDetailDto
            //                          {

            //                              MaterialCode = material.ItemCode,
            //                              MaterialDescription = material.ItemDescription,
            //                              MgfDate = consignment.ManufacturedDate,
            //                              RetestDate = qcDetails.RetestDate != null ? qcDetails.RetestDate : consignment.RetestDate,
            //                              ExpiryDate = consignment.ExpiryDate,
            //                              DaysLeftForExpiry = consignment.ExpiryDate.HasValue ? (consignment.ExpiryDate.Value - DateTime.Now).Days : 0,
            //                              DaysLeftForRetest = qcDetails.RetestDate != null ? (qcDetails.RetestDate.Value - DateTime.Now).Days : (consignment.RetestDate.Value - DateTime.Now).Days,
            //                              SapBatchNo = grnDetail.SAPBatchNumber,
            //                              GRNNo = grnHeader.GRNNumber,
            //                              NoOfContainer = grnDetail.NoOfContainer,
            //                              MfgBatchNo = consignment.ManufacturedBatchNo,
            //                              VendorCode = invoice.VendorCode,
            //                              ManufacturerCode = invoice.Manufacturer,
            //                              QCInvDate = invoice.InvoiceDate,
            //                          };
            //  var result = await materialStatusQuery.FirstOrDefaultAsync() ?? default;
            //var processOrderMaterial = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).FirstOrDefaultAsync();
            //if (processOrderMaterial != null)
            //{

            //    result.TotalQtyReceived = processOrderMaterial.OrderQuantity + " " + processOrderMaterial.UnitOfMeasurement;
            //    result.ArNo = processOrderMaterial.ARNo;
            //    result.QtyIssueToProduction = await _issueToProductionRepository.GetAll().Where(x => x.ProcessOrderNo == processOrderMaterial.ProcessOrderNo && x.MaterialCode == processOrderMaterial.ItemCode).Select(x => x.DispensedQty + "" + x.UOM).FirstOrDefaultAsync();
            //    result.Status = await _sAPQualityControlDetailRepository.GetAll().Where(x => x.ItemCode == result.MaterialCode && x.SAPBatchNo == result.SapBatchNo).Select(x => x.BatchStockStatus).FirstOrDefaultAsync();
            //    var returnData = await _materialReturnRepository.GetAll().Where(x => x.ProcessOrderId == processOrderMaterial.ProcessOrderId.ToString() && x.IsActive).FirstOrDefaultAsync();
            //    if (returnData != null)
            //    {
            //        var returnUom = await _unitOfMeasurementRepository.GetAll().Where(x => x.Id == returnData.UOMId).Select(x => x.UnitOfMeasurement).FirstOrDefaultAsync();
            //        result.QtyReturnedFromProduction = $"{returnData.Quantity} {returnUom}";

            //    }

            //}
            return result;
        }
    }
}