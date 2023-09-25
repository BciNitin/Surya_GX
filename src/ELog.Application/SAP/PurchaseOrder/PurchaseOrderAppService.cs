using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.SAP.PurchaseOrder
{
    public class PurchaseOrderAppService : ApplicationService, IPurchaseOrderAppService
    {
        private readonly IRepository<ELog.Core.Entities.PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<InvoiceDetail> _invoiceRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public PurchaseOrderAppService(IRepository<ELog.Core.Entities.PurchaseOrder> purchaseOrderRepository,
            IRepository<Material> materialRepository,
            IRepository<PlantMaster> plantRepository, IRepository<InvoiceDetail> invoiceRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _plantRepository = plantRepository;
            _materialRepository = materialRepository;
            _invoiceRepository = invoiceRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task<string> UpdateAsync(PurchaseOrderDto input)
        {
            var plantId = await GetPlantIdIfAlreadyExist(input);
            if (plantId > 0)
            {
                var purchaseOrderId = await InsertOrUpdatePurchaseOrder(plantId, input);

                var dictInsertedOrUpdatedMaterials = await InsertOrUpdateMaterials(purchaseOrderId, input.ListOfMaterials);

                //Audit Events
                return PMMSValidationConst.SAPPOCreated;
            }
            else
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.SAPPlantNotFound);
            }
        }

        public async Task<List<PurchaseOrderInternalDto>> GetPurchsaeOrderDetailsAsync(int purchaseOrderId)
        {
            var purchaseOrders = await (from purchaseOrder in _purchaseOrderRepository.GetAll()
                                        where purchaseOrder.Id == purchaseOrderId
                                        orderby purchaseOrder.PurchaseOrderNo
                                        select new PurchaseOrderInternalDto
                                        {
                                            PlantId = purchaseOrder.PlantId,
                                            Id = purchaseOrder.Id,
                                            PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                                            PurchaseOrderDate = purchaseOrder.PurchaseOrderDate,
                                            PurchaseOrderDeliverSchedule = (int)(DateTime.Now - purchaseOrder.PurchaseOrderDate).TotalDays,
                                            VendorName = purchaseOrder.VendorName,
                                            VendorCode = purchaseOrder.VendorCode,
                                            Materials = (from material in _materialRepository.GetAll()
                                                         where material.PurchaseOrderId == purchaseOrderId
                                                         orderby material.ItemCode
                                                         select new MaterialInternalDto
                                                         {
                                                             Id = material.Id,
                                                             Number = material.ItemNo,
                                                             Code = material.ItemCode,
                                                             Description = material.ItemDescription,
                                                             Quantity = material.OrderQuantity,
                                                             BalanceQuantity = material.BalanceQuantity,
                                                             PickedInvoiceQuantity = material.OrderQuantity - material.BalanceQuantity,
                                                             UOM = material.UnitOfMeasurement,
                                                             ManufacturerCode = material.ManufacturerCode,
                                                             ManufacturerName = material.ManufacturerName,
                                                         }).ToList()
                                        }).ToListAsync() ?? default;

            return purchaseOrders;
        }

        private async Task<int> GetPlantIdIfAlreadyExist(PurchaseOrderDto input)
        {
            var plantId = input.PlantId.Trim().ToLower();

            return await _plantRepository.GetAll().Where(x => x.PlantId.ToLower() == input.PlantId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> GetPurchaseOrderIdIfAlreadyExist(PurchaseOrderDto input)
        {
            var purchaseOrderNo = input.PurchaseOrderNo.Trim().ToLower();
            return await _purchaseOrderRepository.GetAll().Where(x => x.PurchaseOrderNo.ToLower() == purchaseOrderNo).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> InsertOrUpdatePurchaseOrder(int plantId, PurchaseOrderDto input)
        {
            int purchaseOrderId = await GetPurchaseOrderIdIfAlreadyExist(input);
            if (purchaseOrderId > 0)
            {
                var purchaseOrder = await _purchaseOrderRepository.GetAsync(purchaseOrderId);
                ObjectMapper.Map(input, purchaseOrder);
                purchaseOrder.PlantId = plantId;
                await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            }
            else
            {
                var purchaseOrder = ObjectMapper.Map<ELog.Core.Entities.PurchaseOrder>(input);
                purchaseOrder.PlantId = plantId;
                var insertedPurchaseOrder = await _purchaseOrderRepository.InsertAsync(purchaseOrder);
                CurrentUnitOfWork.SaveChanges();
                purchaseOrderId = insertedPurchaseOrder.Id;
            }
            return purchaseOrderId;
        }

        private async Task<Dictionary<string, int>> InsertOrUpdateMaterials(int purchaseOrderId, List<MaterialDto> lstMaterials)
        {
            Dictionary<string, int> dictInsertedOrUpdatedMaterialId = new Dictionary<string, int>();
            var lstItemNo = lstMaterials.Select(x => x.ItemNo).Distinct();
            var existingEntitiesMaterials = await _materialRepository.GetAll().Where(x => lstItemNo.Contains(x.ItemNo) && x.PurchaseOrderId == purchaseOrderId).ToListAsync();
            var lstNewItemNo = lstItemNo.Except(existingEntitiesMaterials.Select(x => x.ItemNo));
            foreach (var newItemNo in lstNewItemNo)
            {
                var materialDtoToInsert = lstMaterials.Last(x => x.ItemNo == newItemNo);
                var material = ObjectMapper.Map<ELog.Core.Entities.Material>(materialDtoToInsert);
                material.PurchaseOrderId = purchaseOrderId;
                material.UnitOfMeasurement = materialDtoToInsert.UnitOfMeasurement;
                var insertedMaterial = await _materialRepository.InsertAsync(material);
                CurrentUnitOfWork.SaveChanges();
                dictInsertedOrUpdatedMaterialId.Add(insertedMaterial.ItemNo, insertedMaterial.Id);
            }
            foreach (var materialToUpdate in existingEntitiesMaterials)
            {
                var materialDtoToUpdate = lstMaterials.Last(x => x.ItemNo == materialToUpdate.ItemNo);
                ObjectMapper.Map(materialDtoToUpdate, materialToUpdate);
                materialToUpdate.UnitOfMeasurement = materialDtoToUpdate.UnitOfMeasurement;
                await _materialRepository.UpdateAsync(materialToUpdate);
                dictInsertedOrUpdatedMaterialId.Add(materialDtoToUpdate.ItemNo, materialToUpdate.Id);
            }
            return dictInsertedOrUpdatedMaterialId;
        }
    }
}