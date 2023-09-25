using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonService.Invoices;
using ELog.Application.CommonService.Inward;
using ELog.Application.Inward.MaterialInspections.Dto;
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

namespace ELog.Application.Inward.MaterialInspections
{
    [PMMSAuthorize]
    public class MaterialInspectionAppService : ApplicationService, IMaterialInspectionAppService
    {
        private readonly IRepository<Material> _materialRepository;
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

        public MaterialInspectionAppService(IRepository<Material> materialRepository,
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
            _materialRepository = materialRepository;
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

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<MaterialInspectionDto> GetAsync(EntityDto<int> input)
        {
            var headerEntity = await _materialHeaderRepository.GetAsync(input.Id);
            var materialInspectionDto = new MaterialInspectionDto
            {
                Id = headerEntity.Id,
                MaterialInspectionTransactionId = headerEntity.TransactionStatusId,
                MaterialTransactionId = null
            };
            var gateEntryDetails = await _gateEntryRepository.FirstOrDefaultAsync(a => a.Id == headerEntity.GateEntryId);
            materialInspectionDto.GatePassNo = gateEntryDetails?.GatePassNo;
            materialInspectionDto.GateEntryId = gateEntryDetails?.Id;
            var invoiceDetailEntity = new EntityDto<int>
            {
                Id = headerEntity.InvoiceId ?? 0
            };
            materialInspectionDto.InvoiceDetails = await _invoiceService.GetAsync(invoiceDetailEntity);
            return materialInspectionDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<MaterialInspectionDto> GetWithMaterialAsync(MaterialEntityDto input)
        {
            var sessionUser = await _sessionAppService.GetCurrentLoginInformations();
            var materialInspectionHeader = await _materialHeaderRepository.GetAsync(input.Id);
            var isControllerMode = await _modeRepository.FirstOrDefaultAsync(a => a.ModeName == PMMSConsts.Quality && a.IsController);
            var materialInspectionDto = new MaterialInspectionDto
            {
                MaterialId = input.MaterialId,
                Id = input.Id,
                MaterialInspectionTransactionId = materialInspectionHeader.TransactionStatusId
            };

            var materialRelation = _materialInspectionRelationDetailRepository.GetAllIncluding(x => x.MaterialConsignments, x => x.MaterialDamageDetails, x => x.MaterialCheckpoints)
                 .Where(x => x.MaterialHeaderId == input.Id && x.MaterialId == input.MaterialId)
                 .FirstOrDefault();
            if (materialRelation == null)
            {
                return materialInspectionDto;
            }

            ObjectMapper.Map(materialRelation, materialInspectionDto);
            materialInspectionDto.MaterialTransactionId = materialRelation.TransactionStatusId;
            materialInspectionDto.MaterialCheckpoints = await (from detail in _materialChecklistDetailRepository.GetAll()
                                                               join checkpoint in _checkpointRepository.GetAll()
                                                               on detail.CheckPointId equals checkpoint.Id
                                                               where detail.MaterialRelationId == materialRelation.Id
                                                               select new CheckpointDto
                                                               {
                                                                   Id = detail.Id,
                                                                   CheckPointId = checkpoint.Id,
                                                                   InspectionChecklistId = materialRelation.InspectionChecklistId,
                                                                   CheckpointName = checkpoint.CheckpointName,
                                                                   ValueTag = checkpoint.ValueTag,
                                                                   AcceptanceValue = checkpoint.AcceptanceValue,
                                                                   CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                                   ModeId = checkpoint.ModeId,
                                                                   Observation = detail.Observation,
                                                                   DiscrepancyRemark = detail.DiscrepancyRemark,
                                                                   IsControllerMode = isControllerMode != null && isControllerMode.Id == checkpoint.ModeId
                                                               }).ToListAsync() ?? default;
            if (materialInspectionDto.MaterialTransactionId == (int)PMMSEnums.TransactionStatus.OnHold &&
                sessionUser.User.IsControllerMode &&
                !materialInspectionDto.MaterialCheckpoints.Any(a => a.IsControllerMode))
            {
                var qaCheckpoints = await _inwardAppService.GetCheckpointsByChecklistIdAsync(materialInspectionDto.InspectionChecklistId.GetValueOrDefault(), sessionUser.User.ModeId);
                qaCheckpoints.ForEach(x => x.IsControllerMode = isControllerMode.Id == x.ModeId);
                materialInspectionDto.MaterialCheckpoints.AddRange(qaCheckpoints);
            }
            return materialInspectionDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<MaterialInspectionListDto>> GetAllAsync(PagedMaterialInspectionResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MaterialInspectionListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<int> CreateAsync(CreateMaterialInspectionDto input)
        {
            if (await IsMaterialInspectionPresent(input.GateEntryId, input.InvoiceDetails.PurchaseOrderNo, input.InvoiceDetails.InvoiceNo, input.InvoiceDetails.Id) && input.TransactionStatusId != (int)TransactionStatus.Rejected)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.MaterialInspectionAlreadyExist, input.InvoiceDetails.PurchaseOrderNo + "-" + input.InvoiceDetails.InvoiceNo));
            }
            var invoiceId = input.InvoiceDetails.Id;
            //Insert Invoice Detail
            if (input.InvoiceDetails.Id == 0)
            {
                var invoice = await _invoiceDetailRepository.GetAll().FirstOrDefaultAsync(x =>
                                                                  x.PurchaseOrderNo.Trim().ToLower() == input.InvoiceDetails.PurchaseOrderNo.Trim().ToLower()
                                                                  && x.InvoiceNo.Trim().ToLower() == input.InvoiceDetails.InvoiceNo.Trim().ToLower());
                if (invoice != null)
                {
                    var vehicleInspectionDoneInvoiceId = await _vehicleInspectionHeaderRepository.GetAll().Select(x => x.InvoiceId).FirstOrDefaultAsync(x => x == invoice.Id);
                    if (vehicleInspectionDoneInvoiceId != null)
                    {
                        var vehicleInvoiceDetail = await _invoiceDetailRepository.GetAsync(vehicleInspectionDoneInvoiceId.GetValueOrDefault());
                        vehicleInvoiceDetail.InvoiceNo = input.InvoiceDetails.InvoiceNo;
                        vehicleInvoiceDetail.InvoiceDate = input.InvoiceDetails.InvoiceDate;
                        vehicleInvoiceDetail.DriverName = input.InvoiceDetails.DriverName;
                        vehicleInvoiceDetail.PurchaseOrderNo = input.InvoiceDetails.PurchaseOrderNo;
                        vehicleInvoiceDetail.LRNo = input.InvoiceDetails.LRNo;
                        vehicleInvoiceDetail.LRDate = input.InvoiceDetails.LRDate;
                        vehicleInvoiceDetail.VehicleNumber = input.InvoiceDetails.VehicleNumber;
                        vehicleInvoiceDetail.VendorName = input.InvoiceDetails.VendorName;
                        vehicleInvoiceDetail.VendorCode = input.InvoiceDetails.VendorCode;
                        vehicleInvoiceDetail.purchaseOrderDeliverSchedule = vehicleInvoiceDetail.purchaseOrderDeliverSchedule != null ? vehicleInvoiceDetail.purchaseOrderDeliverSchedule : input.InvoiceDetails.purchaseOrderDeliverSchedule;
                        await _invoiceDetailRepository.UpdateAsync(vehicleInvoiceDetail);
                        invoiceId = vehicleInspectionDoneInvoiceId.GetValueOrDefault();
                    }
                }
                else
                {
                    var invoiceDetail = ObjectMapper.Map<InvoiceDetail>(input.InvoiceDetails);
                    await _invoiceDetailRepository.InsertAsync(invoiceDetail);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    invoiceId = invoiceDetail.Id;
                }
            }
            else
            {
                var invoiceEntity = await _invoiceDetailRepository.GetAsync(input.InvoiceDetails.Id);
                invoiceEntity.purchaseOrderDeliverSchedule = input.InvoiceDetails.purchaseOrderDeliverSchedule;
                await _invoiceDetailRepository.UpdateAsync(invoiceEntity);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            //Insert Material Inspection Header
            var materialInspectionHeader = ObjectMapper.Map<MaterialInspectionHeader>(input);
            materialInspectionHeader.InvoiceId = invoiceId;
            materialInspectionHeader.TransactionStatusId = (int)TransactionStatus.Saved;
            await _materialHeaderRepository.InsertAsync(materialInspectionHeader);
            await CurrentUnitOfWork.SaveChangesAsync();
            //Insert Material Inspection Relational,Consignment,Checklist,Damage Details Table
            var materialInspectionRelation = ObjectMapper.Map<MaterialInspectionRelationDetail>(input);
            materialInspectionRelation.MaterialHeaderId = materialInspectionHeader.Id;
            if (input.MaterialCheckpoints.Any(a => a.DiscrepancyRemark != null))
            {
                materialInspectionRelation.TransactionStatusId = (int)TransactionStatus.OnHold;
            }
            else
            {
                materialInspectionRelation.TransactionStatusId = (int)TransactionStatus.Accepted;
            }
            var insertedMaterialInspectionRelation = await _materialInspectionRelationDetailRepository.InsertAsync(materialInspectionRelation);
            await CurrentUnitOfWork.SaveChangesAsync();
            foreach (var damageDetails in input.MaterialDamageDetails)
            {
                var damageDetailToInsert = ObjectMapper.Map<MaterialDamageDetail>(damageDetails);
                damageDetailToInsert.MaterialConsignmentId = insertedMaterialInspectionRelation.MaterialConsignments.FirstOrDefault(x => x.SequenceId == damageDetails.SequenceId).Id;
                damageDetailToInsert.MaterialRelationId = insertedMaterialInspectionRelation.Id;
                await _materialDamageDetailRepository.InsertAsync(damageDetailToInsert);
            }
            return materialInspectionHeader.Id;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<MaterialInspectionDto> UpdateAsync(MaterialInspectionDto input)
        {
            int savedTransactionStatus = (int)TransactionStatus.Saved;
            if (_materialHeaderRepository.GetAll().Any(x => x.Id == input.Id && x.TransactionStatusId != savedTransactionStatus))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.MaterialInspectionCompleted);
            }
            var invoiceEntity = await _invoiceDetailRepository.GetAsync(input.InvoiceDetails.Id);
            var purchaseOrderId = invoiceEntity.PurchaseOrderId;
            var invoiceNo = invoiceEntity.InvoiceNo;
            ObjectMapper.Map(input.InvoiceDetails, invoiceEntity);
            invoiceEntity.PurchaseOrderId = purchaseOrderId;
            invoiceEntity.InvoiceNo = invoiceNo;
            await _invoiceDetailRepository.UpdateAsync(invoiceEntity);

            if (input.MaterialRelationId == null || input.MaterialRelationId == 0)
            {
                var materialRelation = ObjectMapper.Map<MaterialInspectionRelationDetail>(input);
                if (input.MaterialCheckpoints.Any(a => a.DiscrepancyRemark != null))
                {
                    materialRelation.TransactionStatusId = (int)TransactionStatus.OnHold;
                }
                else
                {
                    materialRelation.TransactionStatusId = (int)TransactionStatus.Accepted;
                }
                var insertedMaterialInspectionRelation = await _materialInspectionRelationDetailRepository.InsertAsync(materialRelation);
                await CurrentUnitOfWork.SaveChangesAsync();
                foreach (var damageDetails in input.MaterialDamageDetails)
                {
                    var damageDetailToInsert = ObjectMapper.Map<MaterialDamageDetail>(damageDetails);
                    damageDetailToInsert.MaterialConsignmentId = insertedMaterialInspectionRelation.MaterialConsignments.FirstOrDefault(x => x.SequenceId == damageDetails.SequenceId).Id;
                    damageDetailToInsert.MaterialRelationId = insertedMaterialInspectionRelation.Id;
                    await _materialDamageDetailRepository.InsertAsync(damageDetailToInsert);
                }
            }

            MaterialEntityDto entityDto = new MaterialEntityDto { MaterialId = input.MaterialId.GetValueOrDefault(), Id = input.Id };
            return await GetWithMaterialAsync(entityDto);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<bool> CompleteInspectionAsync(CompleteInspectionEntityDto input)
        {
            var relationMaterialId = _materialInspectionRelationDetailRepository
                                        .GetAll().Where(x => x.MaterialHeaderId == input.Id).Select(x => x.MaterialId);
            var purchaseOrderMaterialId = _materialRepository
                                        .GetAll().Where(x => x.PurchaseOrderId == input.PurchaseOrderId).Select(x => x.Id);

            if (await purchaseOrderMaterialId.Except(relationMaterialId).AnyAsync())
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.CannotCompleteMaterialInspection);
            }
            var materialHeader = await _materialHeaderRepository.GetAsync(input.Id);
            var onHoldStatusId = (int)TransactionStatus.OnHold;
            if (await _materialInspectionRelationDetailRepository.GetAll().AnyAsync(x => x.MaterialHeaderId == input.Id && x.TransactionStatusId == onHoldStatusId))
            {
                materialHeader.TransactionStatusId = onHoldStatusId;
            }
            else
            {
                materialHeader.TransactionStatusId = (int)TransactionStatus.Accepted;
            }
            await _materialHeaderRepository.UpdateAsync(materialHeader);

            return true;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspection_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<bool> AcceptOrRejectInspectionAsync(AcceptRejectInspectionCheckpointDto input)
        {
            foreach (var checkpoint in input.QualityCheckpoints)
            {
                var qaCheckpoint = ObjectMapper.Map<MaterialChecklistDetail>(checkpoint);
                qaCheckpoint.MaterialRelationId = input.MaterialRelationId;
                await _materialChecklistDetailRepository.InsertAsync(qaCheckpoint);
            }
            var materialRelation = await _materialInspectionRelationDetailRepository.GetAsync(input.MaterialRelationId);
            materialRelation.TransactionStatusId = input.TransactionStatusId;
            await _materialInspectionRelationDetailRepository.UpdateAsync(materialRelation);

            await CurrentUnitOfWork.SaveChangesAsync();

            int rejectedTransactionId = (int)TransactionStatus.Rejected;
            int acceptedTransactionId = (int)TransactionStatus.Accepted;
            var materialInspectionHeader = await _materialHeaderRepository.GetAsync(input.Id);
            var existingTransactionStatuses = await _materialInspectionRelationDetailRepository.GetAll()
                .Where(x => x.MaterialHeaderId == input.Id).Select(x => x.TransactionStatusId)?.ToListAsync() ?? default;

            if (existingTransactionStatuses.Any(x => x == rejectedTransactionId))
            {
                materialInspectionHeader.TransactionStatusId = rejectedTransactionId;
                await _materialHeaderRepository.UpdateAsync(materialInspectionHeader);
            }
            else if (existingTransactionStatuses.All(x => x == acceptedTransactionId))
            {
                materialInspectionHeader.TransactionStatusId = acceptedTransactionId;
                await _materialHeaderRepository.UpdateAsync(materialInspectionHeader);
            }

            return true;
        }

        public async Task<bool> IsMaterialInspectionPresent(int? GateEntryId, string PO_Number, string invoice_Number, int? invoiceId = null)
        {
            if (GateEntryId != null)
            {
                return await _materialHeaderRepository.GetAll().AnyAsync(x => x.GateEntryId == GateEntryId && x.InvoiceId == invoiceId);
            }
            if (!string.IsNullOrEmpty(PO_Number) && !string.IsNullOrEmpty(invoice_Number))
            {
                var invoice = await _invoiceDetailRepository.GetAll().FirstOrDefaultAsync(x =>
                                                                                 x.PurchaseOrderNo.Trim().ToLower() == PO_Number.Trim().ToLower()
                                                                                 && x.InvoiceNo.Trim().ToLower() == invoice_Number.Trim().ToLower());
                if (invoice != null)
                {
                    if (await _materialHeaderRepository.GetAll().AnyAsync(x => x.InvoiceId == invoice.Id))
                    {
                        return true;
                    }
                    else if (await _vehicleInspectionHeaderRepository.GetAll().AnyAsync(x => x.InvoiceId == invoice.Id))
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public async Task<bool> IsVehicleInspectionIsInProgress(int? GateEntryId, string PO_Number, string invoice_Number, int? invoiceId = null)
        {
            if (GateEntryId != null)
            {
                return await _vehicleInspectionHeaderRepository.GetAll().AnyAsync(x => x.GateEntryId == GateEntryId && x.InvoiceId == invoiceId && x.TransactionStatusId == (int)TransactionStatus.OnHold);
            }
            if (!string.IsNullOrEmpty(PO_Number) && !string.IsNullOrEmpty(invoice_Number))
            {
                var invoice = await _invoiceDetailRepository.GetAll().FirstOrDefaultAsync(x => x.PurchaseOrderNo == PO_Number && x.InvoiceNo == invoice_Number);
                if (invoice != null)
                {
                    return await _vehicleInspectionHeaderRepository.GetAll()
                        .AnyAsync(predicate: x => x.InvoiceId == invoice.Id && x.TransactionStatusId == (int)TransactionStatus.OnHold);
                }
            }
            return false;
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<MaterialInspectionListDto> ApplySorting(IQueryable<MaterialInspectionListDto> query, PagedMaterialInspectionResultRequestDto input)
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
        protected IQueryable<MaterialInspectionListDto> ApplyPaging(IQueryable<MaterialInspectionListDto> query, PagedMaterialInspectionResultRequestDto input)
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

        protected IQueryable<MaterialInspectionListDto> CreateUserListFilteredQuery(PagedMaterialInspectionResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var MaterialInspectionHeaderQuery = from materialInspection in _materialHeaderRepository.GetAll()
                                                join gateEntry in _gateEntryRepository.GetAll()
                                                on materialInspection.GateEntryId equals gateEntry.Id into insgate
                                                from gateEntry in insgate.DefaultIfEmpty()
                                                join invoice in _invoiceDetailRepository.GetAll()
                                                on materialInspection.InvoiceId equals invoice.Id
                                                join purchaseOrder in _purchaseOrderRepository.GetAll()
                                                on invoice.PurchaseOrderId equals purchaseOrder.Id
                                                join transaction in _transactionStatusRepository.GetAll()
                                                on materialInspection.TransactionStatusId equals transaction.Id
                                                select new MaterialInspectionListDto
                                                {
                                                    GatePassNo = gateEntry.GatePassNo,
                                                    Id = materialInspection.Id,
                                                    PurchaseOrderId = invoice.PurchaseOrderId,
                                                    GateEntryId = materialInspection.GateEntryId,
                                                    InvoiceNo = invoice.InvoiceNo,
                                                    LRNo = invoice.LRNo,
                                                    PurchaseOrderNo = invoice.PurchaseOrderNo,
                                                    SubPlantId = purchaseOrder.PlantId,
                                                    TransactionStatusId = materialInspection.TransactionStatusId,
                                                    UserEnteredTransaction = transaction.TransactionStatus,
                                                    PurchaseOrderDeliverSchedule = invoice.purchaseOrderDeliverSchedule
                                                };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                MaterialInspectionHeaderQuery = MaterialInspectionHeaderQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (input.TransactionStatusId != null)
            {
                MaterialInspectionHeaderQuery = MaterialInspectionHeaderQuery.Where(x => x.TransactionStatusId == input.TransactionStatusId);
            }
            if (input.PurchaseOrderId != null)
            {
                MaterialInspectionHeaderQuery = MaterialInspectionHeaderQuery.Where(x => x.PurchaseOrderId == input.PurchaseOrderId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                MaterialInspectionHeaderQuery = MaterialInspectionHeaderQuery.Where(x => x.GatePassNo.Contains(input.Keyword));
            }
            return MaterialInspectionHeaderQuery;
        }
    }
}