//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.CommonService.Invoices;
//using ELog.Application.CommonService.Inward;
//using ELog.Application.Inward.GRNPostings.Dto;
//using ELog.Application.Inward.GRNs;
//using ELog.Application.Inward.VehicleInspections.Dto;
//using ELog.Application.SAP.ProcessOrder.Dto;
//using ELog.Application.SelectLists.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Authorization.Users;
//using ELog.Core.Entities;
//using ELog.Core.Printer;
//using ELog.Core.SAP;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using ELog.HardwareConnectorFactory;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Net;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Inward.VehicleInspections
//{
//    [PMMSAuthorize]
//    public class GRNPostingAppService : ApplicationService, IGRNPostingAppService
//    {
//        private const string formatNumber = "FDSTG/F/013,Version:01";
//        private const string sopNo = "FDSOP/GHT/ST/018";

//        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
//        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
//        private readonly IRepository<GRNHeader> _grnHeaderRepository;
//        private readonly IRepository<GRNDetail> _grnDetailRepository;
//        private readonly IRepository<GRNQtyDetail> _grnQtyDetailRepository;
//        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
//        private readonly IRepository<SAPGRNPosting> _sapGRNPostingRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<Material> _materialRepository;
//        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentRepository;
//        private readonly IRepository<GRNMaterialLabelPrintingHeader> _grnMaterialLabelPrintingHeaderRepository;
//        private readonly IRepository<GRNMaterialLabelPrintingDetail> _grnMaterialLabelPrintingDetailRepository;
//        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly PrinterFactory _printerFactory;
//        private readonly IRepository<DeviceMaster> _deviceRepository;



//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IWebHostEnvironment _environment;
//        private readonly IConfiguration _configuration;
//        private readonly IRepository<MaterialMaster> _materialMasterRepository;

//        private readonly ELog.ERPConnectorFactory.ERPConnectorFactory _eRPConnectorFactory;
//        private readonly IRepository<User, long> _userRepository;
//        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
//        private readonly IRepository<Core.Entities.ProcessOrder> _processOrderRepository;
//        private readonly IRepository<InspectionLot> _inspectionLotRepository;
//        private readonly IRepository<MaterialInspectionHeader> _materialInspectionHeaderRepository;
//        private readonly IRepository<MaterialInspectionRelationDetail> _materialInspectionRelationDetailRepository;

//        public GRNPostingAppService(IRepository<PurchaseOrder> purchaseOrderRepository,
//          IRepository<InvoiceDetail> invoiceDetailRepository,
//          IHttpContextAccessor httpContextAccessor, IRepository<MaterialMaster> materialMasterRepository,
//          IRepository<GRNHeader> grnHeaderRepository,
//          IRepository<GRNDetail> grnDetailRepository,
//          IInvoiceService invoiceService,
//          IRepository<GRNQtyDetail> grnQtyDetailRepository,
//          InwardAppService inwardAppService,
//          IMasterCommonRepository masterCommonRepository, IRepository<Material> materialRepository, IRepository<MaterialInspectionHeader> materialInspectionHeaderRepository,
//          IRepository<MaterialConsignmentDetail> materialConsignmentRepository, IRepository<InspectionLot> inspectionLotRepository,
//          IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
//          IRepository<GRNMaterialLabelPrintingHeader> grnMaterialLabelPrintingHeaderRepository, IRepository<Core.Entities.ProcessOrder> processOrderRepository,
//        IRepository<GRNMaterialLabelPrintingDetail> grnMaterialLabelPrintingDetailRepository, IRepository<MaterialInspectionRelationDetail> materialInspectionRelationDetailRepository,
//        IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerRepository,
//         IRepository<PlantMaster> plantRepository, IConfiguration configuration, IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
//          IWebHostEnvironment environment, PrinterFactory printerFactory, IRepository<DeviceMaster> deviceRepository, IRepository<User, long> userRepository,
//          ELog.ERPConnectorFactory.ERPConnectorFactory eRPConnectorFactory, IRepository<SAPGRNPosting> sapGRNPostingRepository)
//        {
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _purchaseOrderRepository = purchaseOrderRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _inspectionLotRepository = inspectionLotRepository;
//            _processOrderMaterialRepository = processOrderMaterialRepository;
//            _processOrderRepository = processOrderRepository;

//            _invoiceDetailRepository = invoiceDetailRepository;
//            _grnHeaderRepository = grnHeaderRepository;
//            _grnDetailRepository = grnDetailRepository;
//            _grnQtyDetailRepository = grnQtyDetailRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _materialRepository = materialRepository;
//            _unitOfMeasurementRepository = unitOfMeasurementRepository;
//            _materialConsignmentRepository = materialConsignmentRepository;
//            _grnMaterialLabelPrintingHeaderRepository = grnMaterialLabelPrintingHeaderRepository;
//            _grnMaterialLabelPrintingDetailRepository = grnMaterialLabelPrintingDetailRepository;
//            _grnMaterialLabelPrintingContainerRepository = grnMaterialLabelPrintingContainerRepository;
//            _plantRepository = plantRepository;
//            _environment = environment;
//            _configuration = configuration;
//            _printerFactory = printerFactory;
//            _deviceRepository = deviceRepository;
//            _eRPConnectorFactory = eRPConnectorFactory;
//            _sapGRNPostingRepository = sapGRNPostingRepository;
//            _userRepository = userRepository;
//            _materialMasterRepository = materialMasterRepository;
//            _materialInspectionHeaderRepository = materialInspectionHeaderRepository;
//            _materialInspectionRelationDetailRepository = materialInspectionRelationDetailRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNPosting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GRNPostingDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _grnHeaderRepository.GetAsync(input.Id);
//            var grnPosting = ObjectMapper.Map<GRNPostingDto>(entity);
//            var poDetails = await _invoiceDetailRepository.FirstOrDefaultAsync(a => a.PurchaseOrderId == grnPosting.PurchaseOrderId);
//            grnPosting.PurchaseOrderNo = poDetails?.PurchaseOrderNo;
//            grnPosting.InvoiceNo = poDetails?.InvoiceNo;
//            grnPosting.GRNPostingNumber = entity.GRNNumber;
//            grnPosting.GRNDetails = await (from grnHeader in _grnHeaderRepository.GetAll()
//                                           join grnDetail in _grnDetailRepository.GetAll()
//                                           on grnHeader.Id equals grnDetail.GRNHeaderId //into grndt
//                                           join consignment in _materialConsignmentRepository.GetAll()
//                                           on grnDetail.MfgBatchNoId equals consignment.Id
//                                           join material in _materialRepository.GetAll()
//                                           on grnDetail.MaterialId equals material.Id
//                                           join invoice in _invoiceDetailRepository.GetAll()
//                                           on grnDetail.InvoiceId equals invoice.Id
//                                           join grnqtyDetail in _grnQtyDetailRepository.GetAll()
//                                           on grnDetail.Id equals grnqtyDetail.GRNDetailId into ps
//                                           from grnqtyDetail in ps.DefaultIfEmpty()
//                                           join uom in _unitOfMeasurementRepository.GetAll()
//                                           on consignment.UnitofMeasurementId equals uom.Id into uomps
//                                           from uom in uomps.DefaultIfEmpty()
//                                           where grnDetail.GRNHeaderId == grnPosting.Id
//                                           orderby material.ItemCode
//                                           select new GRNPostingDetailsDto
//                                           {
//                                               Id = grnDetail.Id,
//                                               GRNHeaderId = grnHeader.Id,
//                                               MaterialId = grnDetail.MaterialId,
//                                               SAPBatchNumber = grnDetail.SAPBatchNumber,
//                                               MfgBatchNoId = consignment.Id,
//                                               MaterialCode = material.ItemCode,
//                                               InvoiceNo = invoice.InvoiceNo,
//                                               ItemCode = material.ItemCode,
//                                               ItemDescription = material.ItemDescription,
//                                               ManufacturedBatchNo = consignment.ManufacturedBatchNo,
//                                               ConsignmentQty = consignment.QtyAsPerInvoice.Value,
//                                               ConsignmentQtyUnit = consignment.QtyAsPerInvoice.HasValue ? consignment.QtyAsPerInvoice.Value + uom.UnitOfMeasurement : null,
//                                               TotalQty = grnqtyDetail.TotalQty == null ? grnDetail.TotalQty : grnqtyDetail.TotalQty,
//                                               NoOfContainer = grnqtyDetail.NoOfContainer == null ? grnDetail.NoOfContainer : grnqtyDetail.NoOfContainer,
//                                               QtyPerContainer = grnqtyDetail.QtyPerContainer == null ? grnDetail.QtyPerContainer : grnqtyDetail.QtyPerContainer,
//                                               DiscrepancyRemark = grnqtyDetail.DiscrepancyRemark ?? grnDetail.DiscrepancyRemark,
//                                               InvoiceId = grnDetail.InvoiceId,
//                                               GRNPreparedBy = grnHeader.CreatorUser.FullName,
//                                               InvoiceQty = consignment.QtyAsPerInvoice.Value,
//                                               IsDamaged = grnqtyDetail.IsDamaged,
//                                           }).ToListAsync() ?? default;
//            var formattedGRNDetails = new List<GRNPostingDetailsDto>();
//            foreach (var grnDetail in grnPosting.GRNDetails)
//            {
//                var isExists = formattedGRNDetails.FirstOrDefault(a => a.SAPBatchNumber == grnDetail.SAPBatchNumber);
//                var newGrnDetail = new GRNPostingDetailsDto
//                {
//                    Id = grnDetail.Id,
//                    GRNHeaderId = grnDetail.Id,
//                    MaterialId = grnDetail.MaterialId,
//                    SAPBatchNumber = isExists != null ? null : grnDetail.SAPBatchNumber,
//                    MfgBatchNoId = grnDetail.Id,
//                    MaterialCode = isExists != null ? null : grnDetail.MaterialCode,
//                    ItemDescription = isExists != null ? null : grnDetail.ItemDescription,
//                    InvoiceNo = isExists != null ? null : grnDetail.InvoiceNo,
//                    ManufacturedBatchNo = isExists != null ? null : grnDetail.ManufacturedBatchNo,
//                    ConsignmentQty = grnDetail.ConsignmentQty,
//                    ConsignmentQtyUnit = grnDetail.ConsignmentQtyUnit,
//                    TotalQty = grnDetail.TotalQty,
//                    NoOfContainer = grnDetail.NoOfContainer,
//                    QtyPerContainer = grnDetail.QtyPerContainer,
//                    DiscrepancyRemark = grnDetail.DiscrepancyRemark,
//                    InvoiceId = grnDetail.InvoiceId,
//                    IsDamaged = grnDetail.IsDamaged
//                };
//                formattedGRNDetails.Add(newGrnDetail);
//            }
//            grnPosting.GRNDetails = formattedGRNDetails;
//            return grnPosting;
//        }

//        private GRNDetail MapGRNDetailsDto(GRNPostingDetailsDto input)
//        {
//            return ObjectMapper.Map<GRNDetail>(input);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNPosting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<GRNPostingListDto>> GetAllAsync(PagedGRNPostingResultRequestDto input)
//        {
//            var query = CreateGRNListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<GRNPostingListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNPosting_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<HTTPResponseDto> CreateAsync(CreateGRNPostingDto input)
//        {
//            var responseDto = new HTTPResponseDto();
//            var grnposting = new GRNPostingDto();
//            var parentRowIndexs = input.GRNDetails.Select(a => a.ParentRow).Distinct().ToList();
//            var parentRows = input.GRNDetails.Where(a => parentRowIndexs.Contains(a.FormArrayIndex)).Distinct();
//            _masterCommonRepository.ResetLineItemNumberSequenceValue();
//            foreach (var grnDetail in parentRows)
//            {
//                var childRows = input.GRNDetails.Where(a => a.ParentRow == grnDetail.ParentRow);
//                var lineItemNo = string.Empty;
//                lineItemNo = $"{_masterCommonRepository.GetNextGRNLineItemSequence():D4}";
//                grnDetail.LineItem = lineItemNo;
//            }
//            var postedGrn = await PostGRNPosting(input);
//            if (postedGrn != null)
//            {
//                await InsertUpdateSAPGRNPostingAsync(postedGrn);
//                grnposting = await InsertUpdateInternalGRNPostingAsync(postedGrn, input);
//                await InsertOrUpdateInspectionLot(postedGrn);
//                responseDto.ResultObject = grnposting;
//                return responseDto;
//            }
//            return UpdateErrorResponse(responseDto, PMMSValidationConst.GRNPostingFailed);
//        }
//        public HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, string ValidationError)
//        {
//            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
//            responseDto.Error = ValidationError;
//            return responseDto;
//        }
//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<GRNPostingListDto> ApplySorting(IQueryable<GRNPostingListDto> query, PagedGRNPostingResultRequestDto input)
//        {
//            //Try to sort query if available
//            var sortInput = input as ISortedResultRequest;
//            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
//            {
//                return query.OrderBy(sortInput.Sorting);
//            }

//            //IQueryable.Task requires sorting, so we should sort if Take will be used.
//            if (input is ILimitedResultRequest)
//            {
//                return query.OrderByDescending(e => e.Id);
//            }

//            //No sorting
//            return query;
//        }

//        /// <summary>
//        /// Should apply paging if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<GRNPostingListDto> ApplyPaging(IQueryable<GRNPostingListDto> query, PagedGRNPostingResultRequestDto input)
//        {
//            //Try to use paging if available
//            var pagedInput = input as IPagedResultRequest;
//            if (pagedInput != null)
//            {
//                return query.PageBy(pagedInput);
//            }

//            //Try to limit query result if available
//            var limitedInput = input as ILimitedResultRequest;
//            if (limitedInput != null)
//            {
//                return query.Take(limitedInput.MaxResultCount);
//            }

//            //No paging
//            return query;
//        }

//        protected IQueryable<GRNPostingListDto> CreateGRNListFilteredQuery(PagedGRNPostingResultRequestDto input)
//        {
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var GRNHeaderQuery = from purchaseOrder in _purchaseOrderRepository.GetAll()
//                                 join invoice in _invoiceDetailRepository.GetAll()
//                                 on purchaseOrder.Id equals invoice.PurchaseOrderId
//                                 join grnHeader in _grnHeaderRepository.GetAll()
//                                 on purchaseOrder.Id equals grnHeader.PurchaseOrderId
//                                 join grnDetail in _grnDetailRepository.GetAll()
//                                 on invoice.Id equals grnDetail.InvoiceId
//                                 group new { grnHeader } by new
//                                 {
//                                     grnHeader.Id,
//                                     invoice.PurchaseOrderId,
//                                     GRNPostingNumber = grnHeader.GRNNumber,
//                                     invoice.PurchaseOrderNo,
//                                     SubPlantId = purchaseOrder.PlantId,
//                                 } into gcs
//                                 select new GRNPostingListDto
//                                 {
//                                     Id = gcs.Key.Id,
//                                     PurchaseOrderId = gcs.Key.PurchaseOrderId,
//                                     GRNPostingNumber = gcs.Key.GRNPostingNumber,
//                                     PurchaseOrderNo = gcs.Key.PurchaseOrderNo,
//                                     SubPlantId = gcs.Key.SubPlantId,
//                                 };
//            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            {
//                GRNHeaderQuery = GRNHeaderQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
//            }
//            if (input.PurchaseOrderId != null)
//            {
//                GRNHeaderQuery = GRNHeaderQuery.Where(x => x.PurchaseOrderId == input.PurchaseOrderId);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                GRNHeaderQuery = GRNHeaderQuery.Where(x => x.GRNPostingNumber.Contains(input.Keyword));
//            }
//            return GRNHeaderQuery;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNMaterialLabelPrinting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<List<SelectListDto>> GetAllGRNHeaders(string input)
//        {
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var GRNHeaderQuery = from grnHeader in _grnHeaderRepository.GetAll()
//                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
//                                 on grnHeader.PurchaseOrderId equals purchaseOrder.Id
//                                 select new { purchaseOrder.PlantId, grnHeader.Id, grnHeader.GRNNumber };
//            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            {
//                GRNHeaderQuery = GRNHeaderQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
//            }
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                input = input.Trim();
//                GRNHeaderQuery = GRNHeaderQuery.Where(x => x.GRNNumber.Contains(input));
//                return await GRNHeaderQuery.Select(x => new SelectListDto { Id = x.Id, Value = x.GRNNumber })
//               .ToListAsync() ?? default;
//            }
//            return default;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNMaterialLabelPrinting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GRNMaterialLabelPrintingListDto> GetGRNDetailWithAllMaterialLabelPrinting(int input)
//        {
//            var listDto = new GRNMaterialLabelPrintingListDto();
//            var grnHeader = await _grnHeaderRepository.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();
//            if (grnHeader == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GRNNotFoundByGRNNo);
//            }
//            listDto.Id = grnHeader.Id;
//            listDto.GRNNo = grnHeader.GRNNumber;

//            listDto.GRNMaterialLabelPrintings = await (from grnDetail in _grnDetailRepository.GetAllIncluding(x => x.GRNQtyDetails)
//                                                       join material in _materialRepository.GetAll()
//                                                       on grnDetail.MaterialId equals material.Id
//                                                       join materialconsignment in _materialConsignmentRepository.GetAll()
//                                                       on grnDetail.MfgBatchNoId equals materialconsignment.Id
//                                                       join grnMateriallabelHeader in _grnMaterialLabelPrintingHeaderRepository.GetAll()
//                                                       on grnDetail.Id equals grnMateriallabelHeader.GRNDetailId into grm
//                                                       from grnMateriallabelHeader in grm.DefaultIfEmpty()
//                                                       where grnDetail.GRNHeaderId == grnHeader.Id
//                                                       select new GRNMaterialLabelPrintingDto
//                                                       {
//                                                           Id = grnDetail.Id,
//                                                           GRNHeaderId = grnHeader.Id,
//                                                           SAPBatchNo = grnDetail.SAPBatchNumber,
//                                                           PackDetails = grnMateriallabelHeader.PackDetails,
//                                                           MaterialLabelPrintHeaderId = grnMateriallabelHeader.Id,
//                                                           IsAlreadyPrinted = grnMateriallabelHeader != null,
//                                                           ExpiryDate = Convert.ToDateTime(materialconsignment.ExpiryDate).Date,
//                                                           RetestDate = materialconsignment.RetestDate,
//                                                           ManufacturingBatchNo = materialconsignment.ManufacturedBatchNo,

//                                                           ManufacturingDate = Convert.ToDateTime(materialconsignment.ManufacturedDate).Date,


//                                                           MaterialCode = material.ItemCode,
//                                                           MaterialDescription = material.ItemDescription,
//                                                           NumberOfContainers = grnDetail.GRNQtyDetails.Sum(x => x.NoOfContainer),
//                                                           TotalQty = grnDetail.GRNQtyDetails.Sum(x => x.TotalQty),
//                                                           lstMaterialLabelGRNQuantity = grnDetail.GRNQtyDetails.Select(x => new MaterialLabelGRNQuantityDto
//                                                           {
//                                                               GRNQuantityId = x.Id,
//                                                               NumberOfContainers = x.NoOfContainer,
//                                                               QtyPerContainer = x.QtyPerContainer,
//                                                               TotalQty = x.TotalQty
//                                                           })
//                                                       }).ToListAsync() ?? default;

//            return listDto;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNMaterialLabelPrinting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GRNMaterialLabelPrintingListDto> GetGRNDetailWithAllMaterialLabelPrinting_material(int input)
//        {
//            var listDto = new GRNMaterialLabelPrintingListDto();
//            var grnHeader = await _grnHeaderRepository.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();
//            if (grnHeader == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GRNNotFoundByGRNNo);
//            }
//            listDto.Id = grnHeader.Id;
//            listDto.GRNNo = grnHeader.GRNNumber;

//            listDto.GRNMaterialLabelPrintings = await (from grnDetail in _grnDetailRepository.GetAllIncluding(x => x.GRNQtyDetails)
//                                                       join material in _materialRepository.GetAll()
//                                                       on grnDetail.MaterialId equals material.Id
//                                                       join materialconsignment in _materialConsignmentRepository.GetAll()
//                                                       on grnDetail.MfgBatchNoId equals materialconsignment.Id
//                                                       join grnMateriallabelHeader in _grnMaterialLabelPrintingHeaderRepository.GetAll()
//                                                       on grnDetail.Id equals grnMateriallabelHeader.GRNDetailId into grm
//                                                       from grnMateriallabelHeader in grm.DefaultIfEmpty()
//                                                       where grnDetail.GRNHeaderId == grnHeader.Id
//                                                       select new GRNMaterialLabelPrintingDto
//                                                       {
//                                                           Id = grnDetail.Id,
//                                                           GRNHeaderId = grnHeader.Id,
//                                                           SAPBatchNo = grnDetail.SAPBatchNumber,
//                                                           PackDetails = grnMateriallabelHeader.PackDetails,
//                                                           MaterialLabelPrintHeaderId = grnMateriallabelHeader.Id,
//                                                           IsAlreadyPrinted = grnMateriallabelHeader != null,
//                                                           ExpiryDate = Convert.ToDateTime(materialconsignment.ExpiryDate).Date,
//                                                           RetestDate = materialconsignment.RetestDate,
//                                                           ManufacturingBatchNo = materialconsignment.ManufacturedBatchNo,
//                                                           // ManufacturingDate = Convert.ToDateTime(materialconsignment.ManufacturedDate).Date,
//                                                           ManufacturingDate = materialconsignment.ManufacturedDate,

//                                                           MaterialCode = material.ItemCode,
//                                                           MaterialDescription = material.ItemDescription,
//                                                           NumberOfContainers = grnDetail.GRNQtyDetails.Sum(x => x.NoOfContainer),
//                                                           TotalQty = grnDetail.GRNQtyDetails.Sum(x => x.TotalQty),
//                                                           lstMaterialLabelGRNQuantity = grnDetail.GRNQtyDetails.Select(x => new MaterialLabelGRNQuantityDto
//                                                           {
//                                                               GRNQuantityId = x.Id,
//                                                               NumberOfContainers = x.NoOfContainer,
//                                                               QtyPerContainer = x.QtyPerContainer,
//                                                               TotalQty = x.TotalQty
//                                                           })
//                                                       }).ToListAsync() ?? default;

//            return listDto;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNMaterialLabelPrinting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GRNMaterialLabelPrintingDto> PrintMaterialLabels(GRNMaterialLabelPrintingDto input)
//        {
//            if (_grnMaterialLabelPrintingHeaderRepository.GetAll().Any(x => x.GRNDetailId == input.Id))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.LabelsAlreadyPrinted);
//            }
//            var grnMaterialLabelPrintingHeader = new GRNMaterialLabelPrintingHeader
//            {
//                GRNDetailId = input.Id,
//                PackDetails = input.PackDetails
//            };

//            var grnMaterialLabelPrintingDetail = new GRNMaterialLabelPrintingDetail
//            {
//                IsController = false,
//                RangePrint = input.NumberOfContainers.ToString(),
//                PrinterId = input.PrinterId
//            };
//            grnMaterialLabelPrintingDetail.RangePrint = input.RangePrint;
//            grnMaterialLabelPrintingHeader.GRNMaterialLabelPrintingDetails = new List<GRNMaterialLabelPrintingDetail>
//            {
//                grnMaterialLabelPrintingDetail
//            };

//            input.MaterialLabelPrintHeaderId = await _grnMaterialLabelPrintingHeaderRepository.InsertAndGetIdAsync(grnMaterialLabelPrintingHeader);
//            input.IsAlreadyPrinted = true;
//            //Print Containers Pages
//            await PrintPage(input, null);
//            return input;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GRNMaterialLabelPrinting_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GRNMaterialLabelPrintingDto> PrintMaterialLabelRange(GRNMaterialLabelPrintingDto input)
//        {
//            var pageRangeArray = GetRangeArray(input.RangePrint, input.NumberOfContainers, out bool isValid);
//            if (!isValid)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PageRangeNotValid);
//            }
//            var grnMaterialLabelPrintingDetail = new GRNMaterialLabelPrintingDetail
//            {
//                GRNMaterialLabelPrintingHeaderId = input.MaterialLabelPrintHeaderId,
//                RangePrint = input.RangePrint,
//                Comment = input.Comment,
//                IsController = true,
//                PrinterId = input.PrinterId
//            };
//            await _grnMaterialLabelPrintingDetailRepository.InsertAndGetIdAsync(grnMaterialLabelPrintingDetail);

//            //Print Containers Pages         
//            await PrintPage(input, pageRangeArray);
//            return input;
//        }

//        private async Task<bool> PrintPage(GRNMaterialLabelPrintingDto input, List<int> pageRange)
//        {
//            List<int> pagesToPrint;
//            var printLabelExtraDetails = await (from grnheader in _grnHeaderRepository.GetAll()

//                                                join grnDetail in _grnDetailRepository.GetAll()
//                                                 on grnheader.Id equals grnDetail.GRNHeaderId

//                                                join invoice in _invoiceDetailRepository.GetAll()
//                                                on grnDetail.InvoiceId equals invoice.Id

//                                                join purchaseOrder in _purchaseOrderRepository.GetAll()
//                                                on invoice.PurchaseOrderId equals purchaseOrder.Id

//                                                join plant in _plantRepository.GetAll()
//                                                on purchaseOrder.PlantId equals plant.Id

//                                                join consDetails in _materialConsignmentRepository.GetAll()
//                                                on grnDetail.MfgBatchNoId equals consDetails.Id

//                                                join uom in _unitOfMeasurementRepository.GetAll()
//                                               on consDetails.UnitofMeasurementId equals uom.Id

//                                                join material in _materialRepository.GetAll()
//                                                on grnDetail.MaterialId equals material.Id
//                                                where grnheader.Id == input.GRNHeaderId && consDetails.ManufacturedBatchNo.ToLower() == input.ManufacturingBatchNo.ToLower()
//                                                && grnDetail.SAPBatchNumber.ToLower() == input.SAPBatchNo.ToLower()

//                                                select new GRNLabelPrintingVendorDetail
//                                                {
//                                                    PlantId = plant.PlantId,
//                                                    PlantName = plant.PlantName,
//                                                    VendorName = purchaseOrder.VendorName,
//                                                    VendorCode = purchaseOrder.VendorCode,
//                                                    ManufacturerCode = material.ManufacturerCode,
//                                                    ManufacturerName = material.ManufacturerName,
//                                                    InvoiceNo = invoice.InvoiceNo,
//                                                    InvoiceDate = invoice.InvoiceDate,
//                                                    GrnNumber = grnheader.GRNNumber,
//                                                    GrnDate = grnheader.GRNPostingDate,
//                                                    // InspectionLotNo = sap.InspectionLotNo,
//                                                    MfgBatchNo = consDetails.ManufacturedBatchNo,
//                                                    CreatedBy = grnheader.CreatorUser.Name,
//                                                    CreatedOn = grnheader.CreationTime,
//                                                    UOM = uom.UnitOfMeasurement,
//                                                    ManufacturingDate = consDetails.ManufacturedDate.HasValue ? consDetails.ManufacturedDate.Value.ToLocalTime() : consDetails.ManufacturedDate,
//                                                    ExpiryDate = consDetails.ExpiryDate.HasValue ? consDetails.ExpiryDate.Value.ToLocalTime() : consDetails.ExpiryDate,
//                                                    RetestDate = consDetails.RetestDate.HasValue ? consDetails.RetestDate.Value.ToLocalTime() : consDetails.RetestDate

//                                                }).FirstOrDefaultAsync();

//            if (pageRange != null && pageRange.Count > 0)
//            {
//                pagesToPrint = pageRange;
//                await PrintMaterialLabelRanges(input, pagesToPrint, printLabelExtraDetails);
//            }
//            else
//            {
//                var enumerator = Enumerable.Range(1, Convert.ToInt32(input.NumberOfContainers)).GetEnumerator();
//                foreach (var grnQtyDetail in input.lstMaterialLabelGRNQuantity)
//                {
//                    pagesToPrint = GetNextBatch(enumerator, Convert.ToInt32(grnQtyDetail.NumberOfContainers));
//                    await PrintMaterialLabels(input, pagesToPrint, printLabelExtraDetails, grnQtyDetail);
//                }
//            }

//            return true;
//        }

//        private List<int> GetNextBatch(IEnumerator<int> iterator, int NoOfContainer)
//        {
//            List<int> pagesToPrint = new List<int>();
//            for (int i = 0; i < NoOfContainer; i++)
//            {
//                iterator.MoveNext();
//                pagesToPrint.Add(iterator.Current);
//            }
//            return pagesToPrint;
//        }

//        private async Task<List<GRNMaterialLabelPrintingContainerBarcode>> PrintMaterialLabels(GRNMaterialLabelPrintingDto input, List<int> pagesToPrint, GRNLabelPrintingVendorDetail printLabelExtraDetails, MaterialLabelGRNQuantityDto grnQtyDetail = null)
//        {
//            List<GRNMaterialLabelPrintingContainerBarcode> lstMaterialLabelBarcodes
//                = new List<GRNMaterialLabelPrintingContainerBarcode>();
//            foreach (var containerNo in pagesToPrint)
//            {
//                var serialNo = $"{_masterCommonRepository.GetNextMaterialLabelBarcodeSequence():D10}";
//                var currentDate = DateTime.UtcNow;
//                var barCode = $"M-{currentDate:yy}-{serialNo}";
//                if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//                {
//                    await PrintMaterialBarcode(input, printLabelExtraDetails, grnQtyDetail, containerNo, barCode);

//                }
//                GRNMaterialLabelPrintingContainerBarcode barcodeData = new GRNMaterialLabelPrintingContainerBarcode
//                {
//                    CreatorUserId = AbpSession.UserId,
//                    CreationTime = DateTime.UtcNow,
//                    IsDeleted = false,
//                    GRNMaterialLabelPrintingHeaderId = input.MaterialLabelPrintHeaderId,
//                    MaterialLabelContainerBarCode = barCode,
//                    ContainerNo = containerNo,
//                    GRNDetailId = input.Id,
//                };
//                if (grnQtyDetail != null)
//                {
//                    barcodeData.BalanceQuantity = grnQtyDetail.QtyPerContainer;
//                    barcodeData.Quantity = grnQtyDetail.QtyPerContainer;
//                    barcodeData.GRNQtyDetailId = grnQtyDetail.GRNQuantityId;
//                }
//                lstMaterialLabelBarcodes.Add(barcodeData);
//            }
//            await _masterCommonRepository.BulkInsertMaterialLabelBarCode(lstMaterialLabelBarcodes);
//            return lstMaterialLabelBarcodes;
//        }

//        private async Task PrintMaterialLabelRanges(GRNMaterialLabelPrintingDto input, List<int> pagesToPrint, GRNLabelPrintingVendorDetail printLabelExtraDetails, MaterialLabelGRNQuantityDto grnQtyDetail = null)
//        {
//            var labelPrintingBarcodes = await _grnMaterialLabelPrintingContainerRepository.GetAll().Where(x =>
//            x.GRNMaterialLabelPrintingHeaderId == input.MaterialLabelPrintHeaderId
//              && pagesToPrint.Contains(x.ContainerNo))
//                .Select(x => new { x.ContainerNo, x.MaterialLabelContainerBarCode })
//                .ToDictionaryAsync(x => x.ContainerNo, y => y.MaterialLabelContainerBarCode) ?? new Dictionary<int, string>();
//            if (pagesToPrint.Except(labelPrintingBarcodes.Keys).Any())
//            {
//                throw new UserFriendlyException("One or more container does not found for range specified to re-print.");
//            }
//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                foreach (var containerNo in pagesToPrint)
//                {
//                    await PrintMaterialBarcode(input, printLabelExtraDetails, grnQtyDetail, containerNo, labelPrintingBarcodes[containerNo]);
//                }
//            }
//        }
//        private async Task PrintMaterialBarcode(GRNMaterialLabelPrintingDto input, GRNLabelPrintingVendorDetail printLabelExtraDetails, MaterialLabelGRNQuantityDto grnQtyDetail, int containerNo, string barCode)
//        {
//            var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                PrintBody = GetMaterialBarcodePrintBody(input, printLabelExtraDetails, grnQtyDetail, containerNo, barCode)
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);

//        }

//        private string GetMaterialBarcodePrintBody(GRNMaterialLabelPrintingDto input, GRNLabelPrintingVendorDetail printLabelExtraDetails, MaterialLabelGRNQuantityDto grnQtyDetail, int containerNo, string barCode)
//        {
//            var InspectionLotNo = _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode.ToLower() == input.MaterialCode.ToLower() && x.SAPBatchNo.ToLower() == input.SAPBatchNo.ToLower()).Select(x => x.InspectionLotNo).FirstOrDefault();
//            var printByUser = _userRepository.Get((int)AbpSession.UserId);
//            var materialLabelPRNFilePath = $"{_environment.WebRootPath}\\label_print_prn\\material_label.prn";
//            var materilLabelPRNFile = File.ReadAllText(materialLabelPRNFilePath);
//            var invdate = printLabelExtraDetails.InvoiceNo + " / " + printLabelExtraDetails.InvoiceDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
//            var grnnoDate = printLabelExtraDetails.GrnNumber + " / " + printLabelExtraDetails.GrnDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
//            var grncreate = printLabelExtraDetails.CreatedBy + " / " + printLabelExtraDetails.CreatedOn.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
//            var VendorcodeName = printLabelExtraDetails.VendorCode + " / " + printLabelExtraDetails.VendorName;
//            var ManufacturingCodeName = printLabelExtraDetails.ManufacturerCode + " / " + printLabelExtraDetails.ManufacturerName;

//            var args = new Dictionary<string, string>(
//        StringComparer.OrdinalIgnoreCase) {
//            {"{Matcode}", input.MaterialCode},
//            {"{MatDescp}", input.MaterialDescription},
//            {"{SapBatch}", input.SAPBatchNo},
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{VendorCodeName}",$"{VendorcodeName}" },
//            {"{MfgrCodeName}",$"{ManufacturingCodeName}" },
//            {"{MfgDate}",printLabelExtraDetails.ManufacturingDate.HasValue ? printLabelExtraDetails.ManufacturingDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty },
//            {"{ExpDate}",printLabelExtraDetails.ExpiryDate.HasValue?printLabelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) :string.Empty},
//            {"{MfgRetestDate}",printLabelExtraDetails.RetestDate.HasValue?printLabelExtraDetails.RetestDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture):string.Empty },
//            {"{PackDt}",input.PackDetails },
//            {"{NoofCont}",string.Format("{0} of {1}",containerNo,Convert.ToInt32(input.NumberOfContainers)) },
//            {"{TotRecQty}", string.Format("{0}",grnQtyDetail==null? input.TotalQty: grnQtyDetail.TotalQty) + " " + printLabelExtraDetails.UOM },
//            {"{PlantCode}",printLabelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{PrintbyDate}",printByUser.UserName + "/" + DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)},
//            {"{SerialNo}",barCode.Split("-")[2] },
//            {"{Barcode}",$"{barCode}"},

//            {"{PlantName}",$"{printLabelExtraDetails.PlantName}"},
//            {"{InvDate}",$"{invdate}"},
//            {"{GRNNoDate}",$"{grnnoDate}"},
//            {"{Mfgbatch}",$"{printLabelExtraDetails.MfgBatchNo}"},

//            {"{InspectionLotNo}",$"{InspectionLotNo}"},
//            {"{GRNPreparedbyDate}",$"{grncreate}"}
//            };
//            var newstr = args.Aggregate(materilLabelPRNFile, (current, value) => current.Replace(value.Key, value.Value));
//            return newstr;
//        }

//        private List<int> GetRangeArray(string RangePrint, float _pageCount, out bool isValid)
//        {
//            string[] customPageInputList = RangePrint.Split(',');
//            List<int> pageList = new List<int>();
//            isValid = true;
//            foreach (string str in customPageInputList)
//            {
//                if (str.Contains("-"))
//                {
//                    try
//                    {
//                        string[] pageRange = str.Split('-');

//                        if (pageRange.Length != 2 || int.Parse(pageRange[0]) > int.Parse(pageRange[1]) || int.Parse(pageRange[0]) <= 0 || int.Parse(pageRange[1]) > _pageCount)
//                        {
//                            isValid = false;
//                            break;
//                        }
//                        else
//                        {
//                            for (int i = int.Parse(pageRange[0]); i <= int.Parse(pageRange[1]); i++)
//                            {
//                                pageList.Add(i);
//                            }
//                        }
//                    }
//                    catch
//                    {
//                        isValid = false;
//                        break;
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        if (int.Parse(str) <= 0 || int.Parse(str) > _pageCount)
//                        {
//                            isValid = false;
//                            break;
//                        }
//                        else
//                        {
//                            pageList.Add(int.Parse(str));
//                        }
//                    }
//                    catch
//                    {
//                        isValid = false;
//                        break;
//                    }
//                }
//            }
//            return pageList;
//        }

//        private async Task<GRNRequestResponseDto> PostGRNPosting(CreateGRNPostingDto input)
//        {
//            var invoiceList = await _invoiceDetailRepository.GetAll().Where(x => x.PurchaseOrderId == input.PurchaseOrderId).ToListAsync();
//            var grnRequestResponseDto = new GRNRequestResponseDto();
//            var parentRowIndexs = input.GRNDetails.Select(a => a.ParentRow).Distinct().ToList();
//            var parentRows = input.GRNDetails.Where(a => parentRowIndexs.Contains(a.FormArrayIndex)).Distinct();
//            var grnRequests = new List<Record>();
//            foreach (var grnDetail in parentRows)
//            {
//                float totalNoOfContainers = 0;
//                float allQty = 0;
//                var qtyDetailList = new List<GRNPostingQtyDetailsDto>();
//                var childRows = input.GRNDetails.Where(a => a.ParentRow == grnDetail.ParentRow);
//                foreach (var qtyDetail in childRows)
//                {
//                    totalNoOfContainers += qtyDetail.NoOfContainer.Value;
//                    allQty += qtyDetail.TotalQty.Value;
//                }

//                var invoiceDetails = invoiceList.FirstOrDefault(x => x.Id == grnDetail.InvoiceId);
//                var mfgDetails = await _materialConsignmentRepository.FirstOrDefaultAsync(x => x.Id == grnDetail.MfgBatchNoId);
//                var material = await _materialRepository.GetAll().Where(x => x.Id == grnDetail.MaterialId).FirstOrDefaultAsync();
//                var lRDate = invoiceDetails.LRDate ?? default;
//                var MfgDate = mfgDetails.ManufacturedDate ?? default;
//                var ExpDate = mfgDetails.ExpiryDate ?? default;

//                var replacedlRDate = lRDate != default ? lRDate.ToString("d/M/yyyy").Replace("/", ".") : string.Empty;
//                var replacedMfgDate = MfgDate != default ? MfgDate.ToString("d/M/yyyy").Replace("/", ".") : string.Empty;
//                var replacedExpDate = ExpDate != default ? ExpDate.ToString("d/M/yyyy").Replace("/", ".") : string.Empty;
//                var grnPreparedBy = await _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).Select(x => x.FullName).FirstOrDefaultAsync();
//                var record = new Record()
//                {
//                    ItemCode = material.ItemCode,
//                    PurchaseOrder = invoiceDetails.PurchaseOrderNo,
//                    LRNo = invoiceDetails.LRNo,
//                    LRDate = replacedlRDate,
//                    TransporterName = invoiceDetails.TransporterName,
//                    Vehicle = invoiceDetails.VehicleNumber,
//                    MfgDate = replacedMfgDate,
//                    ExpDate = replacedExpDate,
//                    Manufacturer = material.ManufacturerCode,
//                    NoOfCases = totalNoOfContainers.ToString(),
//                    VendorBatch = mfgDetails.ManufacturedBatchNo,
//                    VendorCode = invoiceDetails.VendorCode,
//                    NetQty = allQty.ToString(),
//                    UOM = material.UnitOfMeasurement,
//                    POLineItem = material.ItemNo,
//                    Delivery_note_no = invoiceDetails.DeliveryNote,
//                    Storage_location = string.Empty,
//                    Bill_of_lading = invoiceDetails.BillofLanding,
//                    LineItem = grnDetail.LineItem,
//                    QtyPerContainer = grnDetail.QtyPerContainer.ToString(),
//                    InvoiceQty = grnDetail.ConsignmentQty.ToString(),
//                    Remark = grnDetail.DiscrepancyRemark,
//                    InvoiceNo = invoiceDetails.InvoiceNo,
//                    GRNPreparedBy = grnPreparedBy,
//                    No_Of_Containers = grnDetail.NoOfContainer.ToString(),
//                };
//                grnRequests.Add(record);
//            }
//            grnRequestResponseDto.Record = grnRequests;

//            var erpConnFactory = _eRPConnectorFactory.GetERPConnector(ERPConnectorType.SAPAjanta);
//            var result = await erpConnFactory.GRNPosting(grnRequestResponseDto);
//            return result;
//        }

//        private async Task InsertOrUpdateInspectionLot(GRNRequestResponseDto gRNRequestResponseDto)
//        {
//            var plantId = 0;
//            foreach (var material in gRNRequestResponseDto.Record)
//            {
//                plantId = await (from item in _materialRepository.GetAll()
//                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
//                                 on item.PurchaseOrderId equals purchaseOrder.Id
//                                 where item.ItemCode == material.ItemCode
//                                 select purchaseOrder.PlantId).FirstOrDefaultAsync();
//                var inspectionLot = new InspectionLot()
//                {
//                    InspectionLotNumber = material.InspectionLotNo,
//                    PlantId = plantId,
//                    TenantId = AbpSession.TenantId,
//                    ProductCode = material.ItemCode,
//                };
//                var insertedInspectionLotId = await _inspectionLotRepository.InsertAndGetIdAsync(inspectionLot);

//                var grnMaterial = await (from item in _materialRepository.GetAll()
//                                         join grndetail in _grnDetailRepository.GetAll()
//                                         on item.Id equals grndetail.MaterialId
//                                         join qty in _grnQtyDetailRepository.GetAll()
//                                         on grndetail.Id equals qty.GRNDetailId
//                                         join consignment in _materialConsignmentRepository.GetAll()
//                                         on grndetail.MfgBatchNoId equals consignment.Id
//                                         join purchaseOrder in _purchaseOrderRepository.GetAll()
//                                         on item.PurchaseOrderId equals purchaseOrder.Id
//                                         where item.ItemCode == material.ItemCode && grndetail.SAPBatchNumber == material.SAPBatchNo
//                                         select new QCMaterialDto
//                                         {
//                                             ItemCode = material.ItemCode,
//                                             SAPBatchNo = material.SAPBatchNo,
//                                             RetestDate = Convert.ToDateTime(consignment.RetestDate),
//                                             ExpiryDate = Convert.ToDateTime(consignment.ExpiryDate),
//                                             UnitOfMeasurement = item.UnitOfMeasurement,
//                                             BatchNo = grndetail.SAPBatchNumber,
//                                             ItemNo = item.ItemNo,
//                                             ItemDescription = item.ItemDescription,
//                                             OrderQuantity = qty.TotalQty,
//                                         }).ToListAsync();

//                grnMaterial = grnMaterial.GroupBy(a => a.SAPBatchNo).Select(a => new QCMaterialDto
//                {
//                    ItemCode = a.First().ItemCode,
//                    SAPBatchNo = a.First().SAPBatchNo,
//                    RetestDate = a.First().RetestDate,
//                    ExpiryDate = a.First().ExpiryDate,
//                    UnitOfMeasurement = a.First().UnitOfMeasurement,
//                    BatchNo = a.First().BatchNo,
//                    ItemNo = a.First().ItemNo,
//                    ItemDescription = a.First().ItemDescription,
//                    OrderQuantity = a.Sum(a => a.OrderQuantity),
//                    GRNHeaderId = a.First().GRNHeaderId
//                }).ToList();

//                var processOrderMaterial = new ProcessOrderMaterial
//                {
//                    ItemCode = material.ItemCode,
//                    InspectionLotId = insertedInspectionLotId,
//                    RetestDate = Convert.ToDateTime(grnMaterial.FirstOrDefault().RetestDate),
//                    ExpiryDate = Convert.ToDateTime(grnMaterial.FirstOrDefault().ExpiryDate),
//                    UnitOfMeasurement = grnMaterial.FirstOrDefault().UnitOfMeasurement,
//                    BatchNo = grnMaterial.FirstOrDefault().BatchNo,
//                    SAPBatchNo = material.SAPBatchNo,
//                    ItemNo = grnMaterial.FirstOrDefault().ItemNo,
//                    ItemDescription = grnMaterial.FirstOrDefault().ItemDescription,
//                    OrderQuantity = grnMaterial.FirstOrDefault().OrderQuantity,
//                    InspectionLotNo = material.InspectionLotNo
//                };
//                await _processOrderMaterialRepository.InsertAsync(processOrderMaterial);
//            }
//        }

//        private async Task InsertUpdateSAPGRNPostingAsync(GRNRequestResponseDto gRNRequestResponseDto)
//        {
//            foreach (var item in gRNRequestResponseDto.Record)
//            {
//                var sapGrnpostingDetail = ObjectMapper.Map<SAPGRNPosting>(item);
//                await _sapGRNPostingRepository.InsertAsync(sapGrnpostingDetail);
//                CurrentUnitOfWork.SaveChanges();
//            }

//        }
//        private async Task<GRNPostingDto> InsertUpdateInternalGRNPostingAsync(GRNRequestResponseDto gRNRequestResponseDto, CreateGRNPostingDto input)
//        {
//            var currentDate = DateTime.UtcNow;
//            var gRNHeader = ObjectMapper.Map<GRNHeader>(input);
//            gRNHeader.TenantId = AbpSession.TenantId;
//            gRNHeader.GRNNumber = (string)gRNRequestResponseDto.Record.FirstOrDefault().GRNNo;
//            var parentRowIndexs = input.GRNDetails.Select(a => a.ParentRow).Distinct().ToList();
//            var parentRows = input.GRNDetails.Where(a => parentRowIndexs.Contains(a.FormArrayIndex)).Distinct();

//            foreach (var grnDetail in parentRows)
//            {
//                float totalNoOfContainers = 0;
//                grnDetail.SAPBatchNumber = gRNRequestResponseDto.Record.Where(a => a.ItemCode == grnDetail.ItemCode && a.LineItem == grnDetail.LineItem)?.FirstOrDefault().SAPBatchNo;
//                grnDetail.TenantId = AbpSession.TenantId;
//                var qtyDetailList = new List<GRNPostingQtyDetailsDto>();
//                var childRows = input.GRNDetails.Where(a => a.ParentRow == grnDetail.ParentRow);
//                foreach (var qtyDetail in childRows)
//                {
//                    totalNoOfContainers += qtyDetail.NoOfContainer.Value;

//                    var qtyToInsert = new GRNPostingQtyDetailsDto
//                    {
//                        TotalQty = qtyDetail.TotalQty ?? 0,
//                        NoOfContainer = qtyDetail.NoOfContainer ?? 0,
//                        QtyPerContainer = qtyDetail.QtyPerContainer ?? 0,
//                        DiscrepancyRemark = qtyDetail.DiscrepancyRemark,
//                        IsDamaged = qtyDetail.IsDamaged
//                    };
//                    qtyDetailList.Add(qtyToInsert);
//                }
//                grnDetail.GRNQtyDetails = qtyDetailList.ToList();
//                grnDetail.NoOfContainer = totalNoOfContainers;
//                grnDetail.TotalQty = 0;
//                grnDetail.QtyPerContainer = 0;
//            }
//            gRNHeader.GRNDetails = parentRows.Select(MapGRNDetailsDto).ToList();
//            await _grnHeaderRepository.InsertAsync(gRNHeader);
//            var grnheader = ObjectMapper.Map<GRNPostingDto>(gRNHeader);
//            CurrentUnitOfWork.SaveChanges();
//            return grnheader;
//        }
//    }
//}