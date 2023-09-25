using Abp.Application.Services;
using Abp.Domain.Repositories;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.CommonService.Report
{
    [PMMSAuthorize]
    public class ReportFiltersAppService : ApplicationService, IReportFiltersAppService
    {
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignHeaderRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignDetailRepository;
        private readonly IRepository<CubicleMaster> _cubicleMasterRepository;
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<LineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialDispensingHeaderRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayRepository;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<EquipmentCleaningTransaction> _equipmentCleaningTransactionRepository;
        private readonly IRepository<CubicleCleaningTransaction> _cubicleCleaningTransactionRepository;
        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContaierRepository;
        private readonly IRepository<CalibrationStatusMaster> _calibrationStatusRepository;
        private readonly IRepository<WMCalibrationHeader> _wmCalibrationHeaderRepository;
        private readonly int PickingMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Picking;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;

        public ReportFiltersAppService(IRepository<CubicleAssignmentHeader> cubicleAssignHeaderRepository, IRepository<CubicleAssignmentDetail> cubicleAssignDetailRepository,
                                       IRepository<CubicleMaster> cubicleMasterRepository, IRepository<LineClearanceTransaction> lineClearanceTransactionRepository,
                                       IRepository<ProcessOrder> processOrderRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
                                       IRepository<InspectionLot> inspectionlotRepository, IRepository<MaterialBatchDispensingHeader> materialDispensingHeaderRepository,
                                       IRepository<AreaMaster> areaRepository, IRepository<LocationMaster> locationRepository, IRepository<PutAwayBinToBinTransfer> putAwayRepository,
                                       IRepository<Palletization> palletizationRepository, IRepository<Material> materialRepository, IRepository<GRNDetail> grnDetailRepository, IRepository<DispensingHeader> dispensingHeaderRepository,
                                       IRepository<DispensingDetail> dispensingDetailRepository, IRepository<WeighingMachineMaster> weighingMachineRepository, IRepository<EquipmentMaster> equipmentRepository, IRepository<EquipmentCleaningTransaction> equipmentCleaningTransactionRepository
                                       , IRepository<CubicleCleaningTransaction> cubicleCleaningTransactionRepository, IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContaierRepository, IRepository<CalibrationStatusMaster> calibrationStatusRepository,
                                        IRepository<WMCalibrationHeader> wmCalibrationHeaderRepository
                                       )
        {
            _cubicleAssignHeaderRepository = cubicleAssignHeaderRepository;
            _cubicleAssignDetailRepository = cubicleAssignDetailRepository;
            _cubicleMasterRepository = cubicleMasterRepository;
            _processOrderRepository = processOrderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _inspectionLotRepository = inspectionlotRepository;
            _materialDispensingHeaderRepository = materialDispensingHeaderRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _palletizationRepository = palletizationRepository;
            _putAwayRepository = putAwayRepository;
            _materialRepository = materialRepository;
            _grnDetailRepository = grnDetailRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _dispensingDetailRepository = dispensingDetailRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _equipmentRepository = equipmentRepository;
            _equipmentCleaningTransactionRepository = equipmentCleaningTransactionRepository;
            _cubicleCleaningTransactionRepository = cubicleCleaningTransactionRepository;
            _materialBatchDispensingContaierRepository = materialBatchDispensingContaierRepository;
            _wmCalibrationHeaderRepository = wmCalibrationHeaderRepository;
            _calibrationStatusRepository = calibrationStatusRepository;
        }

        #region cubicleAssignment

        public async Task<List<SelectListDtoWithPlantId>> GetAssignCubiclesByPlantIdAsync(int? plantId)
        {
            var cubicleQuery = from cubicle in _cubicleMasterRepository.GetAll()
                               join cubicleAssignmentDetail in _cubicleAssignDetailRepository.GetAll()
                               on cubicle.Id equals cubicleAssignmentDetail.CubicleId
                               join cubicleAssigmentHeader in _cubicleAssignHeaderRepository.GetAll()
                               on cubicleAssignmentDetail.CubicleAssignmentHeaderId equals cubicleAssigmentHeader.Id
                               where cubicle.IsActive
                               select new SelectListDtoWithPlantId
                               {
                                   Id = cubicle.Id,
                                   Value = cubicle.CubicleCode,
                                   PlantId = cubicle.PlantId,
                                   IsSampling = cubicleAssigmentHeader.IsSampling
                               };
            if (plantId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == plantId);
            }
            var result = await cubicleQuery?.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetCubiclesAssignProductsAsync(int? plantId)
        {
            var productQuery = from cubicle in _cubicleAssignHeaderRepository.GetAll()
                               join processOrder in _processOrderRepository.GetAll()
                               on cubicle.ProductCode equals processOrder.ProductCode
                               orderby cubicle.ProductCode
                               select new SelectListDtoWithPlantId
                               {
                                   Id = processOrder.Id,
                                   Value = processOrder.ProductCode,
                                   PlantId = processOrder.PlantId,
                                   IsSampling = false,
                               };
            var inspectionLotQuery = from cubicle in _cubicleAssignHeaderRepository.GetAll()
                                     join inspectionLot in _inspectionLotRepository.GetAll()
                                     on cubicle.ProductCode equals inspectionLot.ProductCode
                                     orderby cubicle.ProductCode
                                     select new SelectListDtoWithPlantId
                                     {
                                         Id = inspectionLot.Id,
                                         Value = inspectionLot.ProductCode,
                                         PlantId = inspectionLot.PlantId,
                                         IsSampling = true,
                                     };
            var groupedResult = productQuery.Union(inspectionLotQuery);
            if (plantId != null)
            {
                groupedResult = groupedResult.Where(x => x.PlantId == plantId);
            }
            var result = await groupedResult.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetCubiclesAssignSAPBatchNoAsync(int? plantId)
        {
            var processorderSapBatchNoQuery = from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                              join processOrder in _processOrderRepository.GetAll()
                                              on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                              join cubicleAssignHeader in _cubicleAssignHeaderRepository.GetAll()
                                              on processOrder.ProductCode.ToLower() equals cubicleAssignHeader.ProductCode.ToLower()
                                              orderby processOrderMaterial.SAPBatchNo
                                              select new SelectListDtoWithPlantId
                                              {
                                                  Id = processOrderMaterial.Id,
                                                  Value = processOrderMaterial.SAPBatchNo,
                                                  PlantId = processOrder.PlantId,
                                                  IsSampling = false,
                                              };
            var inspectionSapBatchNoQuery = from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                            join inspectionLot in _inspectionLotRepository.GetAll()
                                            on processOrderMaterial.ProcessOrderId equals inspectionLot.Id
                                            join cubicleAssignHeader in _cubicleAssignHeaderRepository.GetAll()
                                            on inspectionLot.ProductCode.ToLower() equals cubicleAssignHeader.ProductCode.ToLower()
                                            orderby processOrderMaterial.SAPBatchNo
                                            select new SelectListDtoWithPlantId
                                            {
                                                Id = processOrderMaterial.Id,
                                                Value = processOrderMaterial.SAPBatchNo,
                                                PlantId = inspectionLot.PlantId,
                                                IsSampling = true,
                                            };
            var unionRsult = processorderSapBatchNoQuery.Union(inspectionSapBatchNoQuery);
            if (plantId != null)
            {
                unionRsult = unionRsult.Where(x => x.PlantId == plantId);
            }
            var result = await unionRsult.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion cubicleAssignment

        #region lineClearance

        public async Task<List<SelectListDtoWithPlantId>> GetAllLineClearanceMaterialAsync(int? plantId)
        {
            var materialQuery = (from lineClearance in _lineClearanceTransactionRepository.GetAll()
                                 join cubicleDetails in _cubicleAssignDetailRepository.GetAll()
                                 on lineClearance.GroupId equals cubicleDetails.CubicleAssignmentHeaderId
                                 join material in _processOrderMaterialRepository.GetAll()
                                 on cubicleDetails.ProcessOrderMaterialId equals material.Id
                                 join processOrder in _processOrderRepository.GetAll()
                                 on material.ProcessOrderId equals processOrder.Id
                                 orderby material.ItemCode
                                 select new SelectListDtoWithPlantId
                                 {
                                     Id = material.Id,
                                     Value = material.ItemCode,
                                     PlantId = processOrder.PlantId,
                                     IsSampling = lineClearance.IsSampling
                                 });

            if (plantId != null)
            {
                materialQuery = materialQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await materialQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetLineClearanceSAPBatchNoAsync(int? plantId)
        {
            var sapBatchNoQuery = from lineClearance in _lineClearanceTransactionRepository.GetAll()
                                  join cubicle in _cubicleMasterRepository.GetAll()
                                  on lineClearance.CubicleId equals cubicle.Id
                                  join cubicleDetails in _cubicleAssignDetailRepository.GetAll()
                                  on lineClearance.GroupId equals cubicleDetails.CubicleAssignmentHeaderId
                                  join material in _processOrderMaterialRepository.GetAll()
                                  on cubicleDetails.ProcessOrderMaterialId equals material.Id
                                  orderby material.ItemCode
                                  select new SelectListDtoWithPlantId
                                  {
                                      Id = material.Id,
                                      Value = material.SAPBatchNo,
                                      PlantId = cubicle.PlantId,
                                      IsSampling = lineClearance.IsSampling,
                                  };
            if (plantId != null)
            {
                sapBatchNoQuery = sapBatchNoQuery.Where(x => x.PlantId == plantId);
            }
            var result = await sapBatchNoQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetLineClearanceCubicleByPlantIdAsync(int? plantId)
        {
            var cubicleQuery = from cubicle in _cubicleMasterRepository.GetAll()
                               join lineClearance in _lineClearanceTransactionRepository.GetAll()
                               on cubicle.Id equals lineClearance.CubicleId
                               where cubicle.IsActive
                               select new SelectListDtoWithPlantId
                               {
                                   Id = cubicle.Id,
                                   Value = cubicle.CubicleCode,
                                   PlantId = cubicle.PlantId,
                                   IsSampling = lineClearance.IsSampling,
                               };
            if (plantId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == plantId);
            }
            var result = await cubicleQuery?.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetLineClearanceProductsAsync(int? plantId)
        {
            var productQuery = from lineClearance in _lineClearanceTransactionRepository.GetAll()
                               join cubicleHeader in _cubicleAssignHeaderRepository.GetAll()
                               on lineClearance.GroupId equals cubicleHeader.Id
                               join cubicleMaster in _cubicleMasterRepository.GetAll()
                               on lineClearance.CubicleId equals cubicleMaster.Id
                               orderby cubicleHeader.ProductCode
                               select new SelectListDtoWithPlantId
                               {
                                   Id = cubicleHeader.Id,
                                   Value = cubicleHeader.ProductCode,
                                   PlantId = cubicleMaster.PlantId,
                                   IsSampling = lineClearance.IsSampling
                               };
            if (plantId != null)
            {
                productQuery = productQuery.Where(x => x.PlantId == plantId);
            }
            var result = await productQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion lineClearance

        #region picking

        public async Task<List<SelectListDtoWithPlantId>> GetProcessOrderAndInspectionLotNo(int? plantId)
        {
            var processOrderQuery = from pickingHeader in _materialDispensingHeaderRepository.GetAll()
                                    join pickingDetails in _materialBatchDispensingContaierRepository.GetAll()
                                    on pickingHeader.Id equals pickingDetails.MaterialBatchDispensingHeaderId
                                    join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                    on pickingHeader.MaterialCode.ToLower() equals processOrderMaterial.ItemCode.ToLower()
                                    join cubicleAssignDetail in _cubicleAssignDetailRepository.GetAll()
                                    on processOrderMaterial.Id equals cubicleAssignDetail.ProcessOrderMaterialId
                                    join cubicleHeader in _cubicleAssignHeaderRepository.GetAll()
                                    on pickingHeader.GroupCode equals cubicleHeader.GroupId
                                    join processOrder in _processOrderRepository.GetAll()
                                    on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                    where pickingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                    select new SelectListDtoWithPlantId
                                    {
                                        Id = processOrder.Id,
                                        Value = processOrder.ProcessOrderNo,
                                        PlantId = processOrder.PlantId,
                                        IsSampling = false
                                    };
            var inspectionLotQuery = from pickingHeader in _materialDispensingHeaderRepository.GetAll()
                                     join pickingDetails in _materialBatchDispensingContaierRepository.GetAll()
                                     on pickingHeader.Id equals pickingDetails.MaterialBatchDispensingHeaderId
                                     join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                     on pickingHeader.MaterialCode.ToLower() equals processOrderMaterial.ItemCode.ToLower()
                                     join cubicleAssignDetail in _cubicleAssignDetailRepository.GetAll()
                                     on processOrderMaterial.Id equals cubicleAssignDetail.ProcessOrderMaterialId
                                     join cubicleHeader in _cubicleAssignHeaderRepository.GetAll()
                                     on pickingHeader.GroupCode equals cubicleHeader.GroupId
                                     join inspectionLot in _inspectionLotRepository.GetAll()
                                     on processOrderMaterial.InspectionLotId equals inspectionLot.Id
                                     where pickingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                     select new SelectListDtoWithPlantId
                                     {
                                         Id = inspectionLot.Id,
                                         Value = inspectionLot.InspectionLotNumber,
                                         PlantId = inspectionLot.PlantId,
                                         IsSampling = true
                                     };
            var pickedProcessOrdersAndInspectionLotNos = processOrderQuery.Union(inspectionLotQuery);
            if (plantId != null)
            {
                pickedProcessOrdersAndInspectionLotNos = pickedProcessOrdersAndInspectionLotNos.Where(x => x.PlantId == plantId);
            }
            var result = await pickedProcessOrdersAndInspectionLotNos?.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetPickedCubiclesMaterialCodesByPlantIdAsync(int? plantId)
        {
            var materialCodeQuery = from pickingHeader in _materialDispensingHeaderRepository.GetAll()
                                    join poMaterial in _processOrderMaterialRepository.GetAll()
                                    on pickingHeader.MaterialCode.ToLower() equals poMaterial.ItemCode.ToLower()
                                    join processOrder in _processOrderRepository.GetAll()
                                    on poMaterial.ProcessOrderId equals processOrder.Id
                                    where pickingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                    select new SelectListDtoWithPlantId
                                    {
                                        Id = poMaterial.Id,
                                        Value = poMaterial.ItemCode,
                                        PlantId = processOrder.PlantId,
                                        IsSampling = pickingHeader.IsSampling
                                    };
            if (plantId != null)
            {
                materialCodeQuery = materialCodeQuery.Where(x => x.PlantId == plantId);
            }
            var result = await materialCodeQuery?.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetPickedCubiclesBatchNoAsync(int? plantId)
        {
            var batchNoQuery = from pickingHeader in _materialDispensingHeaderRepository.GetAll()
                               join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                               on pickingHeader.MaterialCode.ToLower() equals processOrderMaterial.ItemCode.ToLower()
                               join processOrder in _processOrderRepository.GetAll()
                               on processOrderMaterial.ProcessOrderId equals processOrder.Id
                               where pickingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                               orderby processOrderMaterial.BatchNo
                               select new SelectListDtoWithPlantId
                               {
                                   Id = processOrderMaterial.Id,
                                   Value = processOrderMaterial.BatchNo,
                                   PlantId = processOrder.PlantId,
                                   IsSampling = pickingHeader.IsSampling
                               };
            if (plantId != null)
            {
                batchNoQuery = batchNoQuery.Where(x => x.PlantId == plantId);
            }
            var result = await batchNoQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetPickedCubiclesSAPBatchNoAsync(int? plantId)
        {
            var sapBatchNoQuery = from pickingHeader in _materialDispensingHeaderRepository.GetAll()
                                  join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                  on pickingHeader.MaterialCode.ToLower() equals processOrderMaterial.ItemCode.ToLower()
                                  join processOrder in _processOrderRepository.GetAll()
                                  on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                  where pickingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                  orderby processOrderMaterial.SAPBatchNo
                                  select new SelectListDtoWithPlantId
                                  {
                                      Id = processOrderMaterial.Id,
                                      Value = processOrderMaterial.SAPBatchNo,
                                      PlantId = processOrder.PlantId,
                                      IsSampling = pickingHeader.IsSampling
                                  };
            if (plantId != null)
            {
                sapBatchNoQuery = sapBatchNoQuery.Where(x => x.PlantId == plantId);
            }
            var result = await sapBatchNoQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.IsSampling, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion picking

        #region allocation

        public async Task<List<SelectListDtoWithPlantId>> GetAllocatedAreasAsync(int? plantId)
        {
            var areaQuery = from putAway in _putAwayRepository.GetAll()
                            join location in _locationRepository.GetAll()
                            on putAway.LocationId equals location.Id
                            join area in _areaRepository.GetAll()
                            on location.AreaId equals area.Id
                            where !putAway.IsUnloaded
                            select new SelectListDtoWithPlantId
                            {
                                Id = area.Id,
                                Value = area.AreaName,
                                PlantId = area.SubPlantId
                            };
            if (plantId != null)
            {
                areaQuery = areaQuery.Where(x => x.PlantId == plantId);
            }
            var result = await areaQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllocatedMaterialCodesByPlantIdAsync(int? plantId)
        {
            var putAwayMaterialQuery = from putAway in _putAwayRepository.GetAll()
                                       join material in _materialRepository.GetAll()
                                       on putAway.MaterialId equals material.Id
                                       join location in _locationRepository.GetAll()
                                       on putAway.LocationId equals location.Id
                                       where !putAway.IsUnloaded
                                       select new SelectListDtoWithPlantId
                                       {
                                           Id = material.ItemCode,
                                           Value = material.ItemCode,
                                           PlantId = location.PlantId
                                       };
            var palletizationMaterialQuery = from putAway in _putAwayRepository.GetAll()
                                             join pallet in _palletizationRepository.GetAll()
                                             on putAway.PalletId equals pallet.PalletId
                                             join material in _materialRepository.GetAll()
                                             on pallet.MaterialId equals material.Id
                                             join location in _locationRepository.GetAll()
                                             on putAway.LocationId equals location.Id
                                             where !putAway.IsUnloaded && !pallet.IsUnloaded
                                             select new SelectListDtoWithPlantId
                                             {
                                                 Id = material.ItemCode,
                                                 Value = material.ItemCode,
                                                 PlantId = location.PlantId
                                             };
            var materials = putAwayMaterialQuery.Union(palletizationMaterialQuery);
            if (plantId != null)
            {
                materials = materials.Where(x => x.PlantId == plantId);
            }
            var result = await materials?.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllocatedSAPBatchNoAsync(int? plantId)
        {
            var putAwaySapBatchNoQuery = from putAway in _putAwayRepository.GetAll()
                                         join material in _materialRepository.GetAll()
                                         on putAway.MaterialId equals material.Id
                                         join location in _locationRepository.GetAll()
                                         on putAway.LocationId equals location.Id
                                         where !putAway.IsUnloaded
                                         select new SelectListDtoWithPlantId
                                         {
                                             Id = putAway.SAPBatchNumber,
                                             Value = putAway.SAPBatchNumber,
                                             PlantId = location.PlantId
                                         };
            var palletizationSapBatchNoQuery = from putAway in _putAwayRepository.GetAll()
                                               join pallet in _palletizationRepository.GetAll()
                                               on putAway.PalletId equals pallet.PalletId
                                               join material in _materialRepository.GetAll()
                                               on pallet.MaterialId equals material.Id
                                               join location in _locationRepository.GetAll()
                                               on putAway.LocationId equals location.Id
                                               where !putAway.IsUnloaded && !pallet.IsUnloaded
                                               select new SelectListDtoWithPlantId
                                               {
                                                   Id = pallet.SAPBatchNumber,
                                                   Value = pallet.SAPBatchNumber,
                                                   PlantId = location.PlantId
                                               };
            var sapBatchNoQuery = putAwaySapBatchNoQuery.Union(palletizationSapBatchNoQuery);
            if (plantId != null)
            {
                sapBatchNoQuery = sapBatchNoQuery.Where(x => x.PlantId == plantId);
            }
            var result = await sapBatchNoQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId }).OrderBy(x => x.Value).ToList();
        }

        #endregion allocation

        #region dispensing

        public async Task<List<SelectListDtoWithPlantId>> GetAllDispensingMaterialAsync(int? plantId)
        {
            var materialDispensingQuery = (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                           join processOrder in _processOrderRepository.GetAll()
                                           on dispensingHeader.ProcessOrderId equals processOrder.Id
                                           where !dispensingHeader.IsSampling
                                           orderby dispensingHeader.MaterialCodeId
                                           select new SelectListDtoWithPlantId
                                           {
                                               Id = dispensingHeader.MaterialCodeId,
                                               Value = dispensingHeader.MaterialCodeId,
                                               PlantId = processOrder.PlantId,
                                               IsSampling = dispensingHeader.IsSampling
                                           });

            var materialInspectionLotQuery = (from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                              join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                              on dispensingHeader.InspectionLotId equals processOrderMaterial.InspectionLotId
                                              join processOrder in _processOrderRepository.GetAll()
                                              on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                              where dispensingHeader.IsSampling
                                              orderby dispensingHeader.MaterialCodeId
                                              select new SelectListDtoWithPlantId
                                              {
                                                  Id = dispensingHeader.MaterialCodeId,
                                                  Value = dispensingHeader.MaterialCodeId,
                                                  PlantId = processOrder.PlantId,
                                                  IsSampling = dispensingHeader.IsSampling
                                              });
            var materialQuery = materialDispensingQuery.Union(materialInspectionLotQuery);
            if (plantId != null)
            {
                materialQuery = materialQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await materialQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllDispensingSAPBatchNoAsync(int? plantId)
        {
            var sapBatchNoDispensingQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                            join dispensingDetail in _dispensingDetailRepository.GetAll()
                                            on dispensingHeader.Id equals dispensingDetail.DispensingHeaderId
                                            join processOrder in _processOrderRepository.GetAll()
                                            on dispensingHeader.ProcessOrderId equals processOrder.Id
                                            where !dispensingHeader.IsSampling
                                            orderby dispensingDetail.SAPBatchNumber
                                            select new SelectListDtoWithPlantId
                                            {
                                                Id = dispensingDetail.SAPBatchNumber,
                                                Value = dispensingDetail.SAPBatchNumber,
                                                PlantId = processOrder.PlantId,
                                                IsSampling = dispensingHeader.IsSampling
                                            };
            var sapBatchNoInspectionLotQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                               join dispensingDetail in _dispensingDetailRepository.GetAll()
                                               on dispensingHeader.Id equals dispensingDetail.DispensingHeaderId
                                               join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                               on dispensingHeader.InspectionLotId equals processOrderMaterial.InspectionLotId
                                               join processOrder in _processOrderRepository.GetAll()
                                                on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                               where dispensingHeader.IsSampling
                                               orderby dispensingDetail.SAPBatchNumber
                                               select new SelectListDtoWithPlantId
                                               {
                                                   Id = dispensingDetail.SAPBatchNumber,
                                                   Value = dispensingDetail.SAPBatchNumber,
                                                   PlantId = processOrder.PlantId,
                                                   IsSampling = dispensingHeader.IsSampling
                                               };
            var sapBatchNoQuery = sapBatchNoDispensingQuery.Union(sapBatchNoInspectionLotQuery);
            if (plantId != null)
            {
                sapBatchNoQuery = sapBatchNoQuery.Where(x => x.PlantId == plantId);
            }
            var result = await sapBatchNoQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllDispensingProductsAsync(int? plantId)
        {
            var productDispensingQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                         join processOrder in _processOrderRepository.GetAll()
                                         on dispensingHeader.ProcessOrderId equals processOrder.Id
                                         where !dispensingHeader.IsSampling
                                         orderby processOrder.ProductCode
                                         select new SelectListDtoWithPlantId
                                         {
                                             Id = processOrder.ProductCode,
                                             Value = processOrder.ProductCode,
                                             PlantId = processOrder.PlantId,
                                             IsSampling = dispensingHeader.IsSampling
                                         };
            var productInspectionLotQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                            join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                            on dispensingHeader.InspectionLotId equals processOrderMaterial.InspectionLotId
                                            join processOrder in _processOrderRepository.GetAll()
                                             on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                            where dispensingHeader.IsSampling
                                            orderby processOrder.ProductCode
                                            select new SelectListDtoWithPlantId
                                            {
                                                Id = processOrder.ProductCode,
                                                Value = processOrder.ProductCode,
                                                PlantId = processOrder.PlantId,
                                                IsSampling = dispensingHeader.IsSampling
                                            };
            var productQuery = productDispensingQuery.Union(productInspectionLotQuery);
            if (plantId != null)
            {
                productQuery = productQuery.Where(x => x.PlantId == plantId);
            }
            var result = await productQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllDispensingProcessOrderNoAsync(int? plantId)
        {
            var processOrderDispensingQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                              join processOrder in _processOrderRepository.GetAll()
                                              on dispensingHeader.ProcessOrderId equals processOrder.Id
                                              where !dispensingHeader.IsSampling
                                              orderby processOrder.ProcessOrderNo
                                              select new SelectListDtoWithPlantId
                                              {
                                                  Id = processOrder.Id,
                                                  Value = processOrder.ProcessOrderNo,
                                                  PlantId = processOrder.PlantId,
                                                  IsSampling = dispensingHeader.IsSampling
                                              };
            var processOrderInspectionLotQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                                 join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                 on dispensingHeader.InspectionLotId equals processOrderMaterial.InspectionLotId
                                                 join processOrder in _processOrderRepository.GetAll()
                                                 on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                                 where dispensingHeader.IsSampling
                                                 orderby processOrder.ProcessOrderNo
                                                 select new SelectListDtoWithPlantId
                                                 {
                                                     Id = processOrder.Id,
                                                     Value = processOrder.ProcessOrderNo,
                                                     PlantId = processOrder.PlantId,
                                                     IsSampling = dispensingHeader.IsSampling
                                                 };
            var processOrderQuery = processOrderDispensingQuery.Union(processOrderInspectionLotQuery);
            if (plantId != null)
            {
                processOrderQuery = processOrderQuery.Where(x => x.PlantId == plantId);
            }
            var result = await processOrderQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllDispensingProductBatchAsync(int? plantId)
        {
            var processOrderDispensingQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                              join processOrder in _processOrderRepository.GetAll()
                                              on dispensingHeader.ProcessOrderId equals processOrder.Id
                                              join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                              on dispensingHeader.ProcessOrderId equals processOrderMaterial.ProcessOrderId
                                              where !dispensingHeader.IsSampling
                                              orderby processOrder.ProcessOrderNo
                                              select new SelectListDtoWithPlantId
                                              {
                                                  Id = processOrderMaterial.BatchNo,
                                                  Value = processOrderMaterial.BatchNo,
                                                  PlantId = processOrder.PlantId,
                                                  IsSampling = dispensingHeader.IsSampling
                                              };
            var processOrderInspectionLotQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                                 join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                 on dispensingHeader.InspectionLotId equals processOrderMaterial.InspectionLotId
                                                 join processOrder in _processOrderRepository.GetAll()
                                                 on processOrderMaterial.ProcessOrderId equals processOrder.Id
                                                 where dispensingHeader.IsSampling
                                                 orderby processOrder.ProcessOrderNo
                                                 select new SelectListDtoWithPlantId
                                                 {
                                                     Id = processOrderMaterial.BatchNo,
                                                     Value = processOrderMaterial.BatchNo,
                                                     PlantId = processOrder.PlantId,
                                                     IsSampling = dispensingHeader.IsSampling
                                                 };
            var processOrderQuery = processOrderDispensingQuery.Union(processOrderInspectionLotQuery);
            if (plantId != null)
            {
                processOrderQuery = processOrderQuery.Where(x => x.PlantId == plantId);
            }
            var result = await processOrderQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Id, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion dispensing

        #region cubicleCleaning

        public async Task<List<SelectListDtoWithPlantId>> GetAllCleanCubicleByPlantAsync(int? plantId)
        {
            var cubicleQuery = from cubicle in _cubicleMasterRepository.GetAll()
                               join cubicleCleaningTransaction in _cubicleCleaningTransactionRepository.GetAll()
                               on cubicle.Id equals cubicleCleaningTransaction.CubicleId
                               where cubicle.ApprovalStatusId == approvedApprovalStatusId && cubicle.IsActive
                               select new SelectListDtoWithPlantId
                               {
                                   Id = cubicle.Id,
                                   Value = cubicle.CubicleCode,
                                   PlantId = cubicle.PlantId,
                                   IsSampling = cubicleCleaningTransaction.IsSampling
                               };
            if (plantId != null)
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await cubicleQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion cubicleCleaning

        #region equipmentCleaning

        public async Task<List<SelectListDtoWithPlantId>> GetAllCleanEquipmentByPlantAsync(int? plantId)
        {
            var equipmentQuery = from equipment in _equipmentRepository.GetAll()
                                 join equipementCleaningTransaction in _equipmentCleaningTransactionRepository.GetAll()
                               on equipment.Id equals equipementCleaningTransaction.EquipmentId
                                 where equipment.ApprovalStatusId == approvedApprovalStatusId && equipment.IsActive
                                 select new SelectListDtoWithPlantId
                                 {
                                     Id = equipment.Id,
                                     Value = equipment.EquipmentCode,
                                     PlantId = equipment.PlantId,
                                     IsSampling = equipementCleaningTransaction.IsSampling
                                 };
            if (plantId != null)
            {
                equipmentQuery = equipmentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var result = await equipmentQuery.ToListAsync() ?? default;
            return result.GroupBy(x => new { x.Value, x.PlantId }).Select(x => new SelectListDtoWithPlantId { Id = x.First().Id, Value = x.First().Value, PlantId = x.First().PlantId, IsSampling = x.First().IsSampling }).OrderBy(x => x.Value).ToList();
        }

        #endregion equipmentCleaning

        #region weighingCalibration

        public List<SelectListDto> GetCalibrationModeAsync()
        {
            var frequencyTypeList = new List<SelectListDto>();
            foreach (var e in Enum.GetValues(typeof(WeighingMachineFrequencyType)))
            {
                frequencyTypeList.Add(new SelectListDto { Id = (int)e, Value = e.ToString() });
            }
            return frequencyTypeList;
        }

        public async Task<string> GetLastCalibrationStatus(DateTime calibrationDate, int? weighingMachineId, int calibrationHeaderId)
        {
            var StartDate = calibrationDate.StartOfDay();
            var EndDate = calibrationDate.EndOfDay();
            var statusQuery = from calibrationheader in _wmCalibrationHeaderRepository.GetAll()
                              join status in _calibrationStatusRepository.GetAll()
                              on calibrationheader.CalibrationStatusId equals status.Id
                              where calibrationheader.Id != calibrationHeaderId && calibrationheader.WeighingMachineId == weighingMachineId
                              && calibrationheader.CalibrationTestDate >= StartDate && calibrationheader.CalibrationTestDate <= EndDate
                              select new SelectListDto
                              {
                                  Id = calibrationheader.Id,
                                  Value = status.StatusName,
                              };
            var value = await statusQuery.OrderByDescending(x => x.Id).Select(x => x.Value).FirstOrDefaultAsync();
            return value;
        }

        #endregion weighingCalibration
    }
}