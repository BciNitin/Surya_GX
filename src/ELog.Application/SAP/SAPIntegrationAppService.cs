using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using ELog.Application.SAP.Dto;
using ELog.Application.SAP.MaterialMaster.Dto;
using ELog.Application.SAP.ProcessOrder.Dto;
using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.SAP
{
    //[PMMSAuthorize]
    public class SAPIntegrationAppService : ApplicationService, ISAPIntegrationAppService
    {
        private readonly IRepository<SAPProcessOrderReceivedMaterial> _sapPOReceivedMaterialRepository;
        private readonly IRepository<SAPQualityControlDetail> _sapQualityControlDetailRepository;
        private readonly IRepository<Core.Entities.SAPProcessOrder> _sapProcessOrderRepository;
        private readonly IRepository<Core.Entities.SAPReturntoMaterial> _sapReturnToMaterialRepository;
        private readonly IRepository<SAPUOMMaster> _sapUOMMasterRepository;
        private readonly IRepository<SAPPlantMaster> _sapPlantMasterRepository;
        private readonly IRepository<Core.Entities.MaterialMaster> _materialMasterRepository;
        private readonly IRepository<PlantMaster> _plantMasterRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _uomMasterRepository;
        private readonly IRepository<CountryMaster> _countryRepository;
        private readonly IRepository<StateMaster> _stateRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<Core.Entities.ProcessOrder> _processOrderRepository;
        private readonly IRepository<Core.Entities.PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<Core.Entities.Material> _materialRepository;
        private readonly IRepository<MaterialInspectionHeader> _materialInspectionHeaderRepository;
        private readonly IRepository<MaterialInspectionRelationDetail> _materialInspectionRelationDetailRepository;
        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentDetailRepository;
        private readonly IRepository<GRNQtyDetail> _grnQtyDetailRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;

        private const string masterPlantSuffix = "_Master";
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public SAPIntegrationAppService(IRepository<SAPProcessOrderReceivedMaterial> sapPOReceivedMaterialRepository,
            IRepository<SAPQualityControlDetail> sapQualityControlDetailRepository, IRepository<Core.Entities.SAPProcessOrder> sapProcessOrderRepository,
            IRepository<Core.Entities.SAPReturntoMaterial> sapReturnToMaterialRepository, IRepository<PlantMaster> plantMasterRepository, IRepository<Core.Entities.ProcessOrder> processOrderRepository,
            IRepository<SAPUOMMaster> sapUOMMasterRepository, IRepository<SAPPlantMaster> sapPlantMasterRepository, IRepository<UnitOfMeasurementMaster> uomMasterRepository,
            IRepository<Core.Entities.MaterialMaster> materialMasterRepository, IRepository<Core.Entities.PurchaseOrder> purchaseOrderRepository,
            IRepository<CountryMaster> countryRepository, IRepository<StateMaster> stateRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
            IRepository<Core.Entities.Material> materialRepository, IRepository<MaterialInspectionHeader> materialInspectionHeaderRepository,
            IRepository<MaterialInspectionRelationDetail> materialInspectionRelationDetailRepository, IRepository<MaterialConsignmentDetail> materialConsignmentDetailRepository,
            IRepository<GRNHeader> grnHeaderRepository, IRepository<GRNDetail> grnDetailRepository, IRepository<GRNQtyDetail> grnQtyDetailRepository)
        {
            _sapPOReceivedMaterialRepository = sapPOReceivedMaterialRepository;
            _sapQualityControlDetailRepository = sapQualityControlDetailRepository;
            _sapProcessOrderRepository = sapProcessOrderRepository;
            _sapReturnToMaterialRepository = sapReturnToMaterialRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _sapUOMMasterRepository = sapUOMMasterRepository;
            _sapPlantMasterRepository = sapPlantMasterRepository;
            _materialMasterRepository = materialMasterRepository;
            _plantMasterRepository = plantMasterRepository;
            _uomMasterRepository = uomMasterRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _processOrderRepository = processOrderRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _materialRepository = materialRepository;
            _materialInspectionHeaderRepository = materialInspectionHeaderRepository;
            _materialInspectionRelationDetailRepository = materialInspectionRelationDetailRepository;
            _materialConsignmentDetailRepository = materialConsignmentDetailRepository;
            _grnQtyDetailRepository = grnQtyDetailRepository;
            _grnDetailRepository = grnDetailRepository;
        }

        public async Task InsertUpdateMaterialAsync(SAPMaterial input)
        {
            await InsertUpdateMaterials(input);
        }

        private async Task InsertUpdateMaterials(SAPMaterial input)
        {
            var existingmaterial = await _materialMasterRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialCode.Trim().ToLower() == input.MaterialCode.Trim().ToLower());
            var material = ObjectMapper.Map<ELog.Core.Entities.MaterialMaster>(input);
            if (existingmaterial != null)
            {
                existingmaterial.BaseUOM = material.BaseUOM;
                existingmaterial.Numerator = material.Numerator;
                existingmaterial.Denominator = material.Denominator;
                existingmaterial.MaterialDescription = material.MaterialDescription;
                existingmaterial.Grade = material.Grade;
                existingmaterial.ConversionUOM = material.ConversionUOM;
                existingmaterial.MaterialType = material.MaterialType;
                existingmaterial.Flag = material.Flag;
                await _materialMasterRepository.UpdateAsync(existingmaterial);
            }
            else
            {
                await _materialMasterRepository.InsertAsync(material);
            }
        }

        public async Task InsertUpdateMaterialsAsync(SAPMaterials input)
        {
            if (input.Record?.Count > 0)
            {
                var materials = input.Record.Where(x => x.MaterialCode != "Dummy");
                foreach (var mat in materials)
                {
                    await InsertUpdateMaterials(mat);
                }
            }
        }

        public async Task<SAPMaterial> GetMaterialAsync(string materialCode)
        {
            var existingmaterial = await _materialMasterRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialCode.ToLower() == materialCode);
            var material = ObjectMapper.Map<SAPMaterial>(existingmaterial);
            return material;
        }

        public async Task InsertUpdatePlantMasterAsync(SAPPlantMasterDto input)
        {
            if (!string.IsNullOrEmpty(input.State))
            {
                await ValidateStateAsync(input);
            }
            if (!string.IsNullOrEmpty(input.Country))
            {
                await ValidateCountryAsync(input);
            }
            var existingPlantMaster = await _sapPlantMasterRepository.FirstOrDefaultAsync(x => x.PlantCode.ToLower() == input.PlantCode.ToLower());
            if (existingPlantMaster != null)
            {
                ObjectMapper.Map(input, existingPlantMaster);
                await _sapPlantMasterRepository.UpdateAsync(existingPlantMaster);
                await UpdatePlantMasterAsync(input);
            }
            else
            {
                var sapPlantMaster = ObjectMapper.Map<SAPPlantMaster>(input);
                await _sapPlantMasterRepository.InsertAsync(sapPlantMaster);
                await CreatePlantMasterAsync(input);
            }

            CurrentUnitOfWork.SaveChanges();
        }

        public async Task InsertUpdateUomMasterAsync(SAPUOMMasterDto input)
        {
            var existinguom = await _sapUOMMasterRepository.FirstOrDefaultAsync(x => x.UOM.ToLower() == input.UOM.ToLower());
            var sapUomMaster = ObjectMapper.Map<SAPUOMMaster>(input);
            if (existinguom != null)
            {
                existinguom.Description = sapUomMaster.Description;
                await _sapUOMMasterRepository.UpdateAsync(existinguom);
            }
            else
            {
                await _sapUOMMasterRepository.InsertAsync(sapUomMaster);
            }
            await CreateUpdateUomMasterAsync(input);
            CurrentUnitOfWork.SaveChanges();
        }

        public async Task InsertUpdateProcessOrderAsync(SAPProcessOrderDto input)
        {
            var existingProcessOrder = await _sapProcessOrderRepository.GetAll().FirstOrDefaultAsync(x => x.ProcessOrderNo.ToLower() == input.ProcessOrderNo.ToLower()
            && x.LineItemNo.ToLower() == input.LineItemNo.ToLower());
            var processOrder = ObjectMapper.Map<ELog.Core.Entities.SAPProcessOrder>(input);
            int plantId = await GetPlantIdIfAlreadyExist(input.Plant);
            if (plantId == 0 || plantId < 0)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.SAPPlantNotFound);
            }
            var materialMasterData = await _materialMasterRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialCode.Trim().ToLower() == input.MaterialCode.Trim().ToLower());
            if (materialMasterData == null)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.MaterialNotFound);
            }
            var materialMasterFlag = materialMasterData.Flag;
            if (existingProcessOrder != null)
            {
                existingProcessOrder.ProcessOrderNo = processOrder.ProcessOrderNo;
                existingProcessOrder.LineItemNo = processOrder.LineItemNo;
                existingProcessOrder.MaterialCode = processOrder.MaterialCode;
                existingProcessOrder.MaterialDescription = processOrder.MaterialDescription;
                existingProcessOrder.ARNo = processOrder.ARNo;
                existingProcessOrder.SAPBatchNo = processOrder.SAPBatchNo;
                existingProcessOrder.ProductCode = processOrder.ProductCode;
                existingProcessOrder.ProductBatchNo = processOrder.ProductBatchNo;
                existingProcessOrder.ReqDispensedQty = processOrder.ReqDispensedQty;
                existingProcessOrder.UOM = processOrder.UOM;
                existingProcessOrder.BaseUOM = processOrder.BaseUOM;
                existingProcessOrder.BaseQty = processOrder.BaseQty;
                existingProcessOrder.CurrentStage = processOrder.CurrentStage;
                existingProcessOrder.NextStage = processOrder.NextStage;
                existingProcessOrder.DispensingUOM = processOrder.DispensingUOM;
                existingProcessOrder.DispensingQty = processOrder.DispensingQty;
                existingProcessOrder.Plant = processOrder.Plant;
                existingProcessOrder.StorageLocation = processOrder.StorageLocation;
                existingProcessOrder.IsReservationNo = processOrder.IsReservationNo;
                await _sapProcessOrderRepository.UpdateAsync(existingProcessOrder);
                await CreateOrUpdateProcessOrderAsync(input, plantId, materialMasterFlag);
            }
            else
            {
                await _sapProcessOrderRepository.InsertAsync(processOrder);
                await CreateOrUpdateProcessOrderAsync(input, plantId, materialMasterFlag);
            }
        }

        public async Task InsertUpdateReturnMaterialAsync(SAPReturntoMaterialDto input)
        {
            var existingMaterial = await _sapReturnToMaterialRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialDocumentNo.Trim().ToLower() == input.MaterialDocumentNo.Trim().ToLower()
            && x.LineItemNo.Trim().ToLower() == input.LineItemNo.Trim().ToLower());
            var material = ObjectMapper.Map<ELog.Core.Entities.SAPReturntoMaterial>(input);
            if (existingMaterial != null)
            {
                existingMaterial.MaterialDocumentNo = material.MaterialDocumentNo;
                existingMaterial.MaterialDocumentYear = material.MaterialDocumentYear;
                existingMaterial.ItemCode = material.ItemCode;
                existingMaterial.LineItemNo = material.LineItemNo;
                existingMaterial.MaterialDescription = material.MaterialDescription;
                existingMaterial.SAPBatchNo = material.SAPBatchNo;
                existingMaterial.Qty = material.Qty;
                existingMaterial.UOM = material.UOM;
                await _sapReturnToMaterialRepository.UpdateAsync(existingMaterial);
            }
            else
            {
                await _sapReturnToMaterialRepository.InsertAsync(material);
            }
        }

        public async Task<string> InsertUpdatePurchaseOrderMaterial(SAPProcessOrderReceivedMaterialDto input)
        {
            int plantId = await GetPlantIdIfAlreadyExist(input.Plant);
            if (plantId == 0 || plantId < 0)
            {
                return PMMSValidationConst.SAPPlantNotFound;
            }
            var existingMaterial = await _sapPOReceivedMaterialRepository.GetAll().FirstOrDefaultAsync(x => x.PONo.ToLower() == input.PONo.ToLower()
            && x.LineItemNo.ToLower() == input.LineItemNo.ToLower());
            var sapProcessOrderReceivedMaterial = ObjectMapper.Map<SAPProcessOrderReceivedMaterial>(input);
            var poDate = input.PODate.Replace("-", "");
            var dt = DateTime.ParseExact(poDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            sapProcessOrderReceivedMaterial.PODate = dt;
            if (existingMaterial != null)
            {
                ObjectMapper.Map(input, existingMaterial);
                await _sapPOReceivedMaterialRepository.UpdateAsync(existingMaterial);
                await CreateOrUpdatePurchaseOrderAsync(input, plantId);
                return PMMSValidationConst.SAPPOMaterialUpdated;
            }
            else
            {
                await _sapPOReceivedMaterialRepository.InsertAsync(sapProcessOrderReceivedMaterial);
                await CreateOrUpdatePurchaseOrderAsync(input, plantId);
                return PMMSValidationConst.SAPPOMaterialReceived;
            }
        }

        public async Task<string> InsertUpdateQualityControlDetail(SAPQualityControlDetailDto input)
        {
            var material = await _materialMasterRepository.FirstOrDefaultAsync(x => x.MaterialCode == input.ItemCode);
            if (material == null)
            {
                return PMMSValidationConst.MaterilNotExist;
            }

            var grnMaterial = await (from item in _materialRepository.GetAll()
                                     join grndetail in _grnDetailRepository.GetAll()
                                     on item.Id equals grndetail.MaterialId
                                     join qty in _grnQtyDetailRepository.GetAll()
                                     on grndetail.Id equals qty.GRNDetailId
                                     join consignment in _materialConsignmentDetailRepository.GetAll()
                                     on grndetail.MfgBatchNoId equals consignment.Id
                                     join purchaseOrder in _purchaseOrderRepository.GetAll()
                                     on item.PurchaseOrderId equals purchaseOrder.Id
                                     where item.ItemCode == material.MaterialCode && grndetail.SAPBatchNumber == input.SAPBatchNo
                                     select new QCMaterialDto
                                     {
                                         ItemCode = input.ItemCode,
                                         SAPBatchNo = input.SAPBatchNo,
                                         RetestDate = Convert.ToDateTime(consignment.RetestDate),
                                         ExpiryDate = Convert.ToDateTime(consignment.ExpiryDate),
                                         UnitOfMeasurement = item.UnitOfMeasurement,
                                         BatchNo = grndetail.SAPBatchNumber,
                                         ItemNo = item.ItemNo,
                                         ItemDescription = item.ItemDescription,
                                         OrderQuantity = qty.TotalQty,
                                     }).ToListAsync();

            grnMaterial = grnMaterial.GroupBy(a => a.SAPBatchNo).Select(a => new QCMaterialDto
            {
                ItemCode = a.First().ItemCode,
                SAPBatchNo = a.First().SAPBatchNo,
                RetestDate = a.First().RetestDate,
                ExpiryDate = a.First().ExpiryDate,
                UnitOfMeasurement = a.First().UnitOfMeasurement,
                BatchNo = a.First().BatchNo,
                ItemNo = a.First().ItemNo,
                ItemDescription = a.First().ItemDescription,
                OrderQuantity = a.Sum(a => a.OrderQuantity),
                GRNHeaderId = a.First().GRNHeaderId
            }).ToList();
            if (grnMaterial.Count() == 0)
            {
                return PMMSValidationConst.GRNNotCreatedForQCMaterial;
            }

            var plantId = await (from item in _materialRepository.GetAll()
                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
                                 on item.PurchaseOrderId equals purchaseOrder.Id
                                 where item.Id == material.Id
                                 select purchaseOrder.PlantId).FirstOrDefaultAsync();

            var existingQCDetails = await _sapQualityControlDetailRepository.GetAll().FirstOrDefaultAsync(x => x.InspectionlotNo.Trim().ToLower() == input.InspectionlotNo.Trim().ToLower() && x.ItemCode.Trim().ToLower() == input.ItemCode.Trim().ToLower()
                                    && x.ItemCode.ToLower() == input.ItemCode.ToLower());
            var sapQualityControlDetail = ObjectMapper.Map<SAPQualityControlDetail>(input);
            var date = input.RetestDate.Replace("-", "");
            var dt = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            sapQualityControlDetail.RetestDate = dt;

            if (existingQCDetails != null)
            {
                ObjectMapper.Map(input, existingQCDetails);
                existingQCDetails.RetestDate = dt;
                if (!string.IsNullOrEmpty(input.ReleasedOn))
                {
                    existingQCDetails.ReleasedOn = DateTime.ParseExact(input.ReleasedOn.Replace("-", ""), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                }
                await _sapQualityControlDetailRepository.UpdateAsync(existingQCDetails);
                return PMMSValidationConst.SAPQualityControlDetailUpdated;
            }
            else
            {
                if (!string.IsNullOrEmpty(input.ReleasedOn))
                {
                    sapQualityControlDetail.ReleasedOn = DateTime.ParseExact(input.ReleasedOn.Replace("-", ""), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                }
                await _sapQualityControlDetailRepository.InsertAsync(sapQualityControlDetail);
                return PMMSValidationConst.SAPQualityControlDetailSaved;
            }
        }
        private async Task CreatePlantMasterAsync(SAPPlantMasterDto input)
        {
            //Insert Master Plant
            var plantMaster = ObjectMapper.Map<PlantMaster>(input);
            plantMaster.PlantTypeId = (int)PlantType.MasterPlant;
            plantMaster.PlantId += masterPlantSuffix;
            plantMaster.ApprovalStatusId = (int)ApprovalStatus.Approved;
            plantMaster.IsActive = true;
            plantMaster.TenantId = AbpSession.TenantId;
            var masterPlantId = await _plantMasterRepository.InsertAndGetIdAsync(plantMaster);
            //To Insert SUBPlant
            var subPlantMaster = ObjectMapper.Map<PlantMaster>(input);
            subPlantMaster.PlantTypeId = (int)PlantType.SubPlant;
            subPlantMaster.MasterPlantId = masterPlantId;
            subPlantMaster.IsActive = true;
            subPlantMaster.ApprovalStatusId = (int)ApprovalStatus.Approved;
            subPlantMaster.TenantId = AbpSession.TenantId;
            await _plantMasterRepository.InsertAsync(subPlantMaster);
        }

        private async Task UpdatePlantMasterAsync(SAPPlantMasterDto input)
        {
            var existingPlantMaster = await _plantMasterRepository.FirstOrDefaultAsync(x => x.PlantId.ToLower() == input.PlantCode.ToLower());
            if (existingPlantMaster != null)
            {
                ObjectMapper.Map(input, existingPlantMaster);
                await _plantMasterRepository.UpdateAsync(existingPlantMaster);
                var masterPlant = await _plantMasterRepository.FirstOrDefaultAsync(x => x.PlantId.ToLower() == (input.PlantCode + masterPlantSuffix).ToLower());
                if (masterPlant != null)
                {
                    ObjectMapper.Map(input, masterPlant);
                    masterPlant.IsActive = true;
                    await _plantMasterRepository.UpdateAsync(masterPlant);
                }
            }
        }

        private async Task CreateUpdateUomMasterAsync(SAPUOMMasterDto input)
        {
            var existingUOM = await _uomMasterRepository.FirstOrDefaultAsync(x => x.UnitOfMeasurement.ToLower() == input.UOM.ToLower());
            if (existingUOM != null)
            {
                existingUOM.Description = input.Description;
                existingUOM.UnitOfMeasurementTypeId = (int)UOMType.Weight;
                existingUOM.UOMCode = input.UOM;
                await _uomMasterRepository.UpdateAsync(existingUOM);
            }
            else
            {
                var uomMaster = new UnitOfMeasurementMaster()
                {
                    Description = input.Description,
                    UnitOfMeasurement = input.UOM,
                    UOMCode = input.UOM,
                    UnitOfMeasurementTypeId = (int)UOMType.Weight,
                    ApprovalStatusId = (int)ApprovalStatus.Approved,
                    IsActive = true,
                };
                await _uomMasterRepository.InsertAsync(uomMaster);
            }
        }

        private async Task CreateOrUpdateProcessOrderMaterialsAsync(SAPProcessOrderDto input, int processOrderId, int? materialflag)
        {
            var existingPoMaterials = await _processOrderMaterialRepository.FirstOrDefaultAsync(x => x.ItemCode.ToLower() == input.MaterialCode.ToLower() &&
              x.ItemNo.ToLower() == input.LineItemNo.ToLower());
            if (existingPoMaterials != null)
            {
                existingPoMaterials.ItemNo = input.LineItemNo;
                existingPoMaterials.ItemCode = input.MaterialCode;
                existingPoMaterials.ARNo = input.ARNo;
                existingPoMaterials.ItemDescription = input.MaterialDescription;
                existingPoMaterials.SAPBatchNo = input.SAPBatchNo;
                existingPoMaterials.BatchNo = input.ProductBatchNo;
                existingPoMaterials.UnitOfMeasurement = input.UOM;
                existingPoMaterials.OrderQuantity = (float?)input.ReqDispensedQty;
                existingPoMaterials.RetestDate = DateTime.Now.AddDays(materialflag ?? 0);
                existingPoMaterials.ExpiryDate = DateTime.Now.AddDays(materialflag ?? 0);
                existingPoMaterials.ProcessOrderId = processOrderId;
                existingPoMaterials.ProcessOrderNo = input.ProcessOrderNo;
                existingPoMaterials.TenantId = AbpSession.TenantId;
                await _processOrderMaterialRepository.UpdateAsync(existingPoMaterials);
            }
            else
            {
                var processOrderMaterial = new ProcessOrderMaterial()
                {
                    ItemNo = input.LineItemNo,
                    ItemCode = input.MaterialCode,
                    ARNo = input.ARNo,
                    ItemDescription = input.MaterialDescription,
                    SAPBatchNo = input.SAPBatchNo,
                    BatchNo = input.ProductBatchNo,
                    UnitOfMeasurement = input.UOM,
                    ProcessOrderId = processOrderId,
                    ProcessOrderNo = input.ProcessOrderNo,
                    OrderQuantity = (float?)input.ReqDispensedQty,
                    RetestDate = DateTime.Now.AddDays(materialflag ?? 0),
                    ExpiryDate = DateTime.Now.AddDays(materialflag ?? 0),
                    TenantId = AbpSession.TenantId,
                };
                await _processOrderMaterialRepository.InsertAsync(processOrderMaterial);
            }
        }

        private async Task CreateOrUpdateProcessOrderAsync(SAPProcessOrderDto input, int plantId, int? materialFlag)
        {
            var existingProcessOrder = await _processOrderRepository.FirstOrDefaultAsync(x => x.ProcessOrderNo.ToLower() == input.ProcessOrderNo.ToLower());
            if (existingProcessOrder != null)
            {
                existingProcessOrder.ProcessOrderNo = input.ProcessOrderNo;
                existingProcessOrder.ProductCode = input.ProductCode;
                existingProcessOrder.PlantId = plantId;
                existingProcessOrder.IsReservationNo = input.IsReservationNo;
                await _processOrderRepository.UpdateAsync(existingProcessOrder);
                await CreateOrUpdateProcessOrderMaterialsAsync(input, existingProcessOrder.Id, materialFlag);
            }
            else
            {
                var processOrder = new Core.Entities.ProcessOrder()
                {
                    ProcessOrderNo = input.ProcessOrderNo,
                    ProductCode = input.ProductCode,
                    ProcessOrderDate = DateTime.UtcNow,
                    PlantId = plantId,
                    IsReservationNo = input.IsReservationNo
                };
                var processOrderId = await _processOrderRepository.InsertAndGetIdAsync(processOrder);
                await CreateOrUpdateProcessOrderMaterialsAsync(input, processOrderId, materialFlag);
            }
        }

        private async Task CreateOrUpdatePurchaseOrderAsync(SAPProcessOrderReceivedMaterialDto input, int plantId)
        {
            var existingPurchessOrder = await _purchaseOrderRepository.FirstOrDefaultAsync(x => x.PurchaseOrderNo.ToLower() == input.PONo.ToLower());
            var poDate = input.PODate.Replace("-", "");
            var dt = DateTime.ParseExact(poDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            if (existingPurchessOrder != null)
            {
                existingPurchessOrder.PurchaseOrderNo = input.PONo;
                existingPurchessOrder.PurchaseOrderDate = dt;
                existingPurchessOrder.PlantId = plantId;
                existingPurchessOrder.VendorName = input.VendorName;
                existingPurchessOrder.VendorCode = input.VendorCode;
                await _purchaseOrderRepository.UpdateAsync(existingPurchessOrder);
                await CreateOrUpdateMaterialsAsync(input, existingPurchessOrder.Id);
            }
            else
            {
                var purchessOrder = new Core.Entities.PurchaseOrder()
                {
                    PurchaseOrderNo = input.PONo,
                    PurchaseOrderDate = dt,
                    PlantId = plantId,
                    VendorName = input.VendorName,
                    VendorCode = input.VendorCode,
                };
                var purchaseOrderId = await _purchaseOrderRepository.InsertAndGetIdAsync(purchessOrder);
                await CreateOrUpdateMaterialsAsync(input, purchaseOrderId);
            }
        }

        private async Task CreateOrUpdateMaterialsAsync(SAPProcessOrderReceivedMaterialDto input, int purchaseOrderId)
        {
            var existingMaterials = await _materialRepository.FirstOrDefaultAsync(x => x.PurchaseOrderNo.ToLower() == input.PONo.ToLower() && x.ItemNo.ToLower() == input.LineItemNo.ToLower());
            if (existingMaterials != null)
            {
                existingMaterials.ItemNo = input.LineItemNo;
                existingMaterials.OrderQuantity = (float)input.OrderQty;
                existingMaterials.UnitOfMeasurement = input.UOM;
                existingMaterials.ItemDescription = input.ItemDescription;
                existingMaterials.ItemCode = input.ItemCode;
                existingMaterials.PurchaseOrderNo = input.PONo;
                existingMaterials.UnitOfMeasurement = input.UOM;
                existingMaterials.TenantId = AbpSession.TenantId;
                existingMaterials.ManufacturerCode = input.ManufacturerCode;
                existingMaterials.ManufacturerName = input.ManufacturerName;
                await _materialRepository.UpdateAsync(existingMaterials);
            }
            else
            {
                var material = new Material()
                {
                    ItemNo = input.LineItemNo,
                    OrderQuantity = (float)input.OrderQty,
                    UnitOfMeasurement = input.UOM,
                    PurchaseOrderId = purchaseOrderId,
                    PurchaseOrderNo = input.PONo,
                    ItemCode = input.ItemCode,
                    ItemDescription = input.ItemDescription,
                    ManufacturerCode = input.ManufacturerCode,
                    ManufacturerName = input.ManufacturerName,
                    TenantId = AbpSession.TenantId,
                };
                await _materialRepository.InsertAsync(material);
            }

        }



        private async Task ValidateCountryAsync(SAPPlantMasterDto input)
        {
            var countryId = await _countryRepository.GetAll().Where(x => x.CountryName.ToLower() == input.Country.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
            if (countryId != 0)
            {
                input.Country = countryId.ToString();
            }
            else
            {
                throw new UserFriendlyException("Please enter valid country name");
            }
        }

        private async Task ValidateStateAsync(SAPPlantMasterDto input)
        {
            var stateId = await _stateRepository.GetAll().Where(x => x.StateName.ToLower() == input.State.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
            if (stateId != 0)
            {
                input.State = stateId.ToString();
            }
            else
            {
                throw new UserFriendlyException("Please enter valid state name");
            }
        }

        private async Task<int> GetPlantIdIfAlreadyExist(string plantId)
        {
            var plantIdData = plantId.Trim().ToLower();
            return await _plantMasterRepository.GetAll().Where(x => x.PlantId.ToLower() == plantIdData).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}