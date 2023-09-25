//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonService.Invoices;
//using ELog.Application.Inward.GateEntries.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Authorization.Users;
//using ELog.Core.BarcodeGeneration;
//using ELog.Core.Entities;
//using ELog.Core.Printer;
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
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Inward.GateEntries
//{
//    [PMMSAuthorize]
//    public class GateEntryAppService : ApplicationService, IGateEntryAppService
//    {
//        private const string formatNumber = "FDSTG/F/013,Version:01";
//        private const string sopNo = "FDSOP/GHT/ST/018";

//        private readonly IRepository<GateEntry> _gateEntryRepository;
//        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private readonly IInvoiceService _invoiceService;
//        private readonly IRepository<InvoiceDetail> _invoiceRepository;
//        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionHeaderRepository;
//        private readonly IRepository<MaterialInspectionHeader> _materialHeaderRepository;
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly PrinterFactory _printerFactory;
//        private readonly IConfiguration _configuration;
//        private readonly IWebHostEnvironment _environment;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<User, long> _userRepository;
//        private readonly IRepository<Material> _materialRepository;

//        public GateEntryAppService(IRepository<GateEntry> gateEntryRepository,
//           IRepository<PurchaseOrder> purchaseOrderRepository,
//          IRepository<DeviceMaster> deviceRepository, IHttpContextAccessor httpContextAccessor,
//          IInvoiceService invoiceService, IMasterCommonRepository masterCommonRepository, IRepository<Material> materialRepository,
//          IRepository<InvoiceDetail> invoiceRepository, IRepository<VehicleInspectionHeader> vehicleInspectionHeaderRepository,
//          IRepository<MaterialInspectionHeader> materialHeaderRepository, PrinterFactory printerFactory, IConfiguration configuration, IWebHostEnvironment environment,
//          IRepository<PlantMaster> plantRepository, IRepository<User, long> userRepository)
//        {
//            _gateEntryRepository = gateEntryRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _purchaseOrderRepository = purchaseOrderRepository;
//            _deviceRepository = deviceRepository;
//            _invoiceService = invoiceService;
//            _invoiceRepository = invoiceRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _vehicleInspectionHeaderRepository = vehicleInspectionHeaderRepository;
//            _materialHeaderRepository = materialHeaderRepository;
//            _printerFactory = printerFactory;
//            _configuration = configuration;
//            _environment = environment;
//            _plantRepository = plantRepository;
//            _userRepository = userRepository;
//            _materialRepository = materialRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GateEntry_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<GateEntryDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _gateEntryRepository.GetAsync(input.Id);
//            var invoiceDetailEntity = new EntityDto<int>
//            {
//                Id = entity.InvoiceId ?? 0
//            };
//            var consignmentDetails = await _invoiceService.GetAsync(invoiceDetailEntity);
//            var GateEntry = ObjectMapper.Map<GateEntryDto>(entity);
//            GateEntry.InvoiceDto = consignmentDetails;
//            return GateEntry;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GateEntry_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<GateEntryListDto>> GetAllAsync(PagedGateEntryResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<GateEntryListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GateEntry_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<GateEntryDto> CreateAsync(CreateGateEntryDto input)
//        {
//            if (await IsGateEntryPresent(input.InvoiceDto.PurchaseOrderNo, input.InvoiceDto.InvoiceNo))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.GatePassNoAlreadyExist, input.InvoiceDto.PurchaseOrderNo + "-" + input.InvoiceDto.InvoiceNo));
//            }
//            var GateEntry = ObjectMapper.Map<GateEntry>(input);
//            var currentDate = DateTime.UtcNow;
//            GateEntry.TenantId = AbpSession.TenantId;
//            GateEntry.GatePassNo = $"G-{currentDate:yy}-{_masterCommonRepository.GetNextGateEntryBarcodeSequence():D10}";
//            if (GateEntry.GatePassNo == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//            }
//            BarcodeGeneratorHelper.GetBarCode(GateEntry.GatePassNo);
//            var existingInvoiceId = _invoiceRepository.GetAll().Where(x => x.PurchaseOrderNo.Trim().ToLower() == input.InvoiceDto.PurchaseOrderNo.Trim().ToLower()
//                                                                   && x.InvoiceNo.Trim().ToLower() == input.InvoiceDto.InvoiceNo.Trim().ToLower()).Select(x => x.Id)?.FirstOrDefault();
//            if (existingInvoiceId != null && existingInvoiceId != 0)
//            {
//                var invoiceData = await _invoiceRepository.GetAsync(existingInvoiceId.GetValueOrDefault());
//                ObjectMapper.Map(input.InvoiceDto, invoiceData);
//                await _invoiceRepository.UpdateAsync(invoiceData);
//                GateEntry.InvoiceId = existingInvoiceId;
//            }
//            else
//            {
//                var insertedInvoice = await _invoiceService.CreateAsync(input.InvoiceDto);
//                GateEntry.InvoiceId = insertedInvoice?.Id;
//            }
//            await _gateEntryRepository.InsertAsync(GateEntry);
//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//                await PrintGatePass(device, GateEntry);
//            }
//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<GateEntryDto>(GateEntry);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GateEntry_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<GateEntryDto> CreateGatePassAsync(CreateGateEntryDto[] input)
//        {
//            var GateEntry = new GateEntry();
//            string gatePassNo = $"G-{DateTime.UtcNow:yy}-{_masterCommonRepository.GetNextGateEntryBarcodeSequence():D10}";
//            for (int i = 0; i < input.Length; i++)
//            {
//                if (await IsGateEntryPresent(input[i].InvoiceDto.PurchaseOrderNo, input[i].InvoiceDto.InvoiceNo))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.GatePassNoAlreadyExist, input[i].InvoiceDto.PurchaseOrderNo + "-" + input[i].InvoiceDto.InvoiceNo));
//                }

//                GateEntry = ObjectMapper.Map<GateEntry>(input[i]);
//                var currentDate = DateTime.UtcNow;
//                GateEntry.TenantId = AbpSession.TenantId;
//                GateEntry.GatePassNo = gatePassNo;
//                if (GateEntry.GatePassNo == null)
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//                }
//                BarcodeGeneratorHelper.GetBarCode(GateEntry.GatePassNo);
//                var existingInvoiceId = _invoiceRepository.GetAll().Where(x => x.PurchaseOrderNo.Trim().ToLower() == input[i].InvoiceDto.PurchaseOrderNo.Trim().ToLower()
//                                                                       && x.InvoiceNo.Trim().ToLower() == input[i].InvoiceDto.InvoiceNo.Trim().ToLower()).Select(x => x.Id)?.FirstOrDefault();
//                if (existingInvoiceId != null && existingInvoiceId != 0)
//                {
//                    var invoiceData = await _invoiceRepository.GetAsync(existingInvoiceId.GetValueOrDefault());
//                    ObjectMapper.Map(input[i].InvoiceDto, invoiceData);
//                    await _invoiceRepository.UpdateAsync(invoiceData);
//                    GateEntry.InvoiceId = existingInvoiceId;
//                }
//                else
//                {
//                    var insertedInvoice = await _invoiceService.CreateAsync(input[i].InvoiceDto);
//                    GateEntry.InvoiceId = insertedInvoice?.Id;
//                }
//                await _gateEntryRepository.InsertAsync(GateEntry);
//                if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//                {
//                    var device = await _deviceRepository.GetAsync(Convert.ToInt32(input[i].PrinterId));
//                    await PrintGatePass(device, GateEntry);
//                }
//                foreach (var material in input[i].Materials)
//                {
//                    var existingMaterials = await _materialRepository.GetAsync(material.Id);
//                    existingMaterials.BalanceQuantity = material.BalanceQuantity;
//                    existingMaterials.ManufacturerCode = existingMaterials.ManufacturerCode ?? material.ManufacturerCode;
//                    existingMaterials.ManufacturerName = existingMaterials.ManufacturerName ?? material.ManufacturerName;
//                    await _materialRepository.UpdateAsync(existingMaterials);
//                }
//                CurrentUnitOfWork.SaveChanges();
//            }
//            return ObjectMapper.Map<GateEntryDto>(GateEntry);
//        }

//        private async Task PrintGatePass(DeviceMaster device, GateEntry gateEntry)
//        {
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                PrintBody = await GetGateEntryPrintBody(gateEntry)
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.GateEntry_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<GateEntryDto> UpdateAsync(UpdateGateEntryDto input)
//        {
//            if (!input.IsActive)
//            {
//                if (await IsGatePassAssociatedWithAnyTransactions(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GateEntryRejected);
//                }
//            }
//            var GateEntry = await _gateEntryRepository.GetAsync(input.Id);
//            ObjectMapper.Map(input, GateEntry);
//            await _gateEntryRepository.UpdateAsync(GateEntry);
//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//                await PrintGatePass(device, GateEntry);
//            }
//            return await GetAsync(input);
//        }

//        private async Task<bool> IsGatePassAssociatedWithAnyTransactions(int id)
//        {
//            return await (from gateEntry in _gateEntryRepository.GetAll().Where(x => x.IsActive)
//                          join vehicleInspection in _vehicleInspectionHeaderRepository.GetAll()
//                          on gateEntry.Id equals vehicleInspection.GateEntryId into vehicleInspectionHeader
//                          from vehicleInspection in vehicleInspectionHeader.DefaultIfEmpty()

//                          join materialInspection in _materialHeaderRepository.GetAll()
//                          on gateEntry.Id equals materialInspection.GateEntryId into materialInspectionData
//                          from materialInspection in materialInspectionData.DefaultIfEmpty()
//                          where gateEntry.Id == id
//                          select new
//                          {
//                              vehicleInspectionId = vehicleInspection.GateEntryId,
//                              materialInspectionId = materialInspection.GateEntryId
//                          }).AnyAsync(x => x.vehicleInspectionId > 0 || x.materialInspectionId > 0);
//        }

//        private async Task<bool> IsGateEntryPresent(string PO_Number, string invoice_Number)
//        {
//            var invoice = await _invoiceRepository.GetAll().FirstOrDefaultAsync(x =>
//                                                                  x.PurchaseOrderNo.Trim().ToLower() == PO_Number.Trim().ToLower()
//                                                                  && x.InvoiceNo.Trim().ToLower() == invoice_Number.Trim().ToLower());
//            if (invoice != null)
//            {
//                if (await _gateEntryRepository.GetAll().AnyAsync(x => x.InvoiceId == invoice.Id && x.IsActive))
//                {
//                    return true;
//                }
//                else if (await _gateEntryRepository.GetAll().AnyAsync(x => x.InvoiceId == invoice.Id && !x.IsActive))
//                {
//                    return false;
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<GateEntryListDto> ApplySorting(IQueryable<GateEntryListDto> query, PagedGateEntryResultRequestDto input)
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
//        protected IQueryable<GateEntryListDto> ApplyPaging(IQueryable<GateEntryListDto> query, PagedGateEntryResultRequestDto input)
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

//        protected IQueryable<GateEntryListDto> CreateUserListFilteredQuery(PagedGateEntryResultRequestDto input)
//        {
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var gateEntryQuery = from gateEntry in _gateEntryRepository.GetAll()
//                                 join invoice in _invoiceRepository.GetAll()
//                                 on gateEntry.InvoiceId equals invoice.Id
//                                 join po in _purchaseOrderRepository.GetAll()
//                                 on invoice.PurchaseOrderId equals po.Id into ps
//                                 from po in ps.DefaultIfEmpty()
//                                 select new GateEntryListDto
//                                 {
//                                     GatePassNumber = gateEntry.GatePassNo,
//                                     Id = gateEntry.Id,
//                                     PurchaseOrderId = po.Id,
//                                     UserEnteredPurchaseOrderNumber = po.PurchaseOrderNo,
//                                     IsActive = gateEntry.IsActive,
//                                     SubPlantId = po.PlantId,
//                                     PurchaseOrderDeliverSchedule = invoice.purchaseOrderDeliverSchedule

//                                 };
//            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            {
//                gateEntryQuery = gateEntryQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
//            }
//            if (input.PurchaseOrderId != null)
//            {
//                gateEntryQuery = gateEntryQuery.Where(x => x.PurchaseOrderId == input.PurchaseOrderId);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                gateEntryQuery = gateEntryQuery.Where(x => x.GatePassNumber.Contains(input.Keyword) || x.GatePassNumber.Contains(input.Keyword));
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    gateEntryQuery = gateEntryQuery.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    gateEntryQuery = gateEntryQuery.Where(x => x.IsActive);
//                }
//            }
//            return gateEntryQuery;
//        }
//        private async Task<string> GetGateEntryPrintBody(GateEntry input)
//        {
//            var printByUser = await _userRepository.GetAsync((int)AbpSession.UserId);
//            var invoiceData = await _invoiceRepository.GetAsync((int)input.InvoiceId);
//            var poExtraDetails = await (from purchaseOrder in _purchaseOrderRepository.GetAll()
//                                        join plant in _plantRepository.GetAll()
//                                        on purchaseOrder.PlantId equals plant.Id
//                                        where purchaseOrder.Id == invoiceData.PurchaseOrderId
//                                        select new { plant.PlantId, plant.PlantName, purchaseOrder.PurchaseOrderDate }).FirstOrDefaultAsync();

//            var gateEntryprnFilePath = $"{_environment.WebRootPath}\\gateentry_prn\\GateEntry.prn";
//            var gateEntryPRNFile = File.ReadAllText(gateEntryprnFilePath);
//            var args = new Dictionary<string, string>(
//        StringComparer.OrdinalIgnoreCase) {
//            {"{PO}", invoiceData.PurchaseOrderNo},
//            {"{PODate}", poExtraDetails.PurchaseOrderDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{Transporter}",invoiceData.TransporterName },
//            {"{InvoiceNo}",invoiceData.InvoiceNo },
//            {"{LRNo}",invoiceData.LRNo},
//            {"{LRDate}",invoiceData.LRDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{Driver}",invoiceData.DriverName},

//            {"{PlantCode}",poExtraDetails.PlantId},
//            {"{Printby }", printByUser.UserName},
//            {"{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",input.GatePassNo },
//            {"{Barcode}",$"{input.GatePassNo}"},
//            {"{PlantName}",$"{poExtraDetails.PlantName}"},
//            {"{Vehicle}",invoiceData.VehicleNumber },
//            };

//            var newstr = args.Aggregate(gateEntryPRNFile, (current, value) => current.Replace(value.Key, value.Value));
//            return newstr;
//        }
//    }
//}