using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonService.Invoices;
using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.CommonService.Inward;
using ELog.Application.Inward.VehicleInspections.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Sessions;
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
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Inward.VehicleInspections
{
    [PMMSAuthorize]
    public class VehicleInspectionAppService : ApplicationService, IVehicleInspectionAppService
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<GateEntry> _gateEntryRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionHeaderRepository;
        private readonly IRepository<VehicleInspectionDetail> _vehicleInspectionDetailRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IInvoiceService _invoiceService;
        private readonly InwardAppService _inwardAppService;

        private readonly IRepository<TransactionStatusMaster> _transactionStatusRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleInspectionAppService(IRepository<PurchaseOrder> purchaseOrderRepository,
          IRepository<GateEntry> gateEntryRepository,
          IRepository<InvoiceDetail> invoiceDetailRepository,
          IRepository<CheckpointMaster> checkpointRepository,
          IRepository<ModeMaster> modeRepository,
          IHttpContextAccessor httpContextAccessor,
          IRepository<VehicleInspectionHeader> vehicleInspectionHeaderRepository,
          IRepository<VehicleInspectionDetail> vehicleInspectionDetailRepository,
          IInvoiceService invoiceService, ISessionAppService sessionAppService,
          InwardAppService inwardAppService,
          IRepository<TransactionStatusMaster> transactionStatusRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _purchaseOrderRepository = purchaseOrderRepository;
            _gateEntryRepository = gateEntryRepository;
            _httpContextAccessor = httpContextAccessor;
            _invoiceDetailRepository = invoiceDetailRepository;
            _checkpointRepository = checkpointRepository;
            _modeRepository = modeRepository;
            _invoiceService = invoiceService;
            _vehicleInspectionHeaderRepository = vehicleInspectionHeaderRepository;
            _vehicleInspectionDetailRepository = vehicleInspectionDetailRepository;
            _sessionAppService = sessionAppService;
            _inwardAppService = inwardAppService;
            _transactionStatusRepository = transactionStatusRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.VehicleInspection_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<VehicleInspectionDto> GetAsync(EntityDto<int> input)
        {
            var sessionUser = await _sessionAppService.GetCurrentLoginInformations();
            var entity = await _vehicleInspectionHeaderRepository.GetAsync(input.Id);
            var vehicleInspection = ObjectMapper.Map<VehicleInspectionDto>(entity);
            var gateEntryDetails = await _gateEntryRepository.FirstOrDefaultAsync(a => a.Id == vehicleInspection.GateEntryId);
            vehicleInspection.GatePassNo = gateEntryDetails?.GatePassNo;
            var invoiceDetailEntity = new EntityDto<int>
            {
                Id = entity.InvoiceId ?? 0
            };
            vehicleInspection.InvoiceDto = await _invoiceService.GetAsync(invoiceDetailEntity);
            var isControllerMode = await _modeRepository.FirstOrDefaultAsync(a => a.IsController);

            var vehicleInspectionDetails = await (from detail in _vehicleInspectionDetailRepository.GetAll()
                                                  join checkpoint in _checkpointRepository.GetAll()
                                                  on detail.CheckpointId equals checkpoint.Id
                                                  where detail.VehicleInspectionHeaderId == entity.Id
                                                  select new CheckpointDto
                                                  {
                                                      Id = detail.Id,
                                                      CheckPointId = checkpoint.Id,
                                                      InspectionChecklistId = vehicleInspection.InspectionChecklistId,
                                                      CheckpointName = checkpoint.CheckpointName,
                                                      ValueTag = checkpoint.ValueTag,
                                                      AcceptanceValue = checkpoint.AcceptanceValue,
                                                      CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                      ModeId = checkpoint.ModeId,
                                                      Observation = detail.Observation,
                                                      DiscrepancyRemark = detail.DiscrepancyRemark,
                                                      IsControllerMode = isControllerMode != null && isControllerMode.Id == checkpoint.ModeId
                                                  }).ToListAsync() ?? default;

            if (vehicleInspection.TransactionStatusId == (int)PMMSEnums.TransactionStatus.OnHold && sessionUser.User.IsControllerMode)
            {
                vehicleInspection.VehicleInspectionDetails = vehicleInspectionDetails;
                if (!vehicleInspectionDetails.Any(a => a.IsControllerMode))
                {
                    var qaCheckpoints = await _inwardAppService.GetCheckpointsByChecklistIdAsync(vehicleInspection.InspectionChecklistId, sessionUser.User.ModeId);
                    vehicleInspection.VehicleInspectionDetails.AddRange(qaCheckpoints);
                }
                return vehicleInspection;
            }
            vehicleInspection.VehicleInspectionDetails = vehicleInspectionDetails;
            return vehicleInspection;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.VehicleInspection_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<VehicleInspectionListDto>> GetAllAsync(PagedVehicleInspectionResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<VehicleInspectionListDto>(
                totalCount,
                entities
            );
        }

        public async Task<bool> IsVehicleInspectionPresent(int? gateEntryId, string PO_Number, string invoice_Number, int? invoiceId = null)
        {

            //string ponumber = (from w in _vehicleInspectionHeaderRepository.GetAll()
            //                   from s in _invoiceDetailRepository.GetAll()
            //                   where w.GateEntryId == gateEntryId && w.InvoiceId == invoiceId && w.InvoiceId == s.Id
            //                   select new { s.PurchaseOrderNo }).ToString();

            string ponumber = _invoiceDetailRepository.GetAll().Where(a => a.Id == invoiceId).Select(a => a.PurchaseOrderNo).FirstOrDefault();


            if (gateEntryId != null)
            {
                return await _vehicleInspectionHeaderRepository.GetAll().AnyAsync(x => x.GateEntryId == gateEntryId && x.InvoiceId == invoiceId && ponumber != PO_Number);
            }

            var invoice = await _invoiceDetailRepository.GetAll().FirstOrDefaultAsync(x => x.PurchaseOrderNo == PO_Number && x.InvoiceNo == invoice_Number);
            if (invoice != null)
            {
                return await _vehicleInspectionHeaderRepository.GetAll()
                    .AnyAsync(predicate: x => x.InvoiceId == invoice.Id);
            }
            return false;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.VehicleInspection_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<VehicleInspectionDto> CreateAsync(CreateVehicleInspectionDto input)
        {
            if (await IsVehicleInspectionPresent(input.GateEntryId, input.InvoiceDto.PurchaseOrderNo, input.InvoiceDto.InvoiceNo, input.InvoiceDto.Id) && input.TransactionStatusId != (int)TransactionStatus.Rejected)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.VehicleInspectionAlreadyExist, input.InvoiceDto.PurchaseOrderNo + "-" + input.InvoiceDto.InvoiceNo));
            }
            var vehicleInspectionHeader = ObjectMapper.Map<VehicleInspectionHeader>(input);
            vehicleInspectionHeader.TenantId = AbpSession.TenantId;

            if (input.GateEntryId != null && input.InvoiceDto.Id != 0)
            {
                var invoice = await _invoiceService.GetAsync(input.InvoiceDto);
                await _invoiceService.UpdateAsync(invoice);
                vehicleInspectionHeader.InvoiceId = invoice?.Id;
            }
            else
            {
                var createInvoiceDto = ObjectMapper.Map<CreateInvoiceDto>(input.InvoiceDto);
                var insertedInvoice = await _invoiceService.CreateAsync(createInvoiceDto);
                vehicleInspectionHeader.InvoiceId = insertedInvoice?.Id;
            }

            if (input.VehicleInspectionDetails.Any(a => a.DiscrepancyRemark != null))
            {
                vehicleInspectionHeader.TransactionStatusId = (int)PMMSEnums.TransactionStatus.OnHold;
            }
            else { vehicleInspectionHeader.TransactionStatusId = (int)PMMSEnums.TransactionStatus.Accepted; }
            if (vehicleInspectionHeader.VehicleInspectionDetails.Count > 0)
            {
                vehicleInspectionHeader.VehicleInspectionDetails = input.VehicleInspectionDetails.Select(MapVehicleCheckpointDto).ToList();
            }
            vehicleInspectionHeader.InspectionDate = DateTime.UtcNow;
            await _vehicleInspectionHeaderRepository.InsertAsync(vehicleInspectionHeader);

            CurrentUnitOfWork.SaveChanges();

            return ObjectMapper.Map<VehicleInspectionDto>(vehicleInspectionHeader);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.VehicleInspection_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<VehicleInspectionDto> UpdateAsync(UpdateVehicleInspectionDto input)
        {
            var vehicleInspection = await _vehicleInspectionHeaderRepository.GetAsync(input.Id);
            var details = await _vehicleInspectionDetailRepository.GetAllListAsync(x => x.VehicleInspectionHeaderId == input.Id);
            vehicleInspection.TenantId = AbpSession.TenantId;
            vehicleInspection.TransactionStatusId = input.TransactionStatusId;
            vehicleInspection.VehicleInspectionDetails = details;
            if (input.VehicleInspectionDetails != null)
            {
                foreach (var checkpoint in input.VehicleInspectionDetails.Where(x => x.Id == 0))
                {
                    var checkpointToInsert = ObjectMapper.Map<VehicleInspectionDetail>(checkpoint);
                    checkpointToInsert.VehicleInspectionHeaderId = input.Id;
                    checkpointToInsert.TenantId = AbpSession.TenantId;
                    vehicleInspection.VehicleInspectionDetails.Add(checkpointToInsert);
                }
                await _vehicleInspectionHeaderRepository.UpdateAsync(vehicleInspection);
            }
            return await GetAsync(input);
        }

        private VehicleInspectionDetail MapVehicleCheckpointDto(CheckpointDto input)
        {
            var vehicleInspectionDetail = ObjectMapper.Map<VehicleInspectionDetail>(input);
            vehicleInspectionDetail.TenantId = AbpSession.TenantId;
            vehicleInspectionDetail.CheckpointId = input.CheckPointId;
            //vehicleInspectionDetail.Id = input.VehicleInspectionDetailId.HasValue ? input.VehicleInspectionDetailId.Value : 0;
            return vehicleInspectionDetail;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<VehicleInspectionListDto> ApplySorting(IQueryable<VehicleInspectionListDto> query, PagedVehicleInspectionResultRequestDto input)
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
        protected IQueryable<VehicleInspectionListDto> ApplyPaging(IQueryable<VehicleInspectionListDto> query, PagedVehicleInspectionResultRequestDto input)
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

        protected IQueryable<VehicleInspectionListDto> CreateUserListFilteredQuery(PagedVehicleInspectionResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var VehicleInspectionHeaderQuery = from vehicleInspection in _vehicleInspectionHeaderRepository.GetAll()
                                               join transactionStatus in _transactionStatusRepository.GetAll()
                                               on vehicleInspection.TransactionStatusId equals transactionStatus.Id
                                               join gateEntry in _gateEntryRepository.GetAll()
                                               on vehicleInspection.GateEntryId equals gateEntry.Id into insgate
                                               from gateEntry in insgate.DefaultIfEmpty()
                                               join invoice in _invoiceDetailRepository.GetAll()
                                               on vehicleInspection.InvoiceId equals invoice.Id
                                               join purchaseOrder in _purchaseOrderRepository.GetAll()
                                               on invoice.PurchaseOrderId equals purchaseOrder.Id
                                               select new VehicleInspectionListDto
                                               {
                                                   GatePassNo = gateEntry.GatePassNo,
                                                   Id = vehicleInspection.Id,
                                                   PurchaseOrderId = invoice.PurchaseOrderId,
                                                   GateEntryId = vehicleInspection.GateEntryId,
                                                   InvoiceNo = invoice.InvoiceNo,
                                                   LRNo = invoice.LRNo,
                                                   PurchaseOrderNo = invoice.PurchaseOrderNo,
                                                   SubPlantId = purchaseOrder.PlantId,
                                                   TransactionStatusId = vehicleInspection.TransactionStatusId,
                                                   TransactionStatus = transactionStatus.TransactionStatus,
                                                   PurchaseOrderDeliverSchedule = invoice.purchaseOrderDeliverSchedule
                                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                VehicleInspectionHeaderQuery = VehicleInspectionHeaderQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (input.TransactionStatusId != null)
            {
                VehicleInspectionHeaderQuery = VehicleInspectionHeaderQuery.Where(x => x.TransactionStatusId == input.TransactionStatusId);
            }
            if (input.PurchaseOrderId != null)
            {
                VehicleInspectionHeaderQuery = VehicleInspectionHeaderQuery.Where(x => x.PurchaseOrderId == input.PurchaseOrderId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                VehicleInspectionHeaderQuery = VehicleInspectionHeaderQuery.Where(x => x.GatePassNo.Contains(input.Keyword));
            }
            return VehicleInspectionHeaderQuery;
        }
    }
}