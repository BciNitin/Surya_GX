

//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.SelectLists.Dto;
//using ELog.Application.WIP.CageLabelPrint.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.BarcodeGeneration;
//using ELog.Core.Entities;
//using ELog.Core.Printer;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using ELog.HardwareConnectorFactory;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.WIP.CageLabelPrint
//{



//    [PMMSAuthorize]
//    public class CageLabelPrintingService : ApplicationService, ICageLabelPrintingService
//    {


//        private readonly IRepository<CubicleMaster> _cubicleRepository;
//        private readonly IRepository<CageLabelPrinting> _cageLabelPrintingRepository;
//        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
//        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
//        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialRepository;
//        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly PrinterFactory _printerFactory;
//        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private readonly IRepository<MaterialMaster> _materialMasterRepository;

//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }


//        public CageLabelPrintingService(IRepository<CubicleMaster> cubicleRepository,
//            IRepository<CageLabelPrinting> cageLabelPrintingRepository,
//            IRepository<DispensingDetail> dispensingDetailRepository,
//              IHttpContextAccessor httpContextAccessor,
//               IRepository<ProcessOrderAfterRelease> processOrderRepository,
//               IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
//               IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialRepository,
//               IMasterCommonRepository masterCommonRepository, PrinterFactory printerFactory,
//               Microsoft.Extensions.Configuration.IConfiguration configuration,
//              IRepository<DeviceMaster> deviceRepository, IRepository<MaterialMaster> materialMasterRepository
//           )

//        {

//            _httpContextAccessor = httpContextAccessor;
//            _cubicleRepository = cubicleRepository;
//            _cageLabelPrintingRepository = cageLabelPrintingRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _dispensingDetailRepository = dispensingDetailRepository;
//            _processOrderRepository = processOrderRepository;
//            _unitOfMeasurementRepository = unitOfMeasurementRepository;
//            _processOrderMaterialRepository = processOrderMaterialRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _deviceRepository = deviceRepository;
//            _configuration = configuration;
//            _printerFactory = printerFactory;
//            _materialMasterRepository = materialMasterRepository;

//        }

//        [PMMSAuthorize(Permissions = "CageLabelPrinting.Add")]
//        public async Task<CageLabelPrintingDto> CreateAsync(CageLabelPrintingDto input)
//        {


//            var cageLabelPrint = ObjectMapper.Map<CageLabelPrinting>(input);

//            var currentDate = DateTime.UtcNow;

//            cageLabelPrint.CageLabelBarcode = $"Cage-{currentDate:yy}-{_masterCommonRepository.GetNextCageLabelPrintBarcodeSequence():D10}";
//            if (cageLabelPrint.CageLabelBarcode == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//            }
//            //BarcodeGeneratorHelper.GetBarCode(printPacking.PackingLabelBarcode);

//            //if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            //{
//            //    var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            //    await PrintPackingLabelBarcode(device, printPacking);
//            //}
//            await _cageLabelPrintingRepository.InsertAsync(cageLabelPrint);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<CageLabelPrintingDto>(cageLabelPrint);
//        }

//        [PMMSAuthorize(Permissions = "CageLabelPrinting.Delete")]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {

//            var pallet = await _cageLabelPrintingRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _cageLabelPrintingRepository.DeleteAsync(pallet).ConfigureAwait(false);
//        }

//        [PMMSAuthorize(Permissions = "CageLabelPrinting.View")]
//        public async Task<CageLabelPrintingDto> GetAsync(EntityDto<int> input)
//        {

//            var entity = await _cageLabelPrintingRepository.GetAsync(input.Id);
//            var batchNos = from po in _processOrderRepository.GetAll()
//                           join material in _processOrderMaterialRepository.GetAll()
//                           on po.Id equals material.ProcessOrderId into areaps
//                           from material in areaps.DefaultIfEmpty()
//                           where po.Id == entity.ProcessorderID
//                           select new CageLabelPrintingDto
//                           {
//                               ProcessOrderNo = po.ProcessOrderNo,
//                               ProductCode = po.ProductCode,
//                               BatchNo = material.ProductBatchNo,


//                           };
//            var data = batchNos.FirstOrDefault();
//            CageLabelPrintingDto cageLabelPrintingDto = new CageLabelPrintingDto();
//            cageLabelPrintingDto.ProcessOrderNo = data.ProcessOrderNo;
//            cageLabelPrintingDto.ProductCode = data.ProductCode;
//            cageLabelPrintingDto.BatchNo = data.BatchNo;
//            cageLabelPrintingDto.DispensingBarcode = entity.DispensingBarcode;
//            cageLabelPrintingDto.ProcessorderID = entity.ProcessorderID;
//            cageLabelPrintingDto.CubcileCode = entity.CubcileCode;
//            cageLabelPrintingDto.Id = entity.Id;
//            cageLabelPrintingDto.CubicleID = entity.CubicleID;
//            cageLabelPrintingDto.NoOfContainer = entity.NoOfContainer;
//            cageLabelPrintingDto.CageLabelBarcode = entity.CageLabelBarcode;
//            cageLabelPrintingDto.PrinterID = entity.PrinterID;
//            return ObjectMapper.Map<CageLabelPrintingDto>(cageLabelPrintingDto);
//        }

//        [PMMSAuthorize(Permissions = "CageLabelPrinting.Add")]
//        public async Task<PagedResultDto<CageLabelPrintingListDto>> GetListAsync()
//        {

//            var query = from cage in _cageLabelPrintingRepository.GetAll()
//                        join po in _processOrderRepository.GetAll()
//                        on cage.ProcessorderID equals po.Id into pos
//                        from order in pos.DefaultIfEmpty()
//                        join materialmom in _materialMasterRepository.GetAll()
//                        on cage.ProductID equals materialmom.Id into materialmoms
//                        from materialmom in materialmoms.DefaultIfEmpty()
//                        from poMaterial in _processOrderMaterialRepository.GetAll()
//                         .Where(x => x.ProcessOrderId == order.Id).Take(1)
//                        select new CageLabelPrintingListDto
//                        {

//                            NoOfContainer = cage.NoOfContainer,
//                            CubcileCode = cage.CubcileCode,
//                            CubicleID = cage.CubicleID,
//                            CageLabelBarcode = cage.CageLabelBarcode,
//                            BatchNo = poMaterial.ProductBatchNo,
//                            ProcessorderID = cage.ProcessorderID,
//                            ProcessOrderNo = order.ProcessOrderNo,
//                            ProductCode = order.ProductCode,
//                            ProductName = materialmom.MaterialDescription,
//                            ProductID = order.ProductCodeId,


//                        };

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<CageLabelPrintingListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = "CageLabelPrinting.View")]
//        public async Task<PagedResultDto<CageLabelPrintingListDto>> GetAllAsync(PagedCageLabelPrintResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<CageLabelPrintingListDto>(
//                totalCount,
//                entities
//            );
//        }

//        public async Task<CageLabelPrintingDto> UpdateAsync(CageLabelPrintingDto input)
//        {
//            var activity = await _cageLabelPrintingRepository.GetAsync(input.Id);

//            ObjectMapper.Map(input, activity);

//            await _cageLabelPrintingRepository.UpdateAsync(activity);

//            return await GetAsync(input);
//        }



//        protected IQueryable<CageLabelPrintingListDto> ApplySorting(IQueryable<CageLabelPrintingListDto> query, PagedCageLabelPrintResultRequestDto input)
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
//        protected IQueryable<CageLabelPrintingListDto> ApplyPaging(IQueryable<CageLabelPrintingListDto> query, PagedCageLabelPrintResultRequestDto input)
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
//        protected IQueryable<CageLabelPrintingListDto> CreateUserListFilteredQuery(PagedCageLabelPrintResultRequestDto input)
//        {


//            var cageLabelQuery = from cageLabel in _cageLabelPrintingRepository.GetAll()
//                                 join dispensingDetail in _dispensingDetailRepository.GetAll()
//                                 on cageLabel.DispensingId equals dispensingDetail.Id
//                                 join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
//                                 on dispensingDetail.UnitOfMeasurementId equals unitOfMeasurement.Id

//                                 from processOrderMaterial in _processOrderMaterialRepository.GetAll()
//                                 .Where(x => x.ProcessOrderId == cageLabel.ProcessorderID).Take(1)

//                                     //join processOrder in _processOrderRepository.GetAll()
//                                     //on processOrderMaterial.ProcessOrderId equals processOrder.Id
//                                     // where processOrderMaterial.ProcessOrderId== cageLabel.ProcessorderID

//                                 select new CageLabelPrintingListDto
//                                 {
//                                     Id = cageLabel.Id,
//                                     ProductCode = cageLabel.ProductCode,
//                                     SAPBatchNo = dispensingDetail.SAPBatchNumber,
//                                     UOM = unitOfMeasurement.Name,
//                                     ARNo = processOrderMaterial.ARNO,
//                                     ProductID = cageLabel.ProductID,
//                                     ProcessorderID = cageLabel.ProcessorderID,
//                                     CubicleID = cageLabel.CubicleID,
//                                     CageLabelBarcode = cageLabel.CageLabelBarcode,
//                                 };

//            if (input.ProductID != null)
//            {
//                cageLabelQuery = cageLabelQuery.Where(x => x.ProductID == input.ProductID);
//            }
//            if (input.ProcessorderID != null)
//            {
//                cageLabelQuery = cageLabelQuery.Where(x => x.ProcessorderID == input.ProcessorderID);
//            }
//            if (input.CubicleID != null)
//            {
//                cageLabelQuery = cageLabelQuery.Where(x => x.CubicleID == input.CubicleID);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {

//                cageLabelQuery = cageLabelQuery.Where(x => x.SAPBatchNo.Contains(input.Keyword) || x.ProductCode.Contains(input.Keyword) || x.ARNo.Contains(input.Keyword));

//            }
//            return cageLabelQuery;
//        }

//        public async Task<List<CageLabelPrintingDto>> GetAllDispensedCode(string input)
//        {
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {

//                var dispensingBarcodeQuery = from dispensingDetail in _dispensingDetailRepository.GetAll()
//                                             where dispensingDetail.DispenseBarcode.ToLower() == input.ToLower()
//                                             select new CageLabelPrintingDto
//                                             {
//                                                 Id = dispensingDetail.Id,
//                                                 DispensingBarcode = dispensingDetail.DispenseBarcode,

//                                             };

//                return await dispensingBarcodeQuery.ToListAsync() ?? default;
//            }
//            return default;
//        }


//        public async Task<List<string>> GetAllDispCode()
//        {


//            var dispensingBarcodeQuery = (from dispensingDetail in _cageLabelPrintingRepository.GetAll()
//                                          select dispensingDetail.DispensingBarcode).Distinct();

//            return await dispensingBarcodeQuery.ToListAsync() ?? default;

//        }
//        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
//        {

//            var productCodes = from po in _processOrderRepository.GetAll()
//                               select new SelectListDtoWithPlantId
//                               {
//                                   Id = po.ProductCodeId,
//                                   PlantId = po.PlantId,
//                                   Value = po.ProductCode,
//                               };

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
//                var processOrders = await _processOrderRepository.GetAll().Select(po => new SelectListDto
//                {
//                    Id = po.Id,
//                    Value = po.ProcessOrderNo,
//                }).ToListAsync();
//                return processOrders;
//            }

//            return default;
//        }
//        public async Task<CageLabelPrintingDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
//        {
//            var batchNos = from po in _processOrderRepository.GetAll()
//                           join material in _processOrderMaterialRepository.GetAll()
//                           on po.Id equals material.ProcessOrderId into areaps
//                           from material in areaps.DefaultIfEmpty()
//                           where po.Id == input
//                           select new CageLabelPrintingDto
//                           {
//                               ProcessOrderNo = po.ProcessOrderNo,
//                               ProductCode = po.ProductCode,
//                               BatchNo = material.ProductBatchNo,
//                               ProductName = material.MaterialDescription
//                           };
//            var data = batchNos.FirstOrDefault();
//            CageLabelPrintingDto cageLabelPrintingDto = new CageLabelPrintingDto();
//            cageLabelPrintingDto.ProcessOrderNo = data.ProcessOrderNo;
//            cageLabelPrintingDto.ProductCode = data.ProductCode;
//            cageLabelPrintingDto.BatchNo = data.BatchNo;
//            cageLabelPrintingDto.ProductName = data.ProductName;


//            return cageLabelPrintingDto;
//        }
//        public async Task Print(CageLabelPrintingDto cageLabelPrint)
//        {

//            BarcodeGeneratorHelper.GetBarCode(cageLabelPrint.CageLabelBarcode);

//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(cageLabelPrint.PrinterID));
//                await PrintPackingLabelBarcode(device, cageLabelPrint);
//            }
//        }

//        private async Task PrintPackingLabelBarcode(DeviceMaster device, CageLabelPrintingDto cageLabelprint)
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


