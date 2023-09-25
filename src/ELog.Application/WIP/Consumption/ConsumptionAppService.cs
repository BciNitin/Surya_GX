using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.Consumption.Dto;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.Consumption
{
    public class ConsumptionAppService : ApplicationService, IConsumptionAppService
    {
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
        private readonly IRepository<ELog.Core.Entities.Consumption> _consumptionRepository;
        private readonly IRepository<ConsumptionDetails> _consumptionDetails;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<MaterialMaster> _masterMasterRepository;


        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConsumptionAppService(IRepository<ProcessOrderAfterRelease> processOrderRepository,
        IHttpContextAccessor httpContextAccessor,
        IRepository<ELog.Core.Entities.Consumption> consumptionRepository,
        IRepository<CubicleMaster> cubicleRepository,
         IRepository<EquipmentMaster> equipmentRepository,
         IRepository<ConsumptionDetails> consumptionDetails,
         IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository,
         IRepository<MaterialMaster> masterMasterRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _processOrderRepository = processOrderRepository;
            _httpContextAccessor = httpContextAccessor;
            _consumptionRepository = consumptionRepository;
            _cubicleRepository = cubicleRepository;
            _equipmentRepository = equipmentRepository;
            _consumptionDetails = consumptionDetails;
            _processOrdermaterialRepository = processOrdermaterialRepository;
            _masterMasterRepository = masterMasterRepository;

        }


        public async Task<ConsumptionDto> CreateAsync(CreateConsumptionHeaderDto input)
        {
            var cubicleAssignment = ObjectMapper.Map<ELog.Core.Entities.Consumption>(input);
            //var equipmentUsageLog = await _processOrderRepository.GetAsync(input.Id);
            var entity = _processOrderRepository.GetAll().Where(x => x.ProductCode == input.ProductCode).FirstOrDefault();
            cubicleAssignment.ProductId = entity.ProductCodeId; //---Replace Processorder table to ProcessorderAfterRelease
            //cubicleAssignment.TenantId = AbpSession.TenantId;
            //var currentDate = DateTime.UtcNow;
            //cubicleAssignment.GroupId = $"GRP{ currentDate:yyyyMMddhhmmssff}";
            //var groupOpenStatus = await GetStatusIdOfStatus(openStatus);
            //var cubicleAssignmentDetailInProgressStatus = await GetStatusIdOfStatus(inProgressStatus);

            //cubicleAssignment.GroupStatusId = groupOpenStatus;
            //cubicleAssignment.CubicleAssignmentDate = DateTime.UtcNow;

            var cubicleAssignmentHeader = await _consumptionRepository.InsertAsync(cubicleAssignment);
            CurrentUnitOfWork.SaveChanges();

            /*  foreach (var detail in input.ConsumptionDetails)
              {
                  var cubicleAssignmentDetailToInsert = ObjectMapper.Map<ConsumptionDetails>(detail);
                  cubicleAssignmentDetailToInsert.ConsumptionId = cubicleAssignmentHeader.Id;
                  //cubicleAssignmentDetailToInsert.TenantId = AbpSession.TenantId;
                  //cubicleAssignmentDetailToInsert.StatusId = cubicleAssignmentDetailInProgressStatus;
                  await _consumptionDetails.InsertAsync(cubicleAssignmentDetailToInsert);
              } */
            return ObjectMapper.Map<ConsumptionDto>(cubicleAssignment);
        }

        public async Task<ConsumptionDto> UpdateAsync(ConsumptionDto input)
        {
            foreach (var material in input.ConsumptionDetails)
            {
                var cubicleAssignmentDetailToUpdate = ObjectMapper.Map<ELog.Core.Entities.Consumption>(material);
                await _consumptionRepository.UpdateAsync(cubicleAssignmentDetailToUpdate);

            }
            CurrentUnitOfWork.SaveChanges();
            //return await GetAsync(input);
            return (input);
        }

        public async Task<PagedResultDto<ConsumptionListDto>> GetAllAsync(PagedConsumptionResultRequestDto input)
        {
            //var groupClosedStatus = await GetStatusIdOfStatus(closedStatus);

            var query = CreateCubicleAssignmentListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            //var groupEntities = ApplyGrouping(entities.ToList());

            return new PagedResultDto<ConsumptionListDto>(
                totalCount,
                entities
            );
        }


        protected IQueryable<ConsumptionListDto> ApplySorting(IQueryable<ConsumptionListDto> query, PagedConsumptionResultRequestDto input)
        {
            //Try to sort query if available
            ISortedResultRequest sortInput = input;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<ConsumptionListDto> ApplyPaging(IQueryable<ConsumptionListDto> query, PagedConsumptionResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected IQueryable<ConsumptionListDto> CreateCubicleAssignmentListFilteredQuery(PagedConsumptionResultRequestDto input)
        {
            //var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            var cubicleAssignmentQuery = from consum in _consumptionRepository.GetAll()
                                         join po in _processOrderRepository.GetAll()
                                         on consum.ProcessOrderId equals po.Id into pos
                                         from po in pos.DefaultIfEmpty()
                                         join cube in _cubicleRepository.GetAll()
                                         on consum.CubicleId equals cube.Id into spos
                                         from cube in spos.DefaultIfEmpty()
                                         join equp in _equipmentRepository.GetAll()
                                         on consum.EquipmentId equals equp.Id into eq
                                         from equp in eq.DefaultIfEmpty()
                                         select new ConsumptionListDto
                                         {
                                             Id = consum.Id,
                                             CubicleId = cube.Id,
                                             processBarcodeId = cube.Id,
                                             CubicalCode = cube.CubicleCode,
                                             //ProductId=po.ProductCodeId,--Replace Processorder table to ProcessorderAfterRelease
                                             ProductCode = po.ProductCode,
                                             ProcessOrderId = po.Id,
                                             ProcessOrderNo = po.ProcessOrderNo,
                                             EquipmentId = equp.Id,
                                             equipmentBracodeId = equp.Id,
                                             EquipmentNo = equp.EquipmentCode
                                         };
            if (input.ProductId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.ProductCode == input.ProductId);
            }
            if (input.ProcessOrderId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }
            if (input.processBarcodeId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.CubicleId == input.processBarcodeId);
            }
            if (input.equipmentBracodeId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.EquipmentId == input.equipmentBracodeId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.EquipmentNo.Contains(input.Keyword) || x.CubicalCode.Contains(input.Keyword) || x.ProductCode.Contains(input.Keyword) || x.ProcessOrderNo.Contains(input.Keyword));

            }

            return cubicleAssignmentQuery;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _processOrderRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   //   Id = po.Id,
                                   PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }
        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeNewAsync(string input)
        {
            //var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _masterMasterRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   //   Id = po.Id,

                                   Value = po.MaterialCode,
                               };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
        {
            if ((input != null) || (input == null))
            {
                var processOrders = await _processOrderRepository.GetAll().Where(x => x.ProductCode == input).Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                }).ToListAsync();
                return processOrders;
            }

            return default;
        }



        public async Task<List<SelectListDto>> GetMaterialsOfProductCodeAsync(string input, int processOrderId)
        {
            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId
                           where po.Id == processOrderId
                           select new { po.ProductCode, po.Id, material.MaterialCode };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                batchNos = batchNos.Where(x => x.ProductCode.Contains(input)).Distinct();
                return await batchNos.Select(x => new SelectListDto { Id = x.Id, Value = x.MaterialCode }).ToListAsync()
                ?? default;
            }
            return default;
        }


        public async Task<List<ConsumptionDetailDto>> GetBatchNosDetailsAsync(string input, string materialid)
        {
            //var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            //var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

            //var cubicleAssignmentCancelledStatus = await _statusRepository.GetAll()
            //.Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == cancelledStatus).Select(a => a.Id).FirstOrDefaultAsync();

            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId
                           where material.MaterialCode == materialid
                           select new ConsumptionDetailDto
                           {
                               ProcessOrderMaterialId = material.Id,

                               LineItemNo = material.LineItemNo,
                               MaterialCode = material.MaterialCode,
                               ProductCode = po.ProductCode,
                               MaterialDescription = material.MaterialDescription,
                               BatchNo = material.ProductBatchNo,
                               SAPBatchNumber = material.SAPBatchNo,
                               Qty = material.Quantity,
                               UnitOfMeasurement = material.UOM,

                               ExpiryDate = material.ExpiryDate != null ? material.ExpiryDate.ToShortDateString() : null,
                               RetestDate = material.RetestDate != null ? material.RetestDate.ToShortDateString() : null,
                               ARNo = material.ARNO,
                           };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                batchNos = batchNos.Where(x => x.ProductCode.Contains(input));
                return await batchNos.Select(material => new ConsumptionDetailDto
                {
                    ProcessOrderMaterialId = material.ProcessOrderMaterialId,
                    LineItemNo = material.LineItemNo,
                    MaterialCode = material.MaterialCode,
                    ProductCode = material.ProductCode,
                    MaterialDescription = material.MaterialDescription,
                    BatchNo = material.BatchNo,
                    SAPBatchNumber = material.SAPBatchNumber,
                    //ProcessOrderId = material.ProcessOrderId,
                    //ProcessOrderNo = material.ProcessOrderNo,
                    //CubicleId = material.CubicleId,
                    Qty = material.Qty,
                    UnitOfMeasurement = material.UnitOfMeasurement,
                    UnitOfMeasurementId = material.UnitOfMeasurementId,
                    ExpiryDate = material.ExpiryDate,
                    RetestDate = material.RetestDate,
                    ARNo = material.ARNo
                })
               .ToListAsync() ?? default;
            }
            return default;
        }

        public async Task<ConsumptionDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _consumptionRepository.GetAsync(input.Id);
            var cubicleAssignment = ObjectMapper.Map<ConsumptionDto>(entity);
            //var cancelledStatusId = await GetStatusIdOfStatus(cancelledStatus);
            cubicleAssignment.ConsumptionDetails = await (from detail in _consumptionDetails.GetAll()
                                                          join con in _consumptionRepository.GetAll()
                                                          on detail.ConsumptionId equals con.Id
                                                          join POafter in _processOrderRepository.GetAll()
                                                          on con.ProcessOrderId equals POafter.Id
                                                          join material in _processOrdermaterialRepository.GetAll()
                                                            on POafter.Id equals material.ProcessOrderId into poms
                                                          from material in poms.DefaultIfEmpty()
                                                          where detail.ConsumptionId == input.Id

                                                          //join material in _processOrdermaterialRepository.GetAll()
                                                          //// on detail.LineItemNo equals material.LineItemNo into poms
                                                          //on detail. equals material.LineItemNo into poms
                                                          //from material in poms.DefaultIfEmpty()
                                                          //where detail.ConsumptionId == input.Id
                                                          select new ConsumptionDetailDto
                                                          {
                                                              Id = material.Id,
                                                              ConsumptionId = detail.ConsumptionId,
                                                              ProcessOrderMaterialId = detail.Id,
                                                              //ProcessOrderId = material.ProcessOrderId,
                                                              LineItemNo = material.LineItemNo,
                                                              MaterialCode = material.MaterialCode,
                                                              MaterialDescription = material.MaterialDescription,
                                                              BatchNo = material.ProductBatchNo,
                                                              SAPBatchNumber = material.SAPBatchNo,
                                                              Qty = material.Quantity,
                                                              UnitOfMeasurement = material.UOM,
                                                              //UnitOfMeasurementId = material.UnitOfMeasurementId,
                                                              ExpiryDate = material.ExpiryDate != null ? material.ExpiryDate.ToShortDateString() : null,
                                                              RetestDate = material.RetestDate != null ? material.RetestDate.ToShortDateString() : null,
                                                              ARNo = material.ARNO
                                                          }).ToListAsync();
            return cubicleAssignment;
        }


        public async Task<dynamic> GetProcessOrderByCubicleIdAsync(EntityDto<int> input)
        {


            var query = await (from consum in _consumptionRepository.GetAll()
                               join po in _processOrderRepository.GetAll()
                               on consum.ProcessOrderId equals po.Id into pos
                               from po in pos.DefaultIfEmpty()
                               join cube in _cubicleRepository.GetAll()
                               on consum.CubicleId equals cube.Id into spos
                               from cube in spos.DefaultIfEmpty()

                               select new
                               {
                                   ProcessOrderId = consum.ProcessOrderId,
                                   CubicleId = consum.CubicleId,
                                   ProductId = consum.ProductId,
                                   EquipmentId = consum.EquipmentId,
                                   ProcessOrderNo = po.ProcessOrderNo,
                                   CubicleCode = cube.CubicleCode
                               }).ToListAsync();
            return query;
        }

    }
}
