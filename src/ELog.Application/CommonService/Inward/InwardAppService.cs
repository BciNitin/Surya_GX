using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Inward.GateEntries.Dto;
using ELog.Application.Inward.MaterialInspections.Dto;
using ELog.Application.Inward.Palletizations.Dto;
using ELog.Application.Inward.PutAways.Dto;
using ELog.Application.Inward.VehicleInspections.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Masters.UnitOfMeasurements.Dto;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.ConnectorFactory;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.Core.Hardware.WeighingMachine;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.CommonService.Inward
{
    [PMMSAuthorize]
    public class InwardAppService : ApplicationService, IInwardAppService
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<GateEntry> _gateEntryRepository;
        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<UnitOfMeasurementTypeMaster> _unitOfMeasurementTypeMasterRepository;
        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        private readonly IRepository<MaterialDamageDetail> _materialDamgeRepository;
        private readonly IRepository<MaterialInspectionRelationDetail> _materialInspectionRelationalRepository;
        private readonly IRepository<MaterialInspectionHeader> _materialInspectionheaderRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<MaterialInspectionHeader> _materialInspectionHeaderRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<GRNMaterialLabelPrintingHeader> _grnMaterialLabelPrintingHeaderRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerBarcodeRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<Palletization> _palletizationRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private const int putAwayPalletToBin = (int)PMMSEnums.MaterialTransferType.PutAwayPalletToBin;
        private const int putAwayMaterialToBin = (int)PMMSEnums.MaterialTransferType.PutAwayMaterialToBin;
        private const int binToBinTranferPalletToBin = (int)PMMSEnums.MaterialTransferType.BinToBinTranferPalletToBin;
        private const int binToBinTranferMaterialToBin = (int)PMMSEnums.MaterialTransferType.BinToBinTranferMaterialToBin;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTrasferRepository;
        private readonly WeighingScaleFactory _weighingScaleFactory;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public InwardAppService(IRepository<PurchaseOrder> purchaseOrderRepository,
          IRepository<GateEntry> gateEntryRepository,
          IRepository<InvoiceDetail> invoiceDetailRepository,
          IRepository<CheckpointMaster> checkpointRepository,
          IRepository<MaterialConsignmentDetail> materialConsignmentRepository,
          IHttpContextAccessor httpContextAccessor, IRepository<MaterialDamageDetail> materialDamgeRepository,
          IRepository<Material> materialRepository,
          IRepository<MaterialInspectionHeader> materialInspectionHeaderRepository,
          IRepository<GRNDetail> grnDetailRepository,
          IRepository<MaterialInspectionRelationDetail> materialInspectionRelationalRepository,
          IRepository<MaterialInspectionHeader> materialInspectionheaderRepository,
          IRepository<WeighingMachineMaster> weighingMachineRepository,
          IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
          IRepository<HandlingUnitMaster> handlingUnitRepository,
          IRepository<GRNMaterialLabelPrintingHeader> grnMaterialLabelPrintingHeaderRepository, IRepository<MaterialMaster> materialMasterRepository,
          IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerBarcodeRepository,
          IRepository<LocationMaster> locationRepository, IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTrasferRepository,
          IRepository<Palletization> palletizationRepository, IRepository<UnitOfMeasurementTypeMaster> unitOfMeasurementTypeMasterRepository,
          WeighingScaleFactory weighingScaleFactory, IConfiguration configuration, IRepository<VehicleInspectionHeader> vehicleInspectionRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _purchaseOrderRepository = purchaseOrderRepository;
            _gateEntryRepository = gateEntryRepository;
            _httpContextAccessor = httpContextAccessor;
            _invoiceDetailRepository = invoiceDetailRepository;
            _checkpointRepository = checkpointRepository;
            _materialRepository = materialRepository;
            _materialConsignmentRepository = materialConsignmentRepository;
            _materialInspectionRelationalRepository = materialInspectionRelationalRepository;
            _materialInspectionheaderRepository = materialInspectionheaderRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _grnDetailRepository = grnDetailRepository;
            _materialInspectionHeaderRepository = materialInspectionHeaderRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _grnMaterialLabelPrintingContainerBarcodeRepository = grnMaterialLabelPrintingContainerBarcodeRepository;
            _grnMaterialLabelPrintingHeaderRepository = grnMaterialLabelPrintingHeaderRepository;
            _locationRepository = locationRepository;
            _palletizationRepository = palletizationRepository;
            _putAwayBinToBinTrasferRepository = putAwayBinToBinTrasferRepository;
            _unitOfMeasurementTypeMasterRepository = unitOfMeasurementTypeMasterRepository;
            _weighingScaleFactory = weighingScaleFactory;
            _configuration = configuration;
            _vehicleInspectionRepository = vehicleInspectionRepository;
            _materialMasterRepository = materialMasterRepository;
            _materialDamgeRepository = materialDamgeRepository;
        }

        public async Task<List<MaterialSelectWithDescriptionDto>> GetMaterialSelectListDtoAsync(int purchaseOrderId)
        {

            var purchaseOrderDate = await _purchaseOrderRepository.GetAll().Where(purchaseOrder =>
                                                 purchaseOrder.Id == purchaseOrderId
                                               ).Select(x => new { x.PurchaseOrderDate }).ToListAsync()
                                     ;
            var PurchaseOrderDeliverSchedule = (int)((DateTime.Now - purchaseOrderDate[0].PurchaseOrderDate).TotalDays);
            var materials = (from material in _materialRepository.GetAll()
                             join master in _materialMasterRepository.GetAll()
                             on material.ItemCode equals master.MaterialCode into ms
                             from master in ms.DefaultIfEmpty()
                             where material.PurchaseOrderId == purchaseOrderId
                             select new MaterialSelectWithDescriptionDto
                             {
                                 Id = material.Id,
                                 Value = material.ItemCode + "-" + material.ItemNo,
                                 Description = material.ItemDescription,
                                 SelfLife = master.Flag,
                                 PurchaseOrderDeliverSchedule = PurchaseOrderDeliverSchedule
                             });

            return await materials.ToListAsync() ?? default;
        }

        public async Task<List<GateEntryDto>> GetGateEntriesAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var gateEntriesQuery = from gateEntry in _gateEntryRepository.GetAll()
                                   join invoice in _invoiceDetailRepository.GetAll()
                                   on gateEntry.InvoiceId equals invoice.Id
                                   join vi in _vehicleInspectionRepository.GetAll()
                                   on gateEntry.InvoiceId equals vi.InvoiceId
                                   join po in _purchaseOrderRepository.GetAll()
                                   on invoice.PurchaseOrderId equals po.Id into ps
                                   from po in ps.DefaultIfEmpty()
                                   where gateEntry.IsActive
                                   orderby gateEntry.GatePassNo
                                   select new GateEntryDto
                                   {
                                       Id = gateEntry.Id,
                                       GatePassNo = gateEntry.GatePassNo,
                                       PlantId = po.PlantId,
                                       TransactionstatusId = vi.TransactionStatusId,
                                       InvoiceDto = new InvoiceDto
                                       {
                                           Id = invoice.Id,
                                           PurchaseOrderId = invoice.PurchaseOrderId,
                                           PurchaseOrderNo = invoice.PurchaseOrderNo,
                                           VendorName = invoice.VendorName,
                                           VendorCode = invoice.VendorCode,
                                           LRDate = invoice.LRDate,
                                           LRNo = invoice.LRNo,
                                           InvoiceDate = invoice.InvoiceDate,
                                           InvoiceNo = invoice.InvoiceNo,
                                           DriverName = invoice.DriverName,
                                           VehicleNumber = invoice.VehicleNumber,
                                           TransporterName = invoice.TransporterName,
                                           purchaseOrderDeliverSchedule = invoice.purchaseOrderDeliverSchedule == null ? (DateTime.Now - po.PurchaseOrderDate).TotalDays.ToString() : invoice.purchaseOrderDeliverSchedule,
                                           VendorBatchNo = invoice.VendorBatchNo,
                                           Manufacturer = invoice.Manufacturer,
                                           DeliveryNote = invoice.DeliveryNote,
                                           BillofLanding = invoice.BillofLanding,
                                           ManufacturerCode = invoice.ManufacturerCode,
                                       }
                                   };



            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                gateEntriesQuery = gateEntriesQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                gateEntriesQuery = gateEntriesQuery.Where(x => x.GatePassNo.Contains(input));
                return await gateEntriesQuery.ToListAsync() ?? default;
            }
            return default;
        }

        public async Task<List<GateEntryDto>> validateGateEntriesAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var gateEntriesQuery = from gateEntry in _gateEntryRepository.GetAll()
                                   join invoice in _invoiceDetailRepository.GetAll()
                                   on gateEntry.InvoiceId equals invoice.Id
                                   //join vi in _vehicleInspectionRepository.GetAll()
                                   //on gateEntry.InvoiceId equals vi.InvoiceId
                                   join po in _purchaseOrderRepository.GetAll()
                                   on invoice.PurchaseOrderId equals po.Id into ps
                                   from po in ps.DefaultIfEmpty()
                                   where gateEntry.IsActive
                                   orderby gateEntry.GatePassNo
                                   select new GateEntryDto
                                   {
                                       Id = gateEntry.Id,
                                       GatePassNo = gateEntry.GatePassNo,
                                       PlantId = po.PlantId,
                                       //TransactionstatusId = vi.TransactionStatusId,
                                       InvoiceDto = new InvoiceDto
                                       {
                                           Id = invoice.Id,
                                           PurchaseOrderId = invoice.PurchaseOrderId,
                                           PurchaseOrderNo = invoice.PurchaseOrderNo,
                                           VendorName = invoice.VendorName,
                                           VendorCode = invoice.VendorCode,
                                           LRDate = invoice.LRDate,
                                           LRNo = invoice.LRNo,
                                           InvoiceDate = invoice.InvoiceDate,
                                           InvoiceNo = invoice.InvoiceNo,
                                           DriverName = invoice.DriverName,
                                           VehicleNumber = invoice.VehicleNumber,
                                           TransporterName = invoice.TransporterName,
                                           purchaseOrderDeliverSchedule = invoice.purchaseOrderDeliverSchedule == null ? (DateTime.Now - po.PurchaseOrderDate).TotalDays.ToString() : invoice.purchaseOrderDeliverSchedule,
                                           VendorBatchNo = invoice.VendorBatchNo,
                                           Manufacturer = invoice.Manufacturer,
                                           DeliveryNote = invoice.DeliveryNote,
                                           BillofLanding = invoice.BillofLanding

                                       }
                                   };



            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                gateEntriesQuery = gateEntriesQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                gateEntriesQuery = gateEntriesQuery.Where(x => x.GatePassNo.Contains(input));
                return await gateEntriesQuery.ToListAsync() ?? default;
            }
            return default;
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

        public async Task<List<SelectListDto>> GetInvoiceByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            return await _invoiceDetailRepository.GetAll().Where(x => x.PurchaseOrderId == purchaseOrderId).OrderBy(x => x.InvoiceNo)
                    .Select(x => new SelectListDto { Id = x.Id, Value = x.InvoiceNo })?
                    .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetInvoiceByPurchaseOrderIdAutoCompleteAsync(int purchaseOrderId, string input)
        {
            var invoiceQuery = _invoiceDetailRepository.GetAll().Where(x => x.PurchaseOrderId == purchaseOrderId).OrderBy(x => x.InvoiceNo)
                     .Select(x => new SelectListDto { Id = x.Id, Value = x.InvoiceNo });
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                invoiceQuery = invoiceQuery.Where(x => x.Value.Contains(input));
                return await invoiceQuery.ToListAsync() ?? default;
            }
            return default;
        }

        public async Task<List<MaterialInternalDto>> GetMaterialByInvoiceIdAsync(int invoiceId)
        {
            var result = await (from materialheader in _materialInspectionheaderRepository.GetAll()
                                join materialInspectionDetail in _materialInspectionRelationalRepository.GetAll()
                                on materialheader.Id equals materialInspectionDetail.MaterialHeaderId
                                join material in _materialRepository.GetAll()
                                on materialInspectionDetail.MaterialId equals material.Id
                                where materialheader.InvoiceId == invoiceId && materialInspectionDetail.TransactionStatusId == (int)TransactionStatus.Accepted
                                orderby material.ItemCode
                                select new MaterialInternalDto { Id = material.Id, Code = material.ItemCode + "-" + material.ItemNo, UOM = material.UnitOfMeasurement }).ToListAsync() ?? default;
            return result;
        }

        public async Task<List<MaterialInternalDto>> GetMaterialWithMIDoneandGRNPendingAsync(int invoiceId)
        {
            var materials = await (from invoice in _invoiceDetailRepository.GetAll()
                                   join header in _materialInspectionHeaderRepository.GetAll()
                                   on invoice.Id equals header.InvoiceId
                                   join relation in _materialInspectionRelationalRepository.GetAll()
                                   on header.Id equals relation.MaterialHeaderId
                                   join consignment in _materialConsignmentRepository.GetAll()
                                   on relation.Id equals consignment.MaterialRelationId
                                   join material in _materialRepository.GetAll()
                                   on relation.MaterialId equals material.Id
                                   join grnDetail in _grnDetailRepository.GetAll()
                                   on consignment.Id equals grnDetail.MfgBatchNoId into gds
                                   from grnDetail in gds.DefaultIfEmpty()
                                   where invoice.Id == invoiceId && relation.TransactionStatusId == (int)PMMSEnums.TransactionStatus.Accepted
                                    && grnDetail.Id == null
                                   orderby material.ItemCode
                                   select new MaterialInternalDto
                                   {
                                       Id = material.Id,
                                       Code = material.ItemCode + "-" + material.ItemNo
                                   }).ToListAsync();

            return materials.GroupBy(t => t.Id).Select(a => new MaterialInternalDto
            {
                Id = a.FirstOrDefault().Id,
                Code = a.FirstOrDefault().Code
            }).ToList();
        }

        public async Task<List<GRNPostingDetailsDto>> GetMaterialDetailsWithMIDoneandGRNPendingAsync(int materialId, int invoiceId, int[] materialIds)
        {
            var materials = (from invoice in _invoiceDetailRepository.GetAll()
                             join header in _materialInspectionHeaderRepository.GetAll()
                             on invoice.Id equals header.InvoiceId
                             join relation in _materialInspectionRelationalRepository.GetAll()
                             on header.Id equals relation.MaterialHeaderId
                             join consignment in _materialConsignmentRepository.GetAll()
                             on relation.Id equals consignment.MaterialRelationId
                             join grnDetail in _grnDetailRepository.GetAll()
                             on consignment.Id equals grnDetail.MfgBatchNoId into gds
                             from grnDetail in gds.DefaultIfEmpty()
                             join material in _materialRepository.GetAll()
                             on relation.MaterialId equals material.Id
                             join uom in _unitOfMeasurementRepository.GetAll()
                             on consignment.UnitofMeasurementId equals uom.Id into uomps
                             from uom in uomps.DefaultIfEmpty()
                             where invoice.Id == invoiceId && relation.TransactionStatusId == (int)PMMSEnums.TransactionStatus.Accepted
                             && grnDetail.Id == null
                             orderby material.ItemCode
                             select new GRNPostingDetailsDto
                             {
                                 Id = grnDetail.Id,
                                 MaterialId = material.Id,
                                 MfgBatchNoId = consignment.Id,
                                 MaterialCode = material.ItemCode,
                                 ItemCode = material.ItemCode,
                                 ItemDescription = material.ItemDescription,
                                 InvoiceNo = invoice.InvoiceNo,
                                 ManufacturedBatchNo = consignment.ManufacturedBatchNo,
                                 ConsignmentQty = consignment.QtyAsPerInvoice.Value,
                                 InvoiceId = invoice.Id,
                                 ConsignmentQtyUnit = consignment.QtyAsPerInvoice.Value + " " + uom.UnitOfMeasurement,
                                 InvoiceQty = consignment.QtyAsPerInvoice.Value,
                                 MaterialConsignmentId = consignment.Id,
                                 MaterialRelationId = consignment.MaterialRelationId,
                                 IsDamaged = PMMSConsts.No,
                                 UOM = uom.UnitOfMeasurement,
                             }); ;
            if (materialIds.Length == 0)
            {
                materials = materials.Where(a => a.MaterialId == materialId);
            }
            var materialList = await materials.ToListAsync() ?? default;
            await GetDamagedMaterial(materialList, await materials.ToListAsync());
            return materialList;
        }

        private async Task GetDamagedMaterial(List<GRNPostingDetailsDto> materialList, List<GRNPostingDetailsDto> damageMaterialList)
        {
            foreach (var material in damageMaterialList)
            {
                var DamageMaterial = new GRNPostingDetailsDto();
                var damageMaterial = await _materialDamgeRepository.GetAll().Where(x => x.MaterialConsignmentId == material.MaterialConsignmentId && x.MaterialRelationId == material.MaterialRelationId).ToListAsync();
                if (damageMaterial.Any())
                {
                    DamageMaterial = material;
                    DamageMaterial.ConsignmentQty = damageMaterial.Select(x => x.Quantity).Sum();
                    DamageMaterial.ConsignmentQtyUnit = damageMaterial.Select(x => x.Quantity).Sum() + " " + material.UOM;
                    DamageMaterial.IsDamaged = PMMSConsts.Yes;
                    var a = materialList.FindIndex(a => a.MfgBatchNoId == DamageMaterial.MfgBatchNoId);
                    materialList.RemoveAt(a);
                    materialList.Add(DamageMaterial);
                }
            }
        }

        public async Task<List<SelectListDtoWithPlantId>> GetWeighingMachineAutoCompleteAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var weighingMachineQuery = _weighingMachineRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.WeighingMachineCode.ToLower() == input.ToLower()).OrderBy(x => x.WeighingMachineCode)
                    .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.WeighingMachineCode, PlantId = x.SubPlantId, LeastCountDigitAfterDecimal = x.LeastCountDigitAfterDecimal });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                weighingMachineQuery = weighingMachineQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await weighingMachineQuery.ToListAsync() ?? default;
        }

        /// <summary>
        /// This method will get the Weighing machine code,IP Address, and Port Number Details.
        /// </summary>
        /// <param name="input">input.</param>
        public async Task<WeighingMachineSelectWithDetailsDto> GetWeighingMachineDetailsAsync(string input, bool isWeightUOM)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var weighingMachineQuery = _weighingMachineRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.WeighingMachineCode.ToLower() == input.ToLower()).OrderBy(x => x.WeighingMachineCode)
                    .Select(x => new WeighingMachineSelectWithDetailsDto { Id = x.Id, WeighingMachineCode = x.WeighingMachineCode, SubPlantId = x.SubPlantId, PortNumber = x.PortNumber, IPAddress = x.IPAddress, LeastCountDigitAfterDecimal = x.LeastCountDigitAfterDecimal });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                weighingMachineQuery = weighingMachineQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            var weigingMachineDetails = await weighingMachineQuery.FirstOrDefaultAsync() ?? default;
            if (weigingMachineDetails != null && _configuration.GetValue<bool>(PMMSConsts.IsWeighigMachineEnabled) && isWeightUOM)
            {
                weigingMachineDetails.Weight = await GetWeightFromWeighingMachine(weigingMachineDetails.IPAddress, weigingMachineDetails.PortNumber);
                weigingMachineDetails.LeastCountDigitAfterDecimal = weighingMachineQuery.FirstOrDefault().LeastCountDigitAfterDecimal;
            }
            return weigingMachineDetails;
        }

        public async Task<List<MaterialConsignmentDto>> GetConsignmentByMaterialIdAsync(int materialId)
        {
            return await (from consignmentDetail in _materialConsignmentRepository.GetAll()
                          join materialRelation in _materialInspectionRelationalRepository.GetAll()
                          on consignmentDetail.MaterialRelationId equals materialRelation.Id
                          where materialRelation.MaterialId == materialId
                          orderby consignmentDetail.ManufacturedBatchNo
                          select new MaterialConsignmentDto
                          {
                              Id = consignmentDetail.Id,
                              ManufacturedBatchNo = consignmentDetail.ManufacturedBatchNo,
                              ManufacturedDate = consignmentDetail.ManufacturedDate,
                              ExpiryDate = consignmentDetail.ExpiryDate,
                              RetestDate = consignmentDetail.RetestDate,
                              UnitofMeasurementId = consignmentDetail.UnitofMeasurementId,
                          })?.ToListAsync() ?? default;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllPalletsBarcodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = (from handlingUnitMaster in _handlingUnitRepository.GetAll()
                                     join palletization in _palletizationRepository.GetAll()
                                     on handlingUnitMaster.Id equals palletization.PalletId
                                     where handlingUnitMaster.HUCode.ToLower() == input.ToLower() && !palletization.IsUnloaded
                                     && handlingUnitMaster.ApprovalStatusId == approvedApprovalStatusId && handlingUnitMaster.IsActive
                                     orderby handlingUnitMaster.HUCode
                                     select new SelectListDtoWithPlantId
                                     {
                                         Id = handlingUnitMaster.Id,
                                         Value = $"{handlingUnitMaster.HUCode} - {handlingUnitMaster.Name}",
                                         PlantId = handlingUnitMaster.PlantId
                                     });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await handlingUnitQuery.Distinct().ToListAsync() ?? default;
        }

        public async Task<List<MaterialBarcodePrintingDto>> GetAllMaterialSelectListDtoAsync(string searchText)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialQuery = (from grnDetail in _grnDetailRepository.GetAll()
                                 join invoice in _invoiceDetailRepository.GetAll()
                                 on grnDetail.InvoiceId equals invoice.Id
                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
                                 on invoice.PurchaseOrderId equals purchaseOrder.Id
                                 join labelheader in _grnMaterialLabelPrintingHeaderRepository.GetAll()
                                 on grnDetail.Id equals labelheader.GRNDetailId
                                 join labalBarcode in _grnMaterialLabelPrintingContainerBarcodeRepository.GetAll()
                                 on labelheader.Id equals labalBarcode.GRNMaterialLabelPrintingHeaderId
                                 where labalBarcode.MaterialLabelContainerBarCode.ToLower() == searchText.ToLower()
                                 orderby labalBarcode.MaterialLabelContainerBarCode
                                 select new MaterialBarcodePrintingDto
                                 {
                                     Id = grnDetail.MaterialId,
                                     MaterialBarcode = labalBarcode.MaterialLabelContainerBarCode,
                                     ContainerNumber = labalBarcode.ContainerNo,
                                     SAPBatchNumber = grnDetail.SAPBatchNumber,
                                     GRNDetailId = grnDetail.Id,
                                     ContainerId = labalBarcode.Id,
                                     PlantId = purchaseOrder.PlantId,
                                 });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                materialQuery = materialQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await materialQuery.ToListAsync() ?? default;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllLocationsBarcodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var locationQuery = _locationRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.IsActive && x.LocationCode.ToLower() == input.ToLower()).OrderBy(x => x.LocationCode)
                      .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.LocationCode, PlantId = x.PlantId });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                locationQuery = locationQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await locationQuery.ToListAsync() ?? default;
        }

        public async Task<BarcodeValidationDto> ValidateBinBarocde(int locationId, int transferTypeId)
        {
            var barcodeValidationDto = new BarcodeValidationDto();
            var bins = await _putAwayBinToBinTrasferRepository.GetAll().Where(x => x.LocationId == locationId && !x.IsUnloaded).ToListAsync();

            if (transferTypeId == putAwayMaterialToBin || transferTypeId == binToBinTranferMaterialToBin)
            {
                var validBins = bins.Where(a => a.MaterialTransferTypeId == putAwayPalletToBin || a.MaterialTransferTypeId == binToBinTranferPalletToBin);
                if (validBins.Any())
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = "Only Empty Bin && Half Empty Bin is allowed for scanning";
                    return barcodeValidationDto;
                }
                barcodeValidationDto.IsValid = true;
            }
            else
            {
                if (bins.Any())
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = "Only Empty Bin is allowed for scanning";
                    return barcodeValidationDto;
                }
                barcodeValidationDto.IsValid = true;
            }
            return barcodeValidationDto;
        }

        public async Task<BarcodeValidationDto> ValidatePalletBarocde(int palletId, int transferTypeId)
        {
            var barcodeValidationDto = new BarcodeValidationDto();
            var pallet = await _putAwayBinToBinTrasferRepository.GetAll().Where(x => x.PalletId == palletId && !x.IsUnloaded).ToListAsync();
            if (transferTypeId == putAwayPalletToBin)
            {
                if (pallet.Any())
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = PMMSValidationConst.PalletIsAlreadyMappedWithBin;
                    return barcodeValidationDto;
                }
                barcodeValidationDto.IsValid = true;
            }
            if (transferTypeId == binToBinTranferPalletToBin)
            {
                if (!pallet.Any())
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = PMMSValidationConst.PalletIsNotMappedWithBin;
                    return barcodeValidationDto;
                }
                barcodeValidationDto.IsValid = true;
            }
            return barcodeValidationDto;
        }

        public async Task<BarcodeValidationDto> ValidateMaterialBarocdeOnPalletization(MaterialBarcodePrintingDto currentMaterialAdded)
        {
            var barcodeValidationDto = new BarcodeValidationDto();

            var binMappedMaterials = await _putAwayBinToBinTrasferRepository.GetAll()
                .Where(x => !x.IsUnloaded && x.MaterialId == currentMaterialAdded.Id &&
                x.SAPBatchNumber == currentMaterialAdded.SAPBatchNumber && x.ContainerId == currentMaterialAdded.ContainerId)
                    .Select(putaway => new PutAwayBinToBinTransferDto
                    {
                        Id = putaway.Id,
                        MaterialId = putaway.MaterialId,
                        SAPBatchNumber = putaway.SAPBatchNumber,
                        ContainerId = putaway.ContainerId,
                    }).Distinct().ToListAsync();

            if (binMappedMaterials.Count() > 0)
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.MaterialIsAlreadyMappedWithBin;
                return barcodeValidationDto;
            }
            barcodeValidationDto.IsValid = true;
            return barcodeValidationDto;
        }

        public async Task<BarcodeValidationDto> ValidateMaterialBarcode(MaterialBarcodePrintingDto currentMaterialAdded, int locationId, int transferTypeId)
        {
            var barcodeValidationDto = new BarcodeValidationDto();
            if (transferTypeId == putAwayMaterialToBin || transferTypeId == binToBinTranferMaterialToBin)
            {
                var palletMappedmaterials = await (from pallet in _palletizationRepository.GetAll()
                                                   where pallet.MaterialId == currentMaterialAdded.Id && pallet.ContainerId == currentMaterialAdded.ContainerId
                                                   && !pallet.IsUnloaded
                                                   select pallet).ToListAsync();
                if (palletMappedmaterials.Any())
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = PMMSValidationConst.PalletMappedMaterialNotAllowed;
                    return barcodeValidationDto;
                }
                if (transferTypeId == binToBinTranferMaterialToBin)
                {
                    var binMappedMaterials = await _putAwayBinToBinTrasferRepository.GetAll().Where(x => x.MaterialId == currentMaterialAdded.Id
                    && x.ContainerId == currentMaterialAdded.ContainerId && !x.IsUnloaded).ToListAsync();

                    if (binMappedMaterials.Any())
                    {
                        barcodeValidationDto.IsValid = false;
                        barcodeValidationDto.ValidationMessage = PMMSValidationConst.MaterialIsNotMappedWithBin;
                        return barcodeValidationDto;
                    }
                }
                var materials = await _putAwayBinToBinTrasferRepository.GetAll().Where(x => x.LocationId == locationId && !x.IsUnloaded).
                                  Select(putaway => new PutAwayBinToBinTransferDto
                                  {
                                      Id = putaway.Id,
                                      MaterialId = putaway.MaterialId,
                                      SAPBatchNumber = putaway.SAPBatchNumber,
                                      ContainerId = putaway.ContainerId,
                                  }).Distinct().ToListAsync();

                var partitions = materials.GroupBy(p => new { p.MaterialId, p.SAPBatchNumber });
                if (partitions.Count() >= 6)
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = PMMSValidationConst.PartitionNotValid;
                    return barcodeValidationDto;
                }

                var isBarcodePresent = await _putAwayBinToBinTrasferRepository.GetAll().AnyAsync(a => a.MaterialId == currentMaterialAdded.Id &&
                                                                                                 a.SAPBatchNumber == currentMaterialAdded.SAPBatchNumber
                                                                                                 && a.ContainerId == currentMaterialAdded.ContainerId
                                                                                                 && !a.IsUnloaded);
                if (isBarcodePresent)
                {
                    barcodeValidationDto.IsValid = false;
                    barcodeValidationDto.ValidationMessage = PMMSValidationConst.DuplicateMaterialBarcode;
                    return barcodeValidationDto;
                }
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
            barcodeValidationDto.IsValid = true;
            return barcodeValidationDto;
        }

        public async Task<List<UnitOfMeasurementListDto>> GetUnitOfMeasurementDetailsByIdAsync(int uomId)
        {
            var uomQuery = (from unitOfMeasurementMaster in _unitOfMeasurementRepository.GetAll()
                            join uomType in _unitOfMeasurementTypeMasterRepository.GetAll()
                            on unitOfMeasurementMaster.UnitOfMeasurementTypeId equals uomType.Id
                            where unitOfMeasurementMaster.ApprovalStatusId == approvedApprovalStatusId && unitOfMeasurementMaster.IsActive && unitOfMeasurementMaster.Id == uomId
                            orderby unitOfMeasurementMaster.UOMCode
                            select new UnitOfMeasurementListDto
                            {
                                Id = unitOfMeasurementMaster.Id,
                                UnitOfMeasurement = unitOfMeasurementMaster.UOMCode + " - " + unitOfMeasurementMaster.Name,
                                UserEnteredUOMType = uomType.UnitOfMeasurementTypeName,
                                UnitOfMeasurementTypeId = uomType.Id,
                                ActualUom = unitOfMeasurementMaster.Name,
                            });
            return await uomQuery.ToListAsync() ?? default;
        }
        public async Task<double> GetWeightFromWeighingMachine(string ipAddress, int? portNumber)
        {
            var weighingMachineType = _weighingScaleFactory.GetPrintConnector(WeighingScaleType.Normal);
            var weighingMachineInput = new WeighingMachineInput
            {
                IPAddress = ipAddress,
                Port = Convert.ToInt32(portNumber),
            };
            return await weighingMachineType.GetWeight(weighingMachineInput);
        }

        public async Task<double> GetWeightForTesting(string ipAddress, int portNumber)
        {
            var weighingMachineType = _weighingScaleFactory.GetPrintConnector(WeighingScaleType.Normal);
            return await weighingMachineType.GetWeightForTesting(ipAddress, portNumber);
        }

        public async Task<List<PurchaseOrderInternalDto>> GetVehicleInspectionPurchaseOrdersAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var purchaseOrderQuery = from purchaseOrder in _purchaseOrderRepository.GetAll()
                                     join invoiceDetail in _invoiceDetailRepository.GetAll()
                                     on purchaseOrder.Id equals invoiceDetail.PurchaseOrderId
                                     select
                                     new PurchaseOrderInternalDto
                                     {
                                         PlantId = purchaseOrder.PlantId,
                                         Id = purchaseOrder.Id,
                                         PurchaseOrderNo = purchaseOrder.PurchaseOrderNo
                                     };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                purchaseOrderQuery = purchaseOrderQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }

            return await purchaseOrderQuery?.ToListAsync() ?? default;
        }
        public async Task<List<PurchaseOrderInternalDto>> GetGateEntryPurchaseOrdersAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var purchaseOrderQuery = from purchaseOrder in _purchaseOrderRepository.GetAll()
                                     join invoiceDetail in _invoiceDetailRepository.GetAll()
                                     on purchaseOrder.Id equals invoiceDetail.PurchaseOrderId
                                     join gateEntry in _gateEntryRepository.GetAll()
                                     on invoiceDetail.Id equals gateEntry.InvoiceId
                                     select new PurchaseOrderInternalDto
                                     {
                                         PlantId = purchaseOrder.PlantId,
                                         Id = purchaseOrder.Id,
                                         PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,

                                     };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                purchaseOrderQuery = purchaseOrderQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await purchaseOrderQuery.Distinct()?.ToListAsync() ?? default;
        }
    }
}