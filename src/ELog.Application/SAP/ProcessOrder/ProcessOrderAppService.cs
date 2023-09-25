using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using ELog.Application.SAP.ProcessOrder.Dto;
using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.SAP.ProcessOrder
{
    public class ProcessOrderAppService : ApplicationService, IProcessOrderAppService
    {
        private readonly IRepository<ELog.Core.Entities.ProcessOrder> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterial> _materialRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ProcessOrderAppService(IRepository<ELog.Core.Entities.ProcessOrder> processOrderRepository,
            IRepository<ProcessOrderMaterial> materialRepository,
            IRepository<PlantMaster> plantRepository)
        {
            _processOrderRepository = processOrderRepository;
            _plantRepository = plantRepository;
            _materialRepository = materialRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task<string> UpdateAsync(ProcessOrderDto input)
        {
            var plantId = await GetPlantIdIfAlreadyExist(input);
            if (plantId > 0)
            {
                var processOrderId = await InsertOrUpdateProcessOrder(plantId, input);

                await InsertOrUpdateMaterials(processOrderId, input.ListOfMaterials);

                //Audit Events
                return PMMSValidationConst.SAPPROCreated;
            }
            else
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.SAPPlantNotFound);
            }
        }

        private async Task<int> GetPlantIdIfAlreadyExist(ProcessOrderDto input)
        {
            var plantId = input.PlantId.Trim().ToLower();

            return await _plantRepository.GetAll().Where(x => x.PlantId.ToLower() == input.PlantId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<bool> IsMaterialApproved(string materialCode)
        {
            var material = await _materialRepository.GetAll().Where(x => x.ItemCode.ToLower() == materialCode).FirstOrDefaultAsync();
            return true;
        }

        private async Task<int> GetProcessOrderIdIfAlreadyExist(ProcessOrderDto input)
        {
            var processOrderNo = input.ProcessOrderNo.Trim().ToLower();
            return await _processOrderRepository.GetAll().Where(x => x.ProcessOrderNo.ToLower() == processOrderNo).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> InsertOrUpdateProcessOrder(int plantId, ProcessOrderDto input)
        {
            int processOrderId = await GetProcessOrderIdIfAlreadyExist(input);
            if (processOrderId > 0)
            {
                var processOrder = await _processOrderRepository.GetAsync(processOrderId);
                ObjectMapper.Map(input, processOrder);
                processOrder.PlantId = plantId;
                await _processOrderRepository.UpdateAsync(processOrder);
            }
            else
            {
                var processOrder = ObjectMapper.Map<ELog.Core.Entities.ProcessOrder>(input);
                processOrder.PlantId = plantId;
                var insertedPurchaseOrder = await _processOrderRepository.InsertAsync(processOrder);
                CurrentUnitOfWork.SaveChanges();
                processOrderId = insertedPurchaseOrder.Id;
            }
            return processOrderId;
        }

        private async Task<Dictionary<string, int>> InsertOrUpdateMaterials(int processOrderId, List<ProcessOrderMaterialDto> lstMaterials)
        {
            Dictionary<string, int> dictInsertedOrUpdatedMaterialId = new Dictionary<string, int>();
            var lstItemNo = lstMaterials.Select(x => x.ItemNo).Distinct();
            var existingEntitiesMaterials = await _materialRepository.GetAll().Where(x => lstItemNo.Contains(x.ItemNo) && x.ProcessOrderId == processOrderId).ToListAsync();
            var lstNewItemNo = lstItemNo.Except(existingEntitiesMaterials.Select(x => x.ItemNo));
            foreach (var newItemNo in lstNewItemNo)
            {
                var materialDtoToInsert = lstMaterials.Last(x => x.ItemNo == newItemNo);
                var material = ObjectMapper.Map<ProcessOrderMaterial>(materialDtoToInsert);
                material.ProcessOrderId = processOrderId;
                var insertedMaterial = await _materialRepository.InsertAsync(material);
                CurrentUnitOfWork.SaveChanges();
                dictInsertedOrUpdatedMaterialId.Add(insertedMaterial.ItemNo, insertedMaterial.Id);
            }
            foreach (var materialToUpdate in existingEntitiesMaterials)
            {
                var materialDtoToUpdate = lstMaterials.Last(x => x.ItemNo == materialToUpdate.ItemNo);
                ObjectMapper.Map(materialDtoToUpdate, materialToUpdate);
                await _materialRepository.UpdateAsync(materialToUpdate);
                dictInsertedOrUpdatedMaterialId.Add(materialDtoToUpdate.ItemNo, materialToUpdate.Id);
            }
            return dictInsertedOrUpdatedMaterialId;
        }

        public async Task<List<ProcessOrderInternalDto>> GetProcessOrderDetailsAsync(int purchaseOrderId)
        {
            var purchaseOrders = await (from processOrder in _processOrderRepository.GetAll()
                                        where processOrder.Id == purchaseOrderId
                                        orderby processOrder.ProcessOrderNo
                                        select new ProcessOrderInternalDto
                                        {
                                            PlantId = processOrder.PlantId,
                                            Id = processOrder.Id,
                                            ProcessOrderNo = processOrder.ProcessOrderNo,
                                            ListOfMaterials = (from material in _materialRepository.GetAll()
                                                               where material.ProcessOrderId == purchaseOrderId
                                                               orderby material.ItemCode
                                                               select new ProcessOrderMaterialInternalDto
                                                               {
                                                                   Id = material.Id,
                                                                   ItemNo = material.ItemNo,
                                                                   ItemCode = material.ItemCode,
                                                                   ItemDescription = material.ItemDescription,
                                                                   OrderQuantity = material.OrderQuantity,
                                                                   UnitOfMeasurementId = material.UnitOfMeasurementId,
                                                                   UnitOfMeasurement = material.UnitOfMeasurement
                                                               }).ToList()
                                        }).ToListAsync() ?? default;

            return purchaseOrders;
        }
    }
}