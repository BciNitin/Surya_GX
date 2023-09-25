using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.CubicleAssignments.Dto;
using ELog.Application.Modules;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.CubicleAssignments
{
    [PMMSAuthorize]
    public class CubicleAssignmentAppService : ApplicationService, ICubicleAssignmentAppService
    {
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly IRepository<StatusMaster> _statusRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;

        private readonly IModuleAppService _moduleAppService;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<ProcessOrderMaterial> _processOrdermaterialRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string inProgressStatus = nameof(PMMSEnums.CubicleAssignementDetailStatus.InProgress).ToLower();
        private readonly string cancelledStatus = nameof(PMMSEnums.CubicleAssignementDetailStatus.Cancelled).ToLower();
        private readonly string closedStatus = nameof(PMMSEnums.CubicleAssignmentGroupStatus.Close).ToLower();
        private readonly string openStatus = nameof(PMMSEnums.CubicleAssignmentGroupStatus.Open).ToLower();

        public CubicleAssignmentAppService(IRepository<ProcessOrder> processOrderRepository,
          IRepository<StatusMaster> statusRepository,
          IHttpContextAccessor httpContextAccessor,
          IRepository<CubicleAssignmentHeader> cubicleAssignmentRepository,
          IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
            IModuleAppService moduleAppService, IRepository<ProcessOrderMaterial> processOrdermaterialRepository,
            IRepository<InspectionLot> inspectionLotRepository,
            IRepository<CubicleMaster> cubicleRepository,
            IDispensingAppService dispensingAppService)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _processOrderRepository = processOrderRepository;
            _httpContextAccessor = httpContextAccessor;
            _statusRepository = statusRepository;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _cubicleAssignmentRepository = cubicleAssignmentRepository;
            _moduleAppService = moduleAppService;
            _processOrdermaterialRepository = processOrdermaterialRepository;
            _dispensingAppService = dispensingAppService;
            _cubicleRepository = cubicleRepository;
            _inspectionLotRepository = inspectionLotRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<CubicleAssignmentDto> GetAsync(EntityDto<int> input)
        {
            return await GetCubicleAssignmentAsync(input.Id, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<CubicleAssignmentDto> GetSamplingAsync(EntityDto<int> input)
        {
            return await GetCubicleAssignmentAsync(input.Id, true);
        }

        private async Task<CubicleAssignmentDto> GetCubicleAssignmentAsync(int cubicleAssignmentId, bool isSampling)
        {
            var entity = await _cubicleAssignmentRepository.GetAsync(cubicleAssignmentId);
            var cubicleAssignment = ObjectMapper.Map<CubicleAssignmentDto>(entity);
            var cubicleAssignmentCancelledStatusId = await GetStatusIdOfStatus(cancelledStatus);
            var cubicleAssignmentDetails = new List<CubicleAssignmentDetailsDto>();
            if (!isSampling)
            {
                cubicleAssignment.CubicleAssignmentDetails = await (from detail in _cubicleAssignmentDetailRepository.GetAll()
                                                                    join po in _processOrderRepository.GetAll()
                                                                    on detail.ProcessOrderId equals po.Id
                                                                    join material in _processOrdermaterialRepository.GetAll()
                                                                    on detail.ProcessOrderMaterialId equals material.Id
                                                                    join cubicle in _cubicleRepository.GetAll()
                                                                    on detail.CubicleId equals cubicle.Id into cps
                                                                    from cubicle in cps.DefaultIfEmpty()
                                                                    where detail.CubicleAssignmentHeaderId == cubicleAssignmentId && detail.StatusId != cubicleAssignmentCancelledStatusId
                                                                    select new CubicleAssignmentDetailsDto
                                                                    {
                                                                        Id = detail.Id,
                                                                        CubicleAssignmentHeaderId = detail.CubicleAssignmentHeaderId,
                                                                        ProcessOrderMaterialId = material.Id,
                                                                        ProcessOrderId = material.ProcessOrderId,
                                                                        ProcessOrderNo = material.ProcessOrderNo,
                                                                        LineItemNo = material.ItemNo,
                                                                        MaterialCode = material.ItemCode,
                                                                        MaterialDescription = material.ItemDescription,
                                                                        BatchNo = material.BatchNo,
                                                                        SAPBatchNumber = material.SAPBatchNo,
                                                                        Qty = material.OrderQuantity,
                                                                        UnitOfMeasurement = material.UnitOfMeasurement,
                                                                        UnitOfMeasurementId = material.UnitOfMeasurementId,
                                                                        ExpiryDate = material.ExpiryDate != default ? material.ExpiryDate.ToShortDateString() : null,
                                                                        RetestDate = material.RetestDate != default ? material.RetestDate.ToShortDateString() : null,
                                                                        ARNo = material.ARNo,
                                                                        CubicleId = detail.CubicleId,
                                                                        CubicleBarcode = cubicle.CubicleCode,
                                                                        IsAssigned = false,
                                                                        StatusId = detail.StatusId,
                                                                        TenantId = detail.TenantId,
                                                                        ProductCode = po.ProductCode,
                                                                        IsReservationNo = po.IsReservationNo
                                                                    }).ToListAsync();
                return cubicleAssignment;
            }
            else
            {
                cubicleAssignment.CubicleAssignmentDetails = await (from detail in _cubicleAssignmentDetailRepository.GetAll()
                                                                    join inspectionLot in _inspectionLotRepository.GetAll()
                                                                    on detail.InspectionLotId equals inspectionLot.Id
                                                                    join material in _processOrdermaterialRepository.GetAll()
                                                                    on detail.ProcessOrderMaterialId equals material.Id
                                                                    join cubicle in _cubicleRepository.GetAll()
                                                                    on detail.CubicleId equals cubicle.Id into cps
                                                                    from cubicle in cps.DefaultIfEmpty()
                                                                    where detail.CubicleAssignmentHeaderId == cubicleAssignment.Id && detail.StatusId != cubicleAssignmentCancelledStatusId
                                                                    select new CubicleAssignmentDetailsDto
                                                                    {
                                                                        Id = detail.Id,
                                                                        ProductCode = inspectionLot.ProductCode,
                                                                        CubicleAssignmentHeaderId = detail.CubicleAssignmentHeaderId,
                                                                        ProcessOrderMaterialId = material.Id,
                                                                        InspectionLotId = inspectionLot.Id,
                                                                        InspectionLotNo = inspectionLot.InspectionLotNumber,
                                                                        LineItemNo = material.ItemNo,
                                                                        MaterialCode = material.ItemCode,
                                                                        MaterialDescription = material.ItemDescription,
                                                                        BatchNo = material.BatchNo,
                                                                        SAPBatchNumber = material.SAPBatchNo,
                                                                        Qty = material.OrderQuantity,
                                                                        UnitOfMeasurement = material.UnitOfMeasurement,
                                                                        UnitOfMeasurementId = material.UnitOfMeasurementId,
                                                                        ExpiryDate = material.ExpiryDate != default ? material.ExpiryDate.ToShortDateString() : null,
                                                                        RetestDate = material.RetestDate != default ? material.RetestDate.ToShortDateString() : null,
                                                                        ARNo = material.ARNo,
                                                                        CubicleId = detail.CubicleId,
                                                                        CubicleBarcode = cubicle.CubicleCode,
                                                                        IsAssigned = false,
                                                                        StatusId = detail.StatusId,
                                                                        TenantId = detail.TenantId,
                                                                        IsReservationNo = false
                                                                    }).ToListAsync();
                return cubicleAssignment;
            }
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<CubicleAssignmentListDto>> GetAllAsync(PagedCubicleAssignmentResultRequestDto input)
        {
            return await GetAllCubicleAssignmentAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<CubicleAssignmentListDto>> GetAllSamplingAsync(PagedCubicleAssignmentResultRequestDto input)
        {
            return await GetAllCubicleAssignmentAsync(input, true);
        }

        private async Task<PagedResultDto<CubicleAssignmentListDto>> GetAllCubicleAssignmentAsync(PagedCubicleAssignmentResultRequestDto input, bool isSampling)
        {
            var groupClosedStatus = await GetStatusIdOfStatus(closedStatus);
            _ = Enumerable.Empty<CubicleAssignmentListDto>().AsQueryable();
            IQueryable<CubicleAssignmentListDto> query;
            if (!isSampling)
            {
                query = CreateDispencingListFilteredQuery(input, groupClosedStatus);
            }
            else
            {
                query = CreateSamplingListFilteredQuery(input, groupClosedStatus);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var groupEntities = ApplyGrouping(entities.ToList());

            return new PagedResultDto<CubicleAssignmentListDto>(
                totalCount,
                groupEntities
            );
        }

        public List<CubicleAssignmentListDto> ApplyGrouping(List<CubicleAssignmentListDto> query)
        {
            return query.GroupBy(p => new { p.GroupCode }).Select(cubicleAssign => new CubicleAssignmentListDto()
            {
                Id = cubicleAssign.First().Id,
                ProcessOrderId = cubicleAssign.First().ProcessOrderId,
                ProcessOrderNo = string.Join(",", cubicleAssign.Select(a => a.ProcessOrderNo).Distinct()),
                InspectionLotId = cubicleAssign.First().InspectionLotId,
                InspectionLotNo = string.Join(",", cubicleAssign.Select(a => a.InspectionLotNo).Distinct()),
                GroupCode = cubicleAssign.First().GroupCode,
                SubPlantId = cubicleAssign.First().SubPlantId,
                GroupStatus = cubicleAssign.First().GroupStatus,
                GroupStatusId = cubicleAssign.First().GroupStatusId,
            }).ToList();
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CubicleAssignmentDto> CreateAsync(CreateCubicleAssignmentDto input)
        {
            return await CreateCubileAssignmentAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CubicleAssignmentDto> CreateSamplingAsync(CreateCubicleAssignmentDto input)
        {
            return await CreateCubileAssignmentAsync(input, true);
        }

        private async Task<CubicleAssignmentDto> CreateCubileAssignmentAsync(CreateCubicleAssignmentDto input, bool isSampling)
        {
            var cubicleAssignment = ObjectMapper.Map<CubicleAssignmentHeader>(input);
            cubicleAssignment.TenantId = AbpSession.TenantId;
            var currentDate = DateTime.UtcNow;
            cubicleAssignment.GroupId = isSampling ? $"IGRP{currentDate:yyyyMMddhhmmssff}" : $"PGRP{currentDate:yyyyMMddhhmmssff}";

            var groupOpenStatus = await GetStatusIdOfStatus(openStatus);
            var cubicleAssignmentInProgressStatus = await GetStatusIdOfStatus(inProgressStatus);

            cubicleAssignment.GroupStatusId = groupOpenStatus;
            cubicleAssignment.CubicleAssignmentDate = DateTime.UtcNow;

            cubicleAssignment.CubicleAssignmentDetails = new List<CubicleAssignmentDetail>();
            foreach (var detail in input.CubicleAssignmentDetails)
            {
                var cubicleAssignmentDetail = ObjectMapper.Map<CubicleAssignmentDetail>(detail);
                cubicleAssignmentDetail.TenantId = AbpSession.TenantId;
                cubicleAssignmentDetail.StatusId = cubicleAssignmentInProgressStatus;
                cubicleAssignment.CubicleAssignmentDetails.Add(cubicleAssignmentDetail);
            }
            await _cubicleAssignmentRepository.InsertAsync(cubicleAssignment);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CubicleAssignmentDto>(cubicleAssignment);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.CubicleAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CubicleAssignmentDto> UpdateAsync(CubicleAssignmentDto input)
        {
            return await AssinDeAssignCubicelAssinmentsAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingCubicleAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<CubicleAssignmentDto> UpdateSamplingAsync(CubicleAssignmentDto input)
        {
            return await AssinDeAssignCubicelAssinmentsAsync(input, true);
        }

        private async Task<CubicleAssignmentDto> AssinDeAssignCubicelAssinmentsAsync(CubicleAssignmentDto input, bool isSampling)
        {
            var cubicleAssignment = await _cubicleAssignmentRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, cubicleAssignment);
            await _cubicleAssignmentRepository.UpdateAsync(cubicleAssignment);
            var cubicleAssignmentDetails = await _cubicleAssignmentDetailRepository.GetAll().Where(x => x.CubicleAssignmentHeaderId == input.Id).ToListAsync();

            foreach (var material in input.CubicleAssignmentDetails)
            {
                if (!material.IsAssigned)
                {
                    await _dispensingAppService.RejectLineClearanceTransaction(material.CubicleId, input.Id, isSampling);
                    material.CubicleId = null;
                }
                var materialToUpdate = cubicleAssignmentDetails.FirstOrDefault(x => x.Id == material.Id);
                if (materialToUpdate != null)
                {
                    ObjectMapper.Map(material, materialToUpdate);
                    await _cubicleAssignmentDetailRepository.UpdateAsync(materialToUpdate);
                }
            }
            if (!isSampling)
            {
                return await GetAsync(input);
            }
            else
            {
                return await GetSamplingAsync(input);
            }
        }
        private async Task<int> GetStatusIdOfStatus(string status)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

            return await _statusRepository.GetAll()
                                         .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == status)
                                         .Select(a => a.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<CubicleAssignmentListDto> ApplySorting(IQueryable<CubicleAssignmentListDto> query, PagedCubicleAssignmentResultRequestDto input)
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
        protected IQueryable<CubicleAssignmentListDto> ApplyPaging(IQueryable<CubicleAssignmentListDto> query, PagedCubicleAssignmentResultRequestDto input)
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

        protected IQueryable<CubicleAssignmentListDto> CreateDispencingListFilteredQuery(PagedCubicleAssignmentResultRequestDto input, int groupClosedStatus)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            var cubicleAssignmentDispencingQuery = from cubicleAssign in _cubicleAssignmentRepository.GetAll()
                                                   join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                   on cubicleAssign.Id equals cubicleDetail.CubicleAssignmentHeaderId into cds
                                                   from cubicleDetail in cds.DefaultIfEmpty()
                                                   join po in _processOrderRepository.GetAll()
                                                   on cubicleDetail.ProcessOrderId equals po.Id into pos
                                                   from po in pos.DefaultIfEmpty()
                                                   join status in _statusRepository.GetAll()
                                                   on cubicleAssign.GroupStatusId equals status.Id into spos
                                                   from status in spos.DefaultIfEmpty()
                                                   where cubicleAssign.GroupStatusId != groupClosedStatus && !cubicleAssign.IsSampling
                                                   select new CubicleAssignmentListDto
                                                   {
                                                       Id = cubicleAssign.Id,
                                                       ProcessOrderId = cubicleDetail.ProcessOrderId,
                                                       ProcessOrderNo = po.ProcessOrderNo,
                                                       GroupCode = cubicleAssign.GroupId,
                                                       SubPlantId = po.PlantId,
                                                       GroupStatus = status.Status,
                                                       GroupStatusId = cubicleAssign.Id
                                                   };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                cubicleAssignmentDispencingQuery = cubicleAssignmentDispencingQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (input.ProcessOrderId != null)
            {
                cubicleAssignmentDispencingQuery = cubicleAssignmentDispencingQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                cubicleAssignmentDispencingQuery = cubicleAssignmentDispencingQuery.Where(x => x.GroupCode.Contains(input.Keyword));
            }
            return cubicleAssignmentDispencingQuery;
        }

        protected IQueryable<CubicleAssignmentListDto> CreateSamplingListFilteredQuery(PagedCubicleAssignmentResultRequestDto input, int groupClosedStatus)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            var cubicleAssignmentSamplingQuery = from cubicleAssign in _cubicleAssignmentRepository.GetAll()
                                                 join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                 on cubicleAssign.Id equals cubicleDetail.CubicleAssignmentHeaderId into cds
                                                 from cubicleDetail in cds.DefaultIfEmpty()
                                                 join inspetionLot in _inspectionLotRepository.GetAll()
                                                 on cubicleDetail.InspectionLotId equals inspetionLot.Id into lotps
                                                 from inspetionLot in lotps.DefaultIfEmpty()
                                                 join status in _statusRepository.GetAll()
                                                 on cubicleAssign.GroupStatusId equals status.Id into spos
                                                 from status in spos.DefaultIfEmpty()
                                                 where cubicleAssign.GroupStatusId != groupClosedStatus && cubicleAssign.IsSampling
                                                 select new CubicleAssignmentListDto
                                                 {
                                                     Id = cubicleAssign.Id,
                                                     InspectionLotId = inspetionLot.Id,
                                                     InspectionLotNo = inspetionLot.InspectionLotNumber,
                                                     GroupCode = cubicleAssign.GroupId,
                                                     SubPlantId = inspetionLot.PlantId,
                                                     GroupStatus = status.Status,
                                                     GroupStatusId = cubicleAssign.Id
                                                 };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                cubicleAssignmentSamplingQuery = cubicleAssignmentSamplingQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (input.InspectionLotId != null)
            {
                cubicleAssignmentSamplingQuery = cubicleAssignmentSamplingQuery.Where(x => x.InspectionLotId == input.InspectionLotId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                cubicleAssignmentSamplingQuery = cubicleAssignmentSamplingQuery.Where(x => x.GroupCode.Contains(input.Keyword));
            }
            return cubicleAssignmentSamplingQuery;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input, bool isSampling, int? processOrderId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = GetAllCubicleAssignmentProductCodeAsync(isSampling, plantId, input, processOrderId);
            return await productCodes ?? default;
        }

        private async Task<List<SelectListDtoWithPlantId>> GetAllCubicleAssignmentProductCodeAsync(bool isSampling, string plantId, string input, int? processOrderId)
        {
            var productCodes = Enumerable.Empty<SelectListDtoWithPlantId>().AsQueryable();
            if (!isSampling)
            {
                productCodes = from po in _processOrderRepository.GetAll()
                               where po.Id == processOrderId
                               select new SelectListDtoWithPlantId
                               {
                                   PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };
            }
            else
            {
                productCodes = from inspectionLot in _inspectionLotRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   PlantId = inspectionLot.PlantId,
                                   Value = inspectionLot.ProductCode,
                               };
            }
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

        public async Task<List<SelectListDtoWithPlantId>> GetProcessOrdersOfProductCodeAsync(string input)
        {
            if (input == null)
            {
                return await _processOrderRepository.GetAll().Select(po => new SelectListDtoWithPlantId
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                    IsReservationNo = po.IsReservationNo
                }).ToListAsync();
            }
            else
            {
                var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
                var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

                var groupOpenStatus = await _statusRepository.GetAll()
                                    .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == openStatus).Select(a => a.Id).FirstOrDefaultAsync();

                var assingedProcessOrders = from po in _processOrderRepository.GetAll()
                                            join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                            on po.Id equals cubicleDetail.ProcessOrderId
                                            join cubicle in _cubicleAssignmentRepository.GetAll()
                                            on cubicleDetail.CubicleAssignmentHeaderId equals cubicle.Id
                                            select new
                                            {
                                                po.Id,
                                                po.ProcessOrderNo,
                                                po.ProductCode,
                                                cubicle.GroupStatusId,
                                                po.IsReservationNo,
                                            };
                var openProcessOrders = assingedProcessOrders.Where(a => a.GroupStatusId == groupOpenStatus);
                var closedProcessOrders = assingedProcessOrders.Where(a => !openProcessOrders.Select(a => a.Id).Contains(a.Id));

                var notAssignedProcessOrders = from po in _processOrderRepository.GetAll()
                                               join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                               on po.Id equals cubicleDetail.ProcessOrderId into ps
                                               from cubicleDetail in ps.DefaultIfEmpty()
                                               join cubicle in _cubicleAssignmentRepository.GetAll()
                                               on cubicleDetail.CubicleAssignmentHeaderId equals cubicle.Id into dps
                                               from cubicle in dps.DefaultIfEmpty()
                                               where cubicleDetail.Id == null
                                               select new
                                               {
                                                   po.Id,
                                                   po.ProcessOrderNo,
                                                   po.ProductCode,
                                                   cubicle.GroupStatusId,
                                                   po.IsReservationNo,
                                               };
                var processOrdersOfProdcutCode = closedProcessOrders.Distinct().Union(notAssignedProcessOrders.Distinct());
                return await processOrdersOfProdcutCode.Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.ProcessOrderNo, IsReservationNo = x.IsReservationNo })
               .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetInspectionLotNoOfProductCodeAsync(string input)
        {
            if (input == null)
            {
                return await _inspectionLotRepository.GetAll().Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.InspectionLotNumber,
                }).ToListAsync();
            }
            else
            {
                var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
                var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

                var groupOpenStatus = await _statusRepository.GetAll()
                                    .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == openStatus).Select(a => a.Id).FirstOrDefaultAsync();

                var assingedInspectionLotNos = from po in _inspectionLotRepository.GetAll()
                                               join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                               on po.Id equals cubicleDetail.InspectionLotId
                                               join cubicle in _cubicleAssignmentRepository.GetAll()
                                               on cubicleDetail.CubicleAssignmentHeaderId equals cubicle.Id
                                               select new
                                               {
                                                   po.Id,
                                                   po.InspectionLotNumber,
                                                   po.ProductCode,
                                                   cubicle.GroupStatusId
                                               };
                var openInspectionLotNos = assingedInspectionLotNos.Where(a => a.GroupStatusId == groupOpenStatus);
                var closedInspectionLotNos = assingedInspectionLotNos.Where(a => !openInspectionLotNos.Select(a => a.Id).Contains(a.Id));

                var notAssignedInspectionLotNos = from po in _inspectionLotRepository.GetAll()
                                                  join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                  on po.Id equals cubicleDetail.InspectionLotId into ps
                                                  from cubicleDetail in ps.DefaultIfEmpty()
                                                  join cubicle in _cubicleAssignmentRepository.GetAll()
                                                  on cubicleDetail.CubicleAssignmentHeaderId equals cubicle.Id into dps
                                                  from cubicle in dps.DefaultIfEmpty()
                                                  where cubicleDetail.Id == null
                                                  select new
                                                  {
                                                      po.Id,
                                                      po.InspectionLotNumber,
                                                      po.ProductCode,
                                                      cubicle.GroupStatusId
                                                  };
                var inspectionLotNosOfProdcutCode = closedInspectionLotNos.Distinct().Union(notAssignedInspectionLotNos.Distinct());
                if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
                {
                    input = input.Trim();
                    inspectionLotNosOfProdcutCode = inspectionLotNosOfProdcutCode.Where(x => x.ProductCode.Contains(input));
                    return await inspectionLotNosOfProdcutCode.Select(x => new SelectListDto { Id = x.Id, Value = x.InspectionLotNumber })
                   .ToListAsync() ?? default;
                }
            }
            return default;
        }

        public async Task<List<SelectListDto>> GetBatchNosOfProductCodeAsync(string input, int processOrderId)
        {
            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId
                           where po.Id == processOrderId
                           select new { po.ProductCode, po.Id, material.BatchNo };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                batchNos = batchNos.Where(x => x.ProductCode.Contains(input)).Distinct();
                return await batchNos.Select(x => new SelectListDto { Id = x.Id, Value = x.BatchNo }).ToListAsync()
                ?? default;
            }
            return default;
        }

        public async Task<List<SelectListDto>> GetBatchNosOfProductCodeForSamplingAsync(string input, int processOrderId)
        {
            var batchNos = from inspectionLot in _inspectionLotRepository.GetAll()
                           join material in _processOrdermaterialRepository.GetAll()
                           on inspectionLot.Id equals material.InspectionLotId
                           where inspectionLot.Id == processOrderId
                           select new { inspectionLot.ProductCode, inspectionLot.Id, material.BatchNo };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                batchNos = batchNos.Where(x => x.ProductCode.Contains(input)).Distinct();
                return await batchNos.Select(x => new SelectListDto { Id = x.Id, Value = x.BatchNo }).ToListAsync()
                ?? default;
            }
            return default;
        }

        public async Task<List<CubicleAssignmentDetailsDto>> GetBatchNosDetailsAsync(string input, int processOrderId, bool isReservationNo)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

            var cubicleAssignmentCancelledStatus = await _statusRepository.GetAll()
                                .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == cancelledStatus).Select(a => a.Id).FirstOrDefaultAsync();

            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId
                           where po.Id == processOrderId
                           select new CubicleAssignmentDetailsDto
                           {
                               ProcessOrderMaterialId = material.Id,
                               ProcessOrderId = po.Id,
                               LineItemNo = material.ItemNo,
                               MaterialCode = material.ItemCode,
                               ProductCode = po.ProductCode,
                               MaterialDescription = material.ItemDescription,
                               BatchNo = material.BatchNo,
                               SAPBatchNumber = material.SAPBatchNo,
                               Qty = material.OrderQuantity,
                               UnitOfMeasurement = material.UnitOfMeasurement,
                               UnitOfMeasurementId = material.UnitOfMeasurementId,
                               ExpiryDate = material.ExpiryDate != null ? material.ExpiryDate.ToShortDateString() : null,
                               RetestDate = material.RetestDate != null ? material.RetestDate.ToShortDateString() : null,
                               ARNo = material.ARNo,
                               IsReservationNo = po.IsReservationNo
                           };

            if (!isReservationNo && !(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                batchNos = batchNos.Where(x => x.BatchNo.Contains(input));
            }
            return await batchNos.Select(material => new CubicleAssignmentDetailsDto
            {
                ProcessOrderMaterialId = material.ProcessOrderMaterialId,
                LineItemNo = material.LineItemNo,
                MaterialCode = material.MaterialCode,
                ProductCode = material.ProductCode,
                MaterialDescription = material.MaterialDescription,
                BatchNo = material.BatchNo,
                SAPBatchNumber = material.SAPBatchNumber,
                ProcessOrderId = material.ProcessOrderId,
                ProcessOrderNo = material.ProcessOrderNo,
                CubicleId = material.CubicleId,
                Qty = material.Qty,
                UnitOfMeasurement = material.UnitOfMeasurement,
                UnitOfMeasurementId = material.UnitOfMeasurementId,
                ExpiryDate = material.ExpiryDate,
                RetestDate = material.RetestDate,
                ARNo = material.ARNo
            })
           .ToListAsync() ?? default;
        }

        public async Task<List<CubicleAssignmentDetailsDto>> GetBatchNosDetailsForSamplingAsync(string input, int processOrderId)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleAssignmentSubModule);

            var cubicleAssignmentCancelledStatus = await _statusRepository.GetAll()
                                .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == cancelledStatus).Select(a => a.Id).FirstOrDefaultAsync();

            var samplingBatchNos = from inspectionLot in _inspectionLotRepository.GetAll()
                                   join material in _processOrdermaterialRepository.GetAll()
                                   on inspectionLot.Id equals material.InspectionLotId
                                   where inspectionLot.Id == processOrderId
                                   select new CubicleAssignmentDetailsDto
                                   {
                                       ProcessOrderMaterialId = material.Id,
                                       InspectionLotId = inspectionLot.Id,
                                       InspectionLotNo = inspectionLot.InspectionLotNumber,
                                       LineItemNo = material.ItemNo,
                                       MaterialCode = material.ItemCode,
                                       ProductCode = inspectionLot.ProductCode,
                                       MaterialDescription = material.ItemDescription,
                                       BatchNo = material.BatchNo,
                                       SAPBatchNumber = material.SAPBatchNo,
                                       Qty = material.OrderQuantity,
                                       UnitOfMeasurement = material.UnitOfMeasurement,
                                       UnitOfMeasurementId = material.UnitOfMeasurementId,
                                       ExpiryDate = material.ExpiryDate != null ? material.ExpiryDate.ToShortDateString() : null,
                                       RetestDate = material.RetestDate != null ? material.RetestDate.ToShortDateString() : null,
                                       ARNo = material.ARNo,
                                   };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                samplingBatchNos = samplingBatchNos.Where(x => x.BatchNo.Contains(input));
                return await samplingBatchNos.Select(material => new CubicleAssignmentDetailsDto
                {
                    ProcessOrderMaterialId = material.ProcessOrderMaterialId,
                    LineItemNo = material.LineItemNo,
                    MaterialCode = material.MaterialCode,
                    ProductCode = material.ProductCode,
                    MaterialDescription = material.MaterialDescription,
                    BatchNo = material.BatchNo,
                    SAPBatchNumber = material.SAPBatchNumber,
                    InspectionLotId = material.InspectionLotId,
                    InspectionLotNo = material.InspectionLotNo,
                    CubicleId = material.CubicleId,
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
    }
}