//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.SelectLists.Dto;
//using ELog.Application.WIP.WIPPrintPacking.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
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
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.WIP.WIPPrintPacking
//{
//    [PMMSAuthorize]
//    public class PrintPackingService : ApplicationService, IPrintPackingService
//    {
//        private readonly IRepository<LabelPrintPacking> _labelPrintPackingrepository;
//        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
//        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
//        private readonly IRepository<ELog.Core.Entities.InProcessLabelDetails> _processLabelRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private readonly IRepository<RecipeWiseProcessOrderMapping> _recipewisepoRepository;
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly PrinterFactory _printerFactory;
//        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
//        private readonly IWebHostEnvironment _environment;

//        public PrintPackingService(IRepository<LabelPrintPacking> labelPrintPackingrepository,
//         IRepository<ProcessOrderAfterRelease> processOrderRepository,
//         IHttpContextAccessor httpContextAccessor,
//         IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository,
//         IRepository<ELog.Core.Entities.InProcessLabelDetails> processLabelRepository, IMasterCommonRepository masterCommonRepository,
//         IRepository<DeviceMaster> deviceRepository, IRepository<RecipeWiseProcessOrderMapping> recipewisepoRepository,
//        PrinterFactory printerFactory, Microsoft.Extensions.Configuration.IConfiguration configuration, IWebHostEnvironment environment)
//        {
//            _labelPrintPackingrepository = labelPrintPackingrepository;
//            _processOrderRepository = processOrderRepository;
//            _processOrdermaterialRepository = processOrdermaterialRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _httpContextAccessor = httpContextAccessor;
//            _processLabelRepository = processLabelRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _deviceRepository = deviceRepository;
//            _printerFactory = printerFactory;
//            _configuration = configuration;
//            _environment = environment;
//            _recipewisepoRepository = recipewisepoRepository;
//        }

//        public async Task<PrintPackingDto> CreateAsync(CreatePrintPackingDto input)
//        {
//            var printPacking = ObjectMapper.Map<LabelPrintPacking>(input);
//            var currentDate = DateTime.UtcNow;
//            //  ProcessLabel.TenantId = AbpSession.TenantId;
//            printPacking.PackingLabelBarcode = $"Pack-{currentDate:yy}-{_masterCommonRepository.GetNextGateEntryBarcodeSequence():D10}";
//            if (printPacking.PackingLabelBarcode == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//            }
//            //BarcodeGeneratorHelper.GetBarCode(printPacking.PackingLabelBarcode);

//            //if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            //{
//            //    var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            //    await PrintPackingLabelBarcode(device, printPacking);
//            //}
//            await _labelPrintPackingrepository.InsertAsync(printPacking);
//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<PrintPackingDto>(printPacking);
//        }

//        public async Task Print(PrintPackingDto printPacking)
//        {

//            BarcodeGeneratorHelper.GetBarCode(printPacking.PackingLabelBarcode);

//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(printPacking.PrinterId));
//                await PrintPackingLabelBarcode(device, printPacking);
//            }
//        }

//        private async Task PrintPackingLabelBarcode(DeviceMaster device, PrintPackingDto labelprintpacking)
//        {
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                //PrintBody = await GetPackingPrintBody(labelprintpacking)
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);
//        }

//        public async Task<PrintPackingDto> GetAsync(EntityDto<int> input)
//        {
//            //var entity = await _labelPrintPackingrepository.GetAsync(input.Id);
//            //return ObjectMapper.Map<PrintPackingDto>(entity);
//            var printpackingdata = (from pack in _labelPrintPackingrepository.GetAll()
//                                    join pro in _processOrderRepository.GetAll()
//                                    on pack.ProcessOrderId equals pro.Id into poms
//                                    from pro in poms.DefaultIfEmpty()
//                                    join lbl in _processLabelRepository.GetAll()
//                                    on pack.ContainerBarcodeId equals lbl.Id into los
//                                    from lbl in los.DefaultIfEmpty()
//                                    where pack.Id == input.Id
//                                    select new PrintPackingDto
//                                    {
//                                        Id = pack.Id,
//                                        ProcessOrderId = pack.ProcessOrderId,
//                                        ProcessOrderNo = pro.ProcessOrderNo,
//                                        ProductCode = Convert.ToString(pack.ProductId),
//                                        ContainerId = lbl.Id,
//                                        ContainerBarcode = lbl.ProcessLabelBarcode,
//                                        ContainerCount = pack.ContainerCount,
//                                        Quantity = pack.Quantity

//                                    });
//            //packingdata.Cast<PackingDto>().ToArray()
//            var entities = await AsyncQueryableExecuter.FirstOrDefaultAsync(printpackingdata);
//            return entities;
//        }
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            var approvalUserModule = await _labelPrintPackingrepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _labelPrintPackingrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
//        }

//        public async Task<PrintPackingDto> GetListAsync(EntityDto<int> input)
//        {
//            var printpackingdata = (from pac in _labelPrintPackingrepository.GetAll()
//                                    join po in _processOrderRepository.GetAll()
//                                    on pac.ProcessOrderId equals po.Id into pos
//                                    from po in pos.DefaultIfEmpty()
//                                    join poMaterial in _processOrdermaterialRepository.GetAll()
//                                     on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                                    from poMaterial in pomaterials.DefaultIfEmpty()
//                                    where pac.Id == input.Id

//                                    select new PrintPackingDto
//                                    {
//                                        Id = pac.Id,
//                                        ProductCode = po.ProductCode,
//                                        ProductName = poMaterial.MaterialDescription,
//                                        BatchNo = poMaterial.SAPBatchNo,
//                                        ProcessOrderId = po.Id,
//                                        ProcessOrderNo = po.ProcessOrderNo,
//                                        ContainerBarcode = pac.ContainerBarcode,
//                                        ContainerCount = pac.ContainerCount,
//                                        Quantity = pac.Quantity,
//                                        PackingLabelBarcode = pac.PackingLabelBarcode,
//                                        IsPrint = pac.IsPrint,
//                                        PrinterId = pac.PrinterId,
//                                        PrintCount = pac.PrintCount

//                                    });

//            var entities = await AsyncQueryableExecuter.FirstOrDefaultAsync(printpackingdata);
//            return entities;
//        }

//        public async Task<PrintPackingDto> GetProductNamebyProductcodeAsync(EntityDto<string> input)
//        {
//            var printpackingdata = (
//                                    from po in _processOrderRepository.GetAll()
//                                    join poMaterial in _processOrdermaterialRepository.GetAll()
//                                     on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                                    from poMaterial in pomaterials.DefaultIfEmpty()
//                                    where po.ProductCode == input.Id

//                                    select new PrintPackingDto
//                                    {
//                                        Id = po.Id,
//                                        ProductCode = po.ProductCode,
//                                        ProductName = poMaterial.MaterialDescription,



//                                    });

//            var entities = await AsyncQueryableExecuter.FirstOrDefaultAsync(printpackingdata);
//            return entities;
//        }

//        public async Task<PrintPackingDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
//        {


//            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

//            var batchNos = from po in _processOrderRepository.GetAll()
//                           join material in _processOrdermaterialRepository.GetAll()
//                           on po.Id equals material.ProcessOrderId into areaps
//                           from material in areaps.DefaultIfEmpty()
//                           where po.Id == input
//                           select new PrintPackingDto
//                           {
//                               ProcessOrderNo = po.ProcessOrderNo,
//                               ProductCode = po.ProductCode,
//                               BatchNo = material.SAPBatchNo,
//                           };
//            var data = batchNos.FirstOrDefault();
//            PrintPackingDto printPackingDto = new PrintPackingDto();
//            printPackingDto.ProcessOrderNo = data.ProcessOrderNo;
//            printPackingDto.ProductCode = data.ProductCode;
//            printPackingDto.BatchNo = data.BatchNo;


//            return printPackingDto;
//        }


//        public async Task<PrintPackingDto> UpdateAsync(PrintPackingDto input)
//        {
//            var packing = await _labelPrintPackingrepository.GetAsync(input.Id);
//            ObjectMapper.Map(input, packing);
//            await _labelPrintPackingrepository.UpdateAsync(packing);
//            return await GetAsync(input);
//        }
//        public async Task<List<SelectListDtoWithPlantId>> GetAllProcessLabelCodeAsync(string input)
//        {
//            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var processlabel = from po in _processLabelRepository.GetAll()
//                               select new SelectListDtoWithPlantId
//                               {
//                                   Id = po.Id,
//                                   // PlantId = po.PlantId,
//                                   Value = po.ProcessLabelBarcode,
//                               };
//            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            //{
//            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
//            //}
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                input = input.Trim();
//                processlabel = processlabel.Where(x => x.Value.Contains(input)).Distinct();
//                return await processlabel?.ToListAsync() ?? default;
//            }
//            return await processlabel.ToListAsync() ?? default;
//        }
//        public async Task<PagedResultDto<PrintPackingListDto>> GetAllAsync(PagedPrintPackingResultRequestDto input)
//        {
//            var query = CreatePrintPickingFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<PrintPackingListDto>(
//                totalCount,
//                entities
//            );
//        }

//        protected IQueryable<PrintPackingListDto> CreatePrintPickingFilteredQuery(PagedPrintPackingResultRequestDto input)
//        {
//            var PrintPackingQuery = from pac in _labelPrintPackingrepository.GetAll()
//                                    join po in _processOrderRepository.GetAll()
//                                    on pac.ProcessOrderId equals po.Id into pos
//                                    from po in pos.DefaultIfEmpty()
//                                    join poMaterial in _processOrdermaterialRepository.GetAll()
//                                     on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                                    from poMaterial in pomaterials.DefaultIfEmpty()
//                                    select new PrintPackingListDto
//                                    {
//                                        Id = pac.Id,
//                                        ProductCode = po.ProductCode,
//                                        ProductName = poMaterial.MaterialDescription,
//                                        BatchNo = poMaterial.SAPBatchNo,
//                                        ProcessOrderId = po.Id,
//                                        ProcessOrderNo = po.ProcessOrderNo,
//                                        ContainerBarcode = pac.ContainerBarcode,
//                                        ContainerCount = pac.ContainerCount,
//                                        Quantity = pac.Quantity,
//                                        PackingLabelBarcode = pac.PackingLabelBarcode,
//                                        IsPrint = pac.IsPrint

//                                    };

//            if (input.ProcessOrderNo != null)
//            {
//                PrintPackingQuery = PrintPackingQuery.Where(x => x.ProcessOrderNo == input.ProcessOrderNo);
//            }
//            if (input.ProductCode != null)
//            {
//                PrintPackingQuery = PrintPackingQuery.Where(x => x.ProductCode == input.ProductCode);
//            }
//            if (input.ContainerBarcode != null)
//            {
//                PrintPackingQuery = PrintPackingQuery.Where(x => x.ContainerBarcode == input.ContainerBarcode);
//            }
//            if (input.BatchNo != null)
//            {
//                PrintPackingQuery = PrintPackingQuery.Where(x => x.BatchNo == input.BatchNo);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                PrintPackingQuery = PrintPackingQuery.Where(x =>
//                x.ContainerBarcode.Contains(input.Keyword)
//                || x.ProductCode.Contains(input.Keyword)
//                || x.ProcessOrderNo.Contains(input.Keyword)
//                || x.BatchNo.Contains(input.Keyword)
//                || x.PackingLabelBarcode.Contains(input.Keyword));
//            }



//            return PrintPackingQuery;
//        }

//        public async Task<List<SelectListDto>> GetAllContainerBarcode()
//        {
//            var list = await (from pac in _labelPrintPackingrepository.GetAll()
//                              join po in _processOrderRepository.GetAll()
//                              on pac.ProcessOrderId equals po.Id into pos
//                              from po in pos.DefaultIfEmpty()
//                              join poMaterial in _processOrdermaterialRepository.GetAll()
//                               on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                              from poMaterial in pomaterials.DefaultIfEmpty()
//                              select new SelectListDto
//                              {
//                                  Id = pac.ContainerBarcodeId,
//                                  Value = pac.ContainerBarcode
//                              }).ToListAsync();
//            return list ?? default;
//        }

//        protected IQueryable<PrintPackingListDto> ApplySorting(IQueryable<PrintPackingListDto> query, PagedPrintPackingResultRequestDto input)
//        {
//            //Try to sort query if available
//            var sortInput = input as ISortedResultRequest;
//            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
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
//        protected IQueryable<PrintPackingListDto> ApplyPaging(IQueryable<PrintPackingListDto> query, PagedPrintPackingResultRequestDto input)
//        {
//            //Try to use paging if available
//            if (input is IPagedResultRequest pagedInput)
//            {
//                return query.PageBy(pagedInput);
//            }

//            //Try to limit query result if available
//            if (input is ILimitedResultRequest limitedInput)
//            {
//                return query.Take(limitedInput.MaxResultCount);
//            }

//            //No paging
//            return query;
//        }
//        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
//        {
//            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var productCodes = from po in _processOrderRepository.GetAll()
//                               select new SelectListDtoWithPlantId
//                               {
//                                   Id = po.ProductCodeId,
//                                   // PlantId = po.PlantId,
//                                   Value = po.ProductCode,
//                               };
//            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            //{
//            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
//            //}
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                input = input.Trim();
//                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
//                return await productCodes?.ToListAsync() ?? default;
//            }
//            return await productCodes.ToListAsync() ?? default;
//        }
//        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
//        {
//            if ((input != null) || (input == null))
//            {
//                //var processOrders = await _processOrderRepository.GetAll().Select(po => new SelectListDto
//                //{
//                //    Id = po.Id,
//                //    Value = po.ProcessOrderNo,
//                //}).ToListAsync();
//                var processOrders = await (from p in _recipewisepoRepository.GetAll()
//                                           join po in _processOrderRepository.GetAll()
//                                           on p.ProcessOrderId equals po.Id
//                                           where po.ProductCode == input
//                                           select new SelectListDto
//                                           {
//                                               Id = p.ProcessOrderId,
//                                               Value = po.ProcessOrderNo
//                                           }).ToListAsync();
//                return processOrders;
//            }

//            return default;
//        }

//        public async Task<List<PrintPackingListDto>> GetProcessOrderDetailsAsync(int processOrderId)
//        {
//            var processOrder = await (from processorder in _processOrderRepository.GetAll()
//                                      where processorder.Id == processOrderId
//                                      orderby processorder.ProcessOrderNo
//                                      select new PrintPackingListDto
//                                      {

//                                          Id = processorder.Id,
//                                          //BatchNo = processorder.SAPBatchNo,
//                                      }).ToListAsync() ?? default;

//            return processOrder;
//        }

//        //private async Task<string> GetPackingPrintBody(LabelPrintPacking input)
//        //{
//        //   var poExtraDetails = await (from purchaseOrder in _purchaseOrderRepository.GetAll()
//        //                                join plant in _plantRepository.GetAll()
//        //                                on purchaseOrder.PlantId equals plant.Id
//        //                                where purchaseOrder.Id == invoiceData.PurchaseOrderId
//        //                                select new { plant.PlantId, purchaseOrder.PurchaseOrderDate }).FirstOrDefaultAsync();

//        //    var processlabelprnFilePath = $"{_environment.WebRootPath}\\gateentry_prn\\GateEntry.prn";
//        //    var processlabelPRNFile = File.ReadAllText(processlabelprnFilePath);
//        //    var args = new Dictionary<string, string>(
//        //StringComparer.OrdinalIgnoreCase) {
//        //    {"{PO}", invoiceData.PurchaseOrderNo},
//        //    {"{PODate}", poExtraDetails.PurchaseOrderDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//        //    {"{FormatNo}","" },
//        //    {"{SOPNo}","" },
//        //    {"{Transporter}",invoiceData.TransporterName },
//        //    {"{InvoiceNo}",invoiceData.InvoiceNo },
//        //    {"{LRNo}",invoiceData.LRNo},
//        //    {"{LRDate}",invoiceData.LRDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//        //    {"{Driver}",invoiceData.DriverName},
//        //    {"{Vehicle}",invoiceData.VehicleNumber },
//        //    {"{PlantCode}",poExtraDetails.PlantId},
//        //    {"{Print By /Date}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//        //    {"{SerialNo}",input.GatePassNo },
//        //    {"{Barcode}",$"{input.GatePassNo}"},
//        //};
//        //    var newstr = args.Aggregate(processlabelPRNFile, (current, value) => current.Replace(value.Key, value.Value));
//        //    return newstr;
//        //}

//    }
//}
