//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ELog.Application.QCSampling
//{
//   public class QCSamplingService
//    {
//    }
//}







using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonService.Invoices;
using ELog.Application.CommonService.Inward;
using ELog.Application.QCSampling;
using ELog.Application.QCSampling.Dto;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

//using static ELog.Core.PMMSEnums;

namespace ELog.Application.Inward.MaterialInspections
{
    [PMMSAuthorize]
    public class QCSamplingService : ApplicationService, IQCSamplingService
    {
        private readonly IRepository<QC_Sampling> _QCSamplingRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<GateEntry> _gateEntryRepository;
        private readonly IRepository<DeviceMaster> _deviceRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<MaterialInspectionHeader> _materialHeaderRepository;
        private readonly IRepository<MaterialInspectionRelationDetail> _materialInspectionRelationDetailRepository;
        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentRepository;
        private readonly IRepository<MaterialChecklistDetail> _materialChecklistDetailRepository;
        private readonly IRepository<MaterialDamageDetail> _materialDamageDetailRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInvoiceService _invoiceService;
        private readonly InwardAppService _inwardAppService;
        private readonly IRepository<TransactionStatusMaster> _transactionStatusRepository;
        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionHeaderRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public QCSamplingService(IRepository<QC_Sampling> QCSamplingRepository,
           IRepository<PurchaseOrder> purchaseOrderRepository,
          IRepository<DeviceMaster> deviceRepository,
          IRepository<GateEntry> gateEntryRepository,
          IRepository<InvoiceDetail> invoiceDetailRepository,
          IRepository<MaterialInspectionHeader> materialHeaderRepository,
          IRepository<MaterialInspectionRelationDetail> materialInspectionRelationDetailRepository,
          IRepository<MaterialConsignmentDetail> materialConsignmentRepository,
          IRepository<MaterialChecklistDetail> materialChecklistDetailRepository,
          IRepository<MaterialDamageDetail> materialDamageDetailRepository,
          IRepository<CheckpointMaster> checkpointRepository,
          IRepository<ModeMaster> modeRepository,
         ISessionAppService sessionAppService,
         IInvoiceService invoiceService,
          InwardAppService inwardAppService,
          IRepository<TransactionStatusMaster> transactionStatusRepository,
          IRepository<VehicleInspectionHeader> vehicleInspectionHeaderRepository,
         IHttpContextAccessor httpContextAccessor)
        {
            _QCSamplingRepository = QCSamplingRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _purchaseOrderRepository = purchaseOrderRepository;
            _gateEntryRepository = gateEntryRepository;
            _deviceRepository = deviceRepository;
            _httpContextAccessor = httpContextAccessor;
            _invoiceDetailRepository = invoiceDetailRepository;
            _materialHeaderRepository = materialHeaderRepository;
            _materialInspectionRelationDetailRepository = materialInspectionRelationDetailRepository;
            _materialConsignmentRepository = materialConsignmentRepository;
            _materialChecklistDetailRepository = materialChecklistDetailRepository;
            _materialDamageDetailRepository = materialDamageDetailRepository;
            _checkpointRepository = checkpointRepository;
            _modeRepository = modeRepository;
            _invoiceService = invoiceService;
            _sessionAppService = sessionAppService;
            _inwardAppService = inwardAppService;
            _transactionStatusRepository = transactionStatusRepository;
            _vehicleInspectionHeaderRepository = vehicleInspectionHeaderRepository;
        }

        // [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]
        [PMMSAuthorize]
        public async Task<QCSamplingDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _QCSamplingRepository.GetAsync(input.Id);
            return ObjectMapper.Map<QCSamplingDto>(entity);
        }


        //[PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]
        //public async Task<MaterialInspectionDto> GetWithMaterialAsync(MaterialEntityDto input)
        //{
        //    var sessionUser = await _sessionAppService.GetCurrentLoginInformations();
        //    var materialInspectionHeader = await _materialHeaderRepository.GetAsync(input.Id);
        //    var isControllerMode = await _modeRepository.FirstOrDefaultAsync(a => a.ModeName == PMMSConsts.Quality && a.IsController);
        //    var materialInspectionDto = new MaterialInspectionDto
        //    {
        //        MaterialId = input.MaterialId,
        //        Id = input.Id,
        //        MaterialInspectionTransactionId = materialInspectionHeader.TransactionStatusId
        //    };

        //    var materialRelation = _materialInspectionRelationDetailRepository.GetAllIncluding(x => x.MaterialConsignments, x => x.MaterialDamageDetails, x => x.MaterialCheckpoints)
        //         .Where(x => x.MaterialHeaderId == input.Id && x.MaterialId == input.MaterialId)
        //         .FirstOrDefault();
        //    if (materialRelation == null)
        //    {
        //        return materialInspectionDto;
        //    }

        //    ObjectMapper.Map(materialRelation, materialInspectionDto);
        //    materialInspectionDto.MaterialTransactionId = materialRelation.TransactionStatusId;
        //    materialInspectionDto.MaterialCheckpoints = await (from detail in _materialChecklistDetailRepository.GetAll()
        //                                                       join checkpoint in _checkpointRepository.GetAll()
        //                                                       on detail.CheckPointId equals checkpoint.Id
        //                                                       where detail.MaterialRelationId == materialRelation.Id
        //                                                       select new CheckpointDto
        //                                                       {
        //                                                           Id = detail.Id,
        //                                                           CheckPointId = checkpoint.Id,
        //                                                           InspectionChecklistId = materialRelation.InspectionChecklistId,
        //                                                           CheckpointName = checkpoint.CheckpointName,
        //                                                           ValueTag = checkpoint.ValueTag,
        //                                                           AcceptanceValue = checkpoint.AcceptanceValue,
        //                                                           CheckpointTypeId = checkpoint.CheckpointTypeId,
        //                                                           ModeId = checkpoint.ModeId,
        //                                                           Observation = detail.Observation,
        //                                                           DiscrepancyRemark = detail.DiscrepancyRemark,
        //                                                           IsControllerMode = isControllerMode != null && isControllerMode.Id == checkpoint.ModeId
        //                                                       }).ToListAsync() ?? default;
        //    if (materialInspectionDto.MaterialTransactionId == (int)PMMSEnums.TransactionStatus.OnHold &&
        //        sessionUser.User.IsControllerMode &&
        //        !materialInspectionDto.MaterialCheckpoints.Any(a => a.IsControllerMode))
        //    {
        //        var qaCheckpoints = await _inwardAppService.GetCheckpointsByChecklistIdAsync(materialInspectionDto.InspectionChecklistId.GetValueOrDefault(), sessionUser.User.ModeId);
        //        qaCheckpoints.ForEach(x => x.IsControllerMode = isControllerMode.Id == x.ModeId);
        //        materialInspectionDto.MaterialCheckpoints.AddRange(qaCheckpoints);
        //    }
        //    return materialInspectionDto;
        //}

        //  [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]

        [PMMSAuthorize]
        public async Task<PagedResultDto<QCSamplingDto>> GetAllAsync(PagedQCSamplingResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<QCSamplingDto>(
                totalCount,
                entities
            );
        }


        protected IQueryable<QCSamplingDto> ApplySorting(IQueryable<QCSamplingDto> query, PagedQCSamplingResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
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
        protected IQueryable<QCSamplingDto> ApplyPaging(IQueryable<QCSamplingDto> query, PagedQCSamplingResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }




        protected IQueryable<QCSamplingDto> CreateUserListFilteredQuery(PagedQCSamplingResultRequestDto input)
        {
            var clientQuery =
                from client in _QCSamplingRepository.GetAll()

                select new QCSamplingDto
                {
                    Id = client.Id,
                    MaterialType = client.MaterialType,
                    InspectionLevel = client.InspectionLevel,
                    LotQuantityMin = client.LotQuantityMin,
                    LotQuantityMax = client.LotQuantityMax,
                    InspectionQuantity = client.InspectionQuantity,

                };


            if (input.MaterialType != null)
            {
                clientQuery = clientQuery.Where(x => x.MaterialType == input.MaterialType);
            }
            if (input.InspectionLevel != null)
            {
                clientQuery = clientQuery.Where(x => x.InspectionLevel == input.InspectionLevel);
            }
            //if (input.LotQuantityMin != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.LotQuantityMin == input.LotQuantityMin);
            //}
            if (input.InspectionQuantity != null)
            {
                clientQuery = clientQuery.Where(x => x.InspectionQuantity == input.InspectionQuantity);
            }
            if (input.LotQuantityMin != null)
            {
                clientQuery = clientQuery.Where(x => x.LotQuantityMin == input.LotQuantityMin);
            }
            if (input.LotQuantityMax != null)
            {
                clientQuery = clientQuery.Where(x => x.LotQuantityMax == input.LotQuantityMax);
            }




            //if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            //{
            //    gateEntryQuery = gateEntryQuery.Where(x => x.GatePassNumber.Contains(input.Keyword) || x.GatePassNumber.Contains(input.Keyword));
            //}
            //if (input.ActiveInactiveStatusId != null)
            //{
            //    if (input.ActiveInactiveStatusId == (int)Status.In_Active)
            //    {
            //        gateEntryQuery = gateEntryQuery.Where(x => !x.IsActive);
            //    }
            //    else if (input.ActiveInactiveStatusId == (int)Status.Active)
            //    {
            //        gateEntryQuery = gateEntryQuery.Where(x => x.IsActive);
            //    }
            //}
            return clientQuery;
        }


    }
}