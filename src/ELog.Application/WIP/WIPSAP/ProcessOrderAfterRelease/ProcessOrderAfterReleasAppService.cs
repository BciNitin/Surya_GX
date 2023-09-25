using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto;
using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease
{
    public class ProcessOrderAfterReleasAppService : ApplicationService, IProcessOrderAfterReleasAppService
    {
        private readonly IRepository<ELog.Core.Entities.ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _materialRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<MaterialMaster> _materialMaster;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ProcessOrderAfterReleasAppService(IRepository<ELog.Core.Entities.ProcessOrderAfterRelease> processOrderRepository,
            IRepository<ProcessOrderMaterialAfterRelease> materialRepository,
            IRepository<PlantMaster> plantRepository, IRepository<MaterialMaster> materialMaster)
        {
            _processOrderRepository = processOrderRepository;
            _plantRepository = plantRepository;
            _materialRepository = materialRepository;
            _materialMaster = materialMaster;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task<string> InsertUpdateProcessOrderAfterReleaseAsync(ProcessOrderAfterReleasDto input)
        {
            var plantId = await GetPlantIdIfAlreadyExist(input);
            if (plantId > 0)
            {
                var processOrderId = await InsertOrUpdateProcessOrderAfterRelease(plantId, input);

                await InsertOrUpdateMaterialsAfterRelease(processOrderId, input.ListOfMaterials);

                //Audit Events
                return PMMSValidationConst.SAPPROCreated;
            }
            else
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.SAPPlantNotFound);
            }
        }

        public async Task<ProcessOrderAfterReleasDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _processOrderRepository.GetAsync(input.Id);
            ProcessOrderAfterReleasDto response = new ProcessOrderAfterReleasDto();
            // response.Id = entity.Id;
            response.IsPicking = entity.IsPicking;
            response.ProcessOrderNo = entity.ProcessOrderNo;
            response.ProcessOrderType = entity.ProcessOrderType;
            response.ProductCode = entity.ProductCode;
            response.ProductCodeId = entity.ProductCodeId;
            return response;
            //return ObjectMapper.Map<ProcessOrderAfterReleasDto>(entity);
        }
        private async Task<int> InsertOrUpdateProcessOrderAfterRelease(int plantId, ProcessOrderAfterReleasDto input)
        {
            int processOrderId = await GetProcessOrderIdIfAlreadyExist(input);
            var productid = await GetProductCodeIfAlradyExist(input);

            input.ListOfMaterials.RemoveAll(p => p.MaterialCode.ToUpper() == "DUMMY");

            if (processOrderId > 0)
            {
                var processOrder = await _processOrderRepository.GetAsync(processOrderId);
                ObjectMapper.Map(input, processOrder);
                processOrder.PlantId = plantId;
                processOrder.ProductCodeId = productid;
                await _processOrderRepository.UpdateAsync(processOrder);
            }
            else
            {
                var processOrderAfterRelease = ObjectMapper.Map<ELog.Core.Entities.ProcessOrderAfterRelease>(input);
                processOrderAfterRelease.PlantId = plantId;
                processOrderAfterRelease.ProductCodeId = productid;
                var insertedPurchaseOrder = await _processOrderRepository.InsertAsync(processOrderAfterRelease);
                CurrentUnitOfWork.SaveChanges();
                processOrderId = insertedPurchaseOrder.Id;

            }
            return processOrderId;
        }

        private async Task<int> GetPlantIdIfAlreadyExist(ProcessOrderAfterReleasDto input)
        {
            var plantId = input.PlantId.Trim().ToLower();

            return await _plantRepository.GetAll().Where(x => x.PlantId.ToLower() == input.PlantId).Select(x => x.Id).FirstOrDefaultAsync();
        }
        //private async Task<bool> IsMaterialApproved(string materialCode)
        //{
        //    var material = await _materialRepository.GetAll().Where(x => x.MaterialCode.ToLower() == materialCode).FirstOrDefaultAsync();
        //    return true;
        //}

        private async Task<int> GetProductCodeIfAlradyExist(ProcessOrderAfterReleasDto input)
        {
            var productid = input.ProductCodeId;

            return await _materialMaster.GetAll().Where(x => x.MaterialCode.ToLower() == input.ProductCode.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }
        private async Task<int> GetProcessOrderIdIfAlreadyExist(ProcessOrderAfterReleasDto input)
        {
            var processOrderNo = input.ProcessOrderNo.Trim().ToLower();
            return await _processOrderRepository.GetAll().Where(x => x.ProcessOrderNo.ToLower() == processOrderNo).Select(x => x.Id).FirstOrDefaultAsync();
        }


        private async Task<Dictionary<string, int>> InsertOrUpdateMaterialsAfterRelease(int processOrderId, List<ProcessOrderMaterialAfterReleasDto> lstMaterials)
        {
            Dictionary<string, int> dictInsertedOrUpdatedMaterialId = new Dictionary<string, int>();
            var lstItemNo = lstMaterials.Select(x => x.MaterialCode).Distinct();
            var existingEntitiesMaterials = await _materialRepository.GetAll().Where(x => lstItemNo.Contains(x.MaterialCode) && x.ProcessOrderId == processOrderId).ToListAsync();
            var lstNewItemNo = lstItemNo.Except(existingEntitiesMaterials.Select(x => x.MaterialCode));
            foreach (var newItemNo in lstNewItemNo)
            {
                var materialId = await CheckMaterialExist(newItemNo);

                if (materialId > 0)
                {
                    var materialDtoToInsert = lstMaterials.Last(x => x.MaterialCode == newItemNo);
                    var material = ObjectMapper.Map<ProcessOrderMaterialAfterRelease>(materialDtoToInsert);
                    material.ProcessOrderId = processOrderId;
                    //var dtExpiry = material.ExpiryDate;
                    // var dtExpiry = DateTime.ParseExact(Convert.ToDateTime(dateExpiry), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    //material.ExpiryDate = dtExpiry;
                    //var dtRetest = material.RetestDate;
                    // var dtRetest = DateTime.ParseExact(dateRetest, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    // material.RetestDate = Convert.ToDateTime(dtRetest);
                    var insertedMaterial = await _materialRepository.InsertAsync(material);
                    CurrentUnitOfWork.SaveChanges();
                    dictInsertedOrUpdatedMaterialId.Add(insertedMaterial.MaterialCode, insertedMaterial.Id);

                    foreach (var materialToUpdate in existingEntitiesMaterials)
                    {
                        var materialDtoToUpdate = lstMaterials.Last(x => x.MaterialCode == materialToUpdate.MaterialCode);
                        ObjectMapper.Map(materialDtoToUpdate, materialToUpdate);
                        await _materialRepository.UpdateAsync(materialToUpdate);
                        dictInsertedOrUpdatedMaterialId.Add(materialDtoToUpdate.MaterialCode, materialToUpdate.Id);
                    }
                }
            }
            return dictInsertedOrUpdatedMaterialId;
        }
        private async Task<int> CheckMaterialExist(string MatCode)
        {

            return await _materialMaster.GetAll().Where(x => x.MaterialCode == MatCode).Select(x => x.Id).FirstOrDefaultAsync();
        }
        //public async Task<List<ProcessOrderAfterReleasInternalDto>> GetProcessOrderDetailsAsync(int purchaseOrderId)
        //{
        //    var purchaseOrders = await (from processOrder in _processOrderRepository.GetAll()
        //                                where processOrder.Id == purchaseOrderId
        //                                orderby processOrder.ProcessOrderNo
        //                                select new ProcessOrderAfterReleasInternalDto
        //                                {
        //                                    PlantId = processOrder.PlantId,
        //                                    Id = processOrder.Id,
        //                                    ProcessOrderNo = processOrder.ProcessOrderNo,
        //                                    ListOfMaterials = (from material in _materialRepository.GetAll()
        //                                                       where material.ProcessOrderId == purchaseOrderId
        //                                                       orderby material.MaterialCode
        //                                                       select new ProcessOrderMaterialAfterReleasInternalDto
        //                                                       {
        //                                                           Id = material.Id,
        //                                                           //ItemNo = material.ItemNo,
        //                                                           MaterialCode = material.MaterialCode,
        //                                                           MaterialDescription = material.MaterialDescription,
        //                                                           Quantity = material.Quantity,
        //                                                           //uom = material.UnitOfMeasurementId,
        //                                                           //UnitOfMeasurement = material.UnitOfMeasurement
        //                                                       }).ToList()
        //                                }).ToListAsync() ?? default;

        //    return purchaseOrders;
        //}
    }
}
