//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.WIP.MaterialReturn.Dto;
//using ELog.Core;
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
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.WIP.MaterialReturn
//{
//    public class MaterialReturnService : ApplicationService, IMaterialReturnService
//    {
//        private const string formatNumber = "FDSTG/F/013,Version:01";
//        private const string sopNo = "FDSOP/GHT/ST/018";


//        private readonly IRepository<ELog.Core.Entities.MaterialReturn> _materialReturn;
//        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementMaster;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
//        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<DispensingDetail> _dispensingDetail;
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly PrinterFactory _printerFactory;
//        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
//        private readonly IWebHostEnvironment _environment;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<User, long> _userRepository;
//        private object input;

//        public MaterialReturnService(IRepository<ELog.Core.Entities.MaterialReturn> materialReturn,
//         IHttpContextAccessor httpContextAccessor,
//        IRepository<ELog.Core.Entities.InProcessLabelDetails> processLabelRepository,
//        IRepository<CubicleMaster> cubicleRepository, IMasterCommonRepository masterCommonRepository,
//        IRepository<ProcessOrderAfterRelease> processOrderRepository,
//  IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository, IRepository<DeviceMaster> deviceRepository, IRepository<DispensingDetail> dispensingDetail,
//        PrinterFactory printerFactory, Microsoft.Extensions.Configuration.IConfiguration configuration, IWebHostEnvironment environment,
//        IRepository<PlantMaster> plantRepository, IRepository<User, long> userRepository
//      )
//        {
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _processOrderRepository = processOrderRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _materialReturn = materialReturn;
//            _masterCommonRepository = masterCommonRepository;
//            _processOrdermaterialRepository = processOrdermaterialRepository;
//            _deviceRepository = deviceRepository;
//            _printerFactory = printerFactory;
//            _configuration = configuration;
//            _environment = environment;
//            _plantRepository = plantRepository;
//            _userRepository = userRepository;
//            _dispensingDetail = dispensingDetail;
//        }
//        public async Task<MaterialReturnDto> CreateAsync(CreateMaterialReturnDto input)
//        {
//            var materialreturn = ObjectMapper.Map<ELog.Core.Entities.MaterialReturn>(input);
//            var currentDate = DateTime.UtcNow;

//            if (await IsContainerBarcodePresent(input.ContainerId))
//            {
//                throw new Abp.UI.UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.DispContainerCodeAlreadyExist, input.ContainerId));
//            }

//            materialreturn.MaterialReturnProcessLabelBarcode = $"IPL-{currentDate:yy}-{_masterCommonRepository.GetNextGateEntryBarcodeSequence():D10}";
//            if (materialreturn.MaterialReturnProcessLabelBarcode == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//            }

//            await _materialReturn.InsertAsync(materialreturn);
//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<MaterialReturnDto>(materialreturn);
//        }

//        public async Task<bool> IsContainerBarcodePresent(int? ContainerId)
//        {

//            if (ContainerId != null)
//            {
//                return await _materialReturn.GetAll().AnyAsync(x => x.ContainerId == ContainerId);
//            }

//            return false;
//        }

//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            var approvalUserModule = await _materialReturn.GetAsync(input.Id).ConfigureAwait(false);
//            await _materialReturn.DeleteAsync(approvalUserModule).ConfigureAwait(false);
//        }

//        public async Task<PagedResultDto<MaterialReturnListDto>> GetAllAsync(PagedMaterialReturnRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<MaterialReturnListDto>(
//                totalCount,
//                entities
//            );
//        }

//        public async Task<MaterialReturnDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _materialReturn.GetAsync(input.Id);
//            return ObjectMapper.Map<MaterialReturnDto>(entity);
//        }

//        public async Task<MaterialReturnDto> UpdateAsync(MaterialReturnDto input)
//        {
//            var putaway = await _materialReturn.GetAsync(input.Id);
//            ObjectMapper.Map(input, putaway);
//            await _materialReturn.UpdateAsync(putaway);
//            return await GetAsync(input);
//        }

//        public async Task Print(MaterialReturnDto materialReturn)
//        {

//            BarcodeGeneratorHelper.GetBarCode(materialReturn.DocumentNo);

//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(materialReturn.PrinterId));
//                await PrintProcessLabelBarcode(device, materialReturn);
//            }
//        }

//        private async Task PrintProcessLabelBarcode(DeviceMaster device, MaterialReturnDto input)
//        {
//            var materialReturn = ObjectMapper.Map<MaterialReturnDto>(input);
//            //var device1 = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                //PrintBody = await GetMaterialReturnPrintBody(materialReturn)
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);
//        }

//        //private async Task<string> GetMaterialReturnPrintBody(MaterialReturnDto input)
//        //{
//        //    var printByUser = await _userRepository.GetAsync((int)AbpSession.UserId);
//        //    // var processlabelData = await _processLabelRepository.GetAsync((int)input.Id);
//        //    //var poExtraDetails =   (from pl in _processLabelRepository.GetAll()
//        //    //                            join po in _processOrderRepository.GetAll()
//        //    //                            on pl.ProcessOrderId equals po.Id
//        //    //                            join pom in _processOrdermaterialRepository.GetAll()
//        //    //                            on po.Id equals pom.ProcessOrderId into poms
//        //    //                            from pom in poms.DefaultIfEmpty()
//        //    //                            select new ProcessLabelDto
//        //    //                            {
//        //    //                                ProductCode = po.ProductCode,
//        //    //                                BatchNo = pom.ProductBatchNo,
//        //    //                                NoOfContainer = pl.NoOfContainer,
//        //    //                                ExperyDate = pom.ExpiryDate,
//        //    //                                MfgDate = pom.RetestDate,
//        //    //                            });
//        //    //var data = poExtraDetails.FirstOrDefault();

//        //    var poExtraDetails = await (from pl in _materialReturn.GetAll()
//        //                                join po in _processOrderRepository.GetAll()
//        //                                on pl.ProcessOrderId equals po.Id
//        //                                join plant in _plantRepository.GetAll()
//        //                                on po.PlantId equals plant.Id
//        //                                join pom in _processOrdermaterialRepository.GetAll()
//        //                                on po.Id equals pom.ProcessOrderId

//        //                                select new { plant.PlantId, plant.PlantName, pom.ProductBatchNo, pom.MaterialCode, po.ProductCode, pl.NoOfContainer }).FirstOrDefaultAsync();
//        //    var processlabelprnFilePath = $"{_environment.WebRootPath}\\wip_prn\\inprocessslabel_prn\\InProcessLabel.prn";
//        //    var processlabelPRNFile = File.ReadAllText(processlabelprnFilePath);
//        //    var args = new Dictionary<string, string>(
//        //    StringComparer.OrdinalIgnoreCase) {
//        //    {"{ProductCode}", poExtraDetails.ProductCode},
//        //    {"{MGFDate}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//        //    {"{EXPDate}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//        //    {"{FormatNo}",formatNumber },
//        //    {"{SOPNo}",sopNo},
//        //    {"{BatchNo}",poExtraDetails.ProductBatchNo },
//        //    {"{ContainerNo}",poExtraDetails.NoOfContainer },
//        //   // {"{LRNo}",invoiceData.LRNo},
//        //   // {"{LRDate}",invoiceData.LRDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//        //             // {"{PlantCode}",poExtraDetails.PlantId},
//        //    {"{Print By /Date}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//        //   // {"{SerialNo}",input.GatePassNo },
//        //   // {"{Barcode}",$"{input.GatePassNo}"},
//        //};
//        //    var newstr = args.Aggregate(processlabelPRNFile, (current, value) => current.Replace(value.Key, value.Value));
//        //    return newstr;
//        //}

//        protected IQueryable<MaterialReturnListDto> CreateUserListFilteredQuery(PagedMaterialReturnRequestDto input)
//        {

//            var ApprovalusermodulemappingQuery = from put in _materialReturn.GetAll()
//                                                 join dis in _dispensingDetail.GetAll()
//                                                 on put.ContainerId equals dis.Id
//                                                 select new MaterialReturnListDto
//                                                 {
//                                                     Id = put.Id,
//                                                     DocumentNo = put.DocumentNo,
//                                                     ProductId = put.ProductId,
//                                                     ProductNo = put.ProductNo,
//                                                     BatchNo = put.BatchNo,
//                                                     ContainerId = put.ContainerId,
//                                                     ScanBalanceNo = dis.DispenseBarcode,
//                                                     Quantity = put.Quantity,
//                                                     MaterialReturnProcessLabelBarcode = put.MaterialReturnProcessLabelBarcode,
//                                                     IsPrint = put.IsPrint,

//                                                 };
//            if (input.ProductNo != null)
//            {
//                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ProductNo == input.ProductNo);
//            }

//            if (input.DocumentNo != null)
//            {
//                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.DocumentNo == input.DocumentNo);
//            }

//            if (input.BatchNo != null)
//            {
//                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.BatchNo == input.BatchNo);
//            }


//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x =>
//                x.ProductNo.Contains(input.Keyword)
//                || x.DocumentNo.Contains(input.Keyword)
//                || x.BatchNo.Contains(input.Keyword)
//                || x.MaterialReturnProcessLabelBarcode.Contains(input.Keyword)
//                );
//            }

//            return ApprovalusermodulemappingQuery;
//        }

//        protected IQueryable<MaterialReturnListDto> ApplySorting(IQueryable<MaterialReturnListDto> query, PagedMaterialReturnRequestDto input)
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
//        protected IQueryable<MaterialReturnListDto> ApplyPaging(IQueryable<MaterialReturnListDto> query, PagedMaterialReturnRequestDto input)
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
//    }
//}
