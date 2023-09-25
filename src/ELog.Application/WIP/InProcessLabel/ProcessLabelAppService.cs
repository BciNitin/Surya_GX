//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.SelectLists.Dto;
//using ELog.Application.WIP.InProcessLabel.Dto;
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

//namespace ELog.Application.WIP.InProcessLabel
//{
//    [PMMSAuthorize]
//    public class ProcessLabelAppService : ApplicationService, IProcessLabelAppService
//    {
//        private const string formatNumber = "FDSTG/F/013,Version:01";
//        private const string sopNo = "FDSOP/GHT/ST/018";

//        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
//        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
//        private readonly IRepository<ELog.Core.Entities.InProcessLabelDetails> _processLabelRepository;
//        private readonly IRepository<CubicleMaster> _cubicleRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private readonly IRepository<MaterialMaster> _materialMasterRepository;


//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly PrinterFactory _printerFactory;
//        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
//        private readonly IWebHostEnvironment _environment;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<User, long> _userRepository;
//        private object input;

//        public ProcessLabelAppService(IRepository<ProcessOrderAfterRelease> processOrderRepository,
//        IHttpContextAccessor httpContextAccessor,
//        IRepository<ELog.Core.Entities.InProcessLabelDetails> processLabelRepository,
//        IRepository<CubicleMaster> cubicleRepository, IMasterCommonRepository masterCommonRepository,
//        IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository, IRepository<DeviceMaster> deviceRepository,
//        PrinterFactory printerFactory, Microsoft.Extensions.Configuration.IConfiguration configuration, IWebHostEnvironment environment,
//        IRepository<PlantMaster> plantRepository, IRepository<User, long> userRepository, IRepository<MaterialMaster> materialMasterRepository)
//        {
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _processOrderRepository = processOrderRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _processLabelRepository = processLabelRepository;
//            _cubicleRepository = cubicleRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _processOrdermaterialRepository = processOrdermaterialRepository;
//            _deviceRepository = deviceRepository;
//            _printerFactory = printerFactory;
//            _configuration = configuration;
//            _environment = environment;
//            _plantRepository = plantRepository;
//            _userRepository = userRepository;
//            _materialMasterRepository = materialMasterRepository;

//        }
//        [PMMSAuthorize(Permissions = "InProcessLabel.Add")]
//        public async Task<ProcessLabelDto> CreateAsync(CreateProcessLabelDto input)
//        {
//            var processLabel = ObjectMapper.Map<InProcessLabelDetails>(input);


//            var currentDate = DateTime.UtcNow;
//            //  ProcessLabel.TenantId = AbpSession.TenantId;
//            processLabel.ProcessLabelBarcode = $"IPL-{currentDate:yy}-{_masterCommonRepository.GetNextGateEntryBarcodeSequence():D10}";
//            if (processLabel.ProcessLabelBarcode == null)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GatePassNoCanNotNull);
//            }
//            //BarcodeGeneratorHelper.GetBarCode(processLabel.ProcessLabelBarcode);

//            //if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            //{
//            //    var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            //    await PrintProcessLabelBarcode(device, processLabel);
//            //}
//            await _processLabelRepository.InsertAsync(processLabel);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<ProcessLabelDto>(processLabel);
//        }

//        public async Task Print(ProcessLabelDto processLabel)
//        {

//            BarcodeGeneratorHelper.GetBarCode(processLabel.ProcessLabelBarcode);

//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(processLabel.PrinterId));
//                await PrintProcessLabelBarcode(device, processLabel);
//            }
//        }

//        private async Task PrintProcessLabelBarcode(DeviceMaster device, ProcessLabelDto processLabel)
//        {
//            var processLabel1 = ObjectMapper.Map<InProcessLabelDetails>(input);
//            //var device1 = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                PrintBody = await GetProcessLabelPrintBody(processLabel1)
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);
//        }

//        //[PMMSAuthorize(Permissions = "InProcessLabel.View")]
//        //public async Task PrintProcessLabelBarcode(CreateProcessLabelDto input)
//        //{
//        //    var processLabel = ObjectMapper.Map<InProcessLabelDetails>(input);
//        //    var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.PrinterId));
//        //    var printInput = new PrinterInput
//        //    {
//        //        IPAddress = device.IpAddress,
//        //        Port = Convert.ToInt32(device.Port),
//        //        PrintBody = await GetProcessLabelPrintBody(processLabel)
//        //    };
//        //    var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//        //    await prnPrinter.Print(printInput);
//        //}

//        [PMMSAuthorize(Permissions = "InProcessLabel.Edit")]
//        public async Task<ProcessLabelDto> UpdateAsync(ProcessLabelDto input)
//        {

//            var processLabel = await _processLabelRepository.GetAsync(input.Id);

//            ObjectMapper.Map(input, processLabel);

//            await _processLabelRepository.UpdateAsync(processLabel);

//            return await GetAsync(input);
//        }
//        [PMMSAuthorize(Permissions = "InProcessLabel.View")]
//        public async Task<ProcessLabelDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _processLabelRepository.GetAsync(input.Id);
//            var processlabelentity = ObjectMapper.Map<ProcessLabelDto>(entity);
//            return ObjectMapper.Map<ProcessLabelDto>(entity);
//        }

//        [PMMSAuthorize(Permissions = "InProcessLabel.View")]
//        public async Task<PagedResultDto<ProcessLabelListDto>> GetAllAsync(PagedProcessLabelResultRequestDto input)
//        {
//            //var groupClosedStatus = await GetStatusIdOfStatus(closedStatus);

//            var query = CreateProcessLabelListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);
//            var entities = await AsyncQueryableExecuter.ToListAsync(query);
//            //var groupEntities = ApplyGrouping(entities.ToList());

//            return new PagedResultDto<ProcessLabelListDto>(
//                totalCount,
//                entities
//            );
//        }
//        [PMMSAuthorize(Permissions = "InProcessLabel.Delete")]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            var inprocesslabeldetails = await _processLabelRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _processLabelRepository.DeleteAsync(inprocesslabeldetails).ConfigureAwait(false);
//        }
//        protected IQueryable<ProcessLabelListDto> CreateProcessLabelListFilteredQuery(PagedProcessLabelResultRequestDto input)
//        {
//            //var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

//            var processLabelQuery = from processlabel in _processLabelRepository.GetAll()
//                                    join cube in _cubicleRepository.GetAll()
//                                    on processlabel.CubicleId equals cube.Id into spos
//                                    from cube in spos.DefaultIfEmpty()
//                                    join material in _materialMasterRepository.GetAll()
//                                   on processlabel.ProductId equals material.Id into materials
//                                    from material in materials.DefaultIfEmpty()
//                                    join po in _processOrderRepository.GetAll()
//                                    on processlabel.ProcessOrderId equals po.Id into pos
//                                    from po in pos.DefaultIfEmpty()

//                                    from poMaterial in _processOrdermaterialRepository.GetAll()
//                                    //on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                                    //from poMaterial in pomaterials.DefaultIfEmpty()
//                                    .Where(x => x.ProcessOrderId == processlabel.ProcessOrderId).Take(1)
//                                    select new ProcessLabelListDto
//                                    {
//                                        Id = processlabel.Id,
//                                        CubicleId = cube.Id,
//                                        CubicleCode = cube.CubicleCode,
//                                        ProductCode = po.ProductCode,
//                                        ProductName = poMaterial.MaterialDescription,
//                                        ProcessOrderId = po.Id,
//                                        ProcessOrderNo = po.ProcessOrderNo,
//                                        BatchNo = poMaterial.ProductBatchNo,
//                                        CurrentStage = poMaterial.CurrentStage,
//                                        NextStage = poMaterial.NextStage,
//                                        LotNo = poMaterial.LotNo,
//                                        GrossWeight = processlabel.GrossWeight,
//                                        NetWeight = processlabel.NetWeight,
//                                        TareWeight = processlabel.TareWeight,
//                                        ProcessLabelBarcode = processlabel.ProcessLabelBarcode,
//                                        IsPrint = processlabel.IsPrint
//                                    };
//            if (input.ProcessOrderNo != null)
//            {
//                processLabelQuery = processLabelQuery.Where(x => x.ProcessOrderNo == input.ProcessOrderNo);
//            }
//            if (input.CubicleCode != null)
//            {
//                processLabelQuery = processLabelQuery.Where(x => x.CubicleCode == input.CubicleCode);
//            }
//            if (input.ProductCode != null)
//            {
//                processLabelQuery = processLabelQuery.Where(x => x.ProductCode == input.ProductCode);
//            }

//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                processLabelQuery = processLabelQuery.Where(x => x.ProcessOrderNo.Contains(input.Keyword) || x.ProductCode.Contains(input.Keyword)
//                || x.BatchNo.Contains(input.Keyword) || x.ProductName.Contains(input.Keyword)
//                || x.CubicleCode.Contains(input.Keyword) || x.LotNo.Contains(input.Keyword)
//                );
//            }
//            return processLabelQuery;
//        }



//        [PMMSAuthorize(Permissions = "InProcessLabel.View")]
//        public async Task<PagedResultDto<ProcessLabelListDto>> GetProductCodeAsync()
//        {
//            //var groupClosedStatus = await GetStatusIdOfStatus(closedStatus);

//            var processLabelQuery = from processlabel in _processLabelRepository.GetAll()
//                                    join cube in _cubicleRepository.GetAll()
//                                    on processlabel.CubicleId equals cube.Id into spos
//                                    from cube in spos.DefaultIfEmpty()
//                                    join material in _materialMasterRepository.GetAll()
//                                   on processlabel.ProductId equals material.Id into materials
//                                    from material in materials.DefaultIfEmpty()
//                                    join po in _processOrderRepository.GetAll()
//                                    on processlabel.ProcessOrderId equals po.Id into pos
//                                    from po in pos.DefaultIfEmpty()

//                                    from poMaterial in _processOrdermaterialRepository.GetAll()
//                                    //on po.Id equals poMaterial.ProcessOrderId into pomaterials
//                                    //from poMaterial in pomaterials.DefaultIfEmpty()
//                                    .Where(x => x.ProcessOrderId == processlabel.ProcessOrderId).Take(1)
//                                    select new ProcessLabelListDto
//                                    {

//                                        ProductCode = po.ProductCode,

//                                    };

//            var totalCount = await AsyncQueryableExecuter.CountAsync(processLabelQuery);


//            var entities = await AsyncQueryableExecuter.ToListAsync(processLabelQuery);
//            //var groupEntities = ApplyGrouping(entities.ToList());

//            return new PagedResultDto<ProcessLabelListDto>(
//                totalCount,
//                entities
//            );
//        }


//        public async Task<List<SelectListDto>> GetMaterialsOfProductCodeAsync(string input, int processOrderId)
//        {
//            var batchNos = from po in _processOrderRepository.GetAll()
//                           where po.Id == processOrderId
//                           select new { po.ProductCode, po.Id };
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                input = input.Trim();
//                batchNos = batchNos.Where(x => x.ProductCode.Contains(input)).Distinct();
//                return await batchNos.Select(x => new SelectListDto { Id = x.Id, Value = x.ProductCode }).ToListAsync()
//                ?? default;
//            }
//            return default;
//        }
//        protected IQueryable<ProcessLabelListDto> ApplySorting(IQueryable<ProcessLabelListDto> query, PagedProcessLabelResultRequestDto input)
//        {
//            //Try to sort query if available
//            ISortedResultRequest sortInput = input;
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
//        protected IQueryable<ProcessLabelListDto> ApplyPaging(IQueryable<ProcessLabelListDto> query, PagedProcessLabelResultRequestDto input)
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
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
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
//            dynamic val = await productCodes.ToListAsync() ?? default;
//            return val;
//        }
//        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
//        {
//            if ((input != null) || (input == null))
//            {
//                var processOrders = await _processOrderRepository.GetAll().Where(x => x.ProductCode == input).Select(po => new SelectListDto
//                {
//                    Id = po.Id,
//                    Value = po.ProcessOrderNo,
//                }).ToListAsync();
//                return processOrders;
//            }

//            return default;
//        }

//        public async Task<List<ProcessLabelDto>> GetProcessOrderDetailsAsync(int processOrderId)
//        {
//            var processOrder = await (from processorder in _processOrderRepository.GetAll()
//                                      where processorder.Id == processOrderId
//                                      orderby processorder.ProcessOrderNo
//                                      select new ProcessLabelDto
//                                      {

//                                          Id = processorder.Id,
//                                          //BatchNo = processorder.SAPBatchNo,
//                                          //CurrentStage = processorder.CurrentStage,
//                                          //NextStage = processorder.NextStage
//                                      }).ToListAsync() ?? default;

//            return processOrder;
//        }

//        public async Task<List<ProcessLabelDto>> GetBatchNosDetailsAsync(string input)
//        {
//            var batchNos = from po in _processOrderRepository.GetAll()
//                           join processlabel in _processLabelRepository.GetAll()
//                           on po.Id equals processlabel.ProcessOrderId
//                           where po.ProductCode == input.Trim()
//                           select new ProcessLabelDto
//                           {

//                               ProcessOrderId = po.Id,
//                               ProcessOrderNo = po.ProcessOrderNo,
//                               ProductCode = po.ProductCode,
//                               //BatchNo = po.SAPBatchNo,
//                               GrossWeight = processlabel.GrossWeight,
//                               NetWeight = processlabel.NetWeight,
//                               TareWeight = processlabel.TareWeight

//                           };
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                input = input.Trim();
//                batchNos = batchNos.Where(x => x.ProductCode.Contains(input));
//                return await batchNos.Select(processlabel => new ProcessLabelDto
//                {
//                    GrossWeight = processlabel.GrossWeight,
//                    NetWeight = processlabel.NetWeight,
//                    TareWeight = processlabel.TareWeight
//                })
//               .ToListAsync() ?? default;
//            }
//            return default;
//        }
//        public async Task<ProcessLabelDto> GetProcessDetailsbyProcessOrderAsync(int input)
//        {


//            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

//            var batchNos = from po in _processOrderRepository.GetAll()
//                           join material in _processOrdermaterialRepository.GetAll()
//                           on po.Id equals material.ProcessOrderId into areaps
//                           from material in areaps.DefaultIfEmpty()
//                           where po.Id == input
//                           select new ProcessLabelDto
//                           {
//                               ProcessOrderNo = po.ProcessOrderNo,
//                               ProductCode = po.ProductCode,
//                               ProductName = material.MaterialDescription,
//                               BatchNo = material.ProductBatchNo,
//                               CurrentStage = material.CurrentStage,
//                               NextStage = material.NextStage,
//                           };
//            var data = batchNos.FirstOrDefault();
//            ProcessLabelDto processLabelDto = new ProcessLabelDto();
//            processLabelDto.ProcessOrderNo = data.ProcessOrderNo;
//            processLabelDto.ProductCode = data.ProductCode;
//            processLabelDto.BatchNo = data.BatchNo;
//            processLabelDto.CurrentStage = data.CurrentStage;
//            processLabelDto.NextStage = data.NextStage;
//            processLabelDto.ProductName = data.ProductName;

//            return processLabelDto;
//        }

//        private async Task<string> GetProcessLabelPrintBody(InProcessLabelDetails input)
//        {
//            var printByUser = await _userRepository.GetAsync((int)AbpSession.UserId);
//            // var processlabelData = await _processLabelRepository.GetAsync((int)input.Id);
//            //var poExtraDetails =   (from pl in _processLabelRepository.GetAll()
//            //                            join po in _processOrderRepository.GetAll()
//            //                            on pl.ProcessOrderId equals po.Id
//            //                            join pom in _processOrdermaterialRepository.GetAll()
//            //                            on po.Id equals pom.ProcessOrderId into poms
//            //                            from pom in poms.DefaultIfEmpty()
//            //                            select new ProcessLabelDto
//            //                            {
//            //                                ProductCode = po.ProductCode,
//            //                                BatchNo = pom.ProductBatchNo,
//            //                                NoOfContainer = pl.NoOfContainer,
//            //                                ExperyDate = pom.ExpiryDate,
//            //                                MfgDate = pom.RetestDate,
//            //                            });
//            //var data = poExtraDetails.FirstOrDefault();

//            var poExtraDetails = await (from pl in _processLabelRepository.GetAll()
//                                        join po in _processOrderRepository.GetAll()
//                                        on pl.ProcessOrderId equals po.Id
//                                        join plant in _plantRepository.GetAll()
//                                        on po.PlantId equals plant.Id
//                                        join pom in _processOrdermaterialRepository.GetAll()
//                                        on po.Id equals pom.ProcessOrderId

//                                        select new { plant.PlantId, plant.PlantName, pom.ProductBatchNo, pom.MaterialCode, po.ProductCode, pl.NoOfContainer }).FirstOrDefaultAsync();
//            var processlabelprnFilePath = $"{_environment.WebRootPath}\\wip_prn\\inprocessslabel_prn\\InProcessLabel.prn";
//            var processlabelPRNFile = File.ReadAllText(processlabelprnFilePath);
//            var args = new Dictionary<string, string>(
//            StringComparer.OrdinalIgnoreCase) {
//            {"{Productname}", poExtraDetails.ProductCode},
//            {"{Mfgdate}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//            {"{Expdate}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)},
//            {"{FormatNo}",formatNumber },
//            {"{SOPNo}",sopNo},
//            {"{BatchNo}",poExtraDetails.ProductBatchNo },
//            {"{ContainerNo}",poExtraDetails.NoOfContainer },
//           // {"{LRNo}",invoiceData.LRNo},
//           // {"{LRDate}",invoiceData.LRDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//                     // {"{PlantCode}",poExtraDetails.PlantId},
//            {"{CheckedBy}", printByUser.UserName},
//            {"{Date}", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },

//           // {"{SerialNo}",input.GatePassNo },
//           // {"{Barcode}",$"{input.GatePassNo}"},
//        };
//            var newstr = args.Aggregate(processlabelPRNFile, (current, value) => current.Replace(value.Key, value.Value));
//            return newstr;
//        }

//    }
//}
