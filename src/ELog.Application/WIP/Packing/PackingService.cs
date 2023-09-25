using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.Packing.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.WIP.Packing
{
    [PMMSAuthorize]
    public class PackingService : ApplicationService, IPackingService
    {
        private readonly IRepository<PackingMaster> _packingrepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
        private readonly IRepository<ELog.Core.Entities.InProcessLabelDetails> _processLabelRepository;
        private readonly IRepository<MaterialMaster> _materialmasterRepository;
        private readonly IRepository<LocationMaster> _locationmasterRepository;
        private readonly IRepository<RecipeWiseProcessOrderMapping> _recipewisepoRepository;



        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PackingService(IRepository<PackingMaster> packingrepository,
          IRepository<ProcessOrderAfterRelease> processOrderRepository,
           IHttpContextAccessor httpContextAccessor,
          IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository,
          IRepository<ELog.Core.Entities.InProcessLabelDetails> processLabelRepository,
          IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialRepository,
          IRepository<MaterialMaster> materialmasterRepository,
          IRepository<LocationMaster> locationmasterRepository,
          IRepository<RecipeWiseProcessOrderMapping> recipewisepoRepository)
        {
            _packingrepository = packingrepository;
            _processOrderRepository = processOrderRepository;
            _processOrdermaterialRepository = processOrdermaterialRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _processLabelRepository = processLabelRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _materialmasterRepository = materialmasterRepository;
            _locationmasterRepository = locationmasterRepository;
            _recipewisepoRepository = recipewisepoRepository;
        }

        [PMMSAuthorize(Permissions = "WIPPacking.Add")]
        public async Task<PackingDto> CreateAsync(CreatePackingDto input)
        {
            var packing = ObjectMapper.Map<PackingMaster>(input);
            await _packingrepository.InsertAsync(packing);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PackingDto>(packing);
        }

        [PMMSAuthorize(Permissions = "WIPPacking.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _packingrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _packingrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = "WIPPacking.View")]
        public async Task<PackingDto> GetAsync(EntityDto<int> input)
        {
            //var entity = await _packingrepository.GetAsync(input.Id);

            var packingdata = from pack in _packingrepository.GetAll()
                              join pro in _processOrderRepository.GetAll()
                              on pack.ProcessOrderId equals pro.Id into poms
                              from pro in poms.DefaultIfEmpty()
                              join lbl in _processLabelRepository.GetAll()
                              on pack.ContainerId equals lbl.Id into los
                              from lbl in los.DefaultIfEmpty()
                              where pack.Id == input.Id
                              from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                             .Where(x => x.ProcessOrderId == pack.ProcessOrderId).Take(1)
                              select new PackingDto
                              {
                                  Id = pack.Id,
                                  ProcessOrderId = pack.ProcessOrderId,
                                  ProcessOrderNo = pro.ProcessOrderNo,
                                  ProductCode = pack.ProductId,
                                  BatchNo = processOrderMaterial.ProductBatchNo,
                                  ContainerId = pack.ContainerId,
                                  ContainerCode = lbl.ProcessLabelBarcode,
                                  ContainerCount = pack.ContainerCount,
                                  Quantity = pack.Quantity,
                                  IsActive = pack.IsActive
                              };
            //packingdata.Cast<PackingDto>().ToArray()

            return await AsyncQueryableExecuter.FirstOrDefaultAsync(packingdata);
        }

        [PMMSAuthorize(Permissions = "WIPPacking.Edit")]
        public async Task<PackingDto> UpdateAsync(PackingDto input)
        {
            var packing = await _packingrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, packing);
            await _packingrepository.UpdateAsync(packing);
            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = "WIPPacking.View")]
        public async Task<PagedResultDto<PackingListDto>> GetAllAsync(PagedPackingResultRequestDto input)
        {
            var query = CreatePickingFilteredQuery(input);


            var totalCount = await AsyncQueryableExecuter.CountAsync(query);



            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PackingListDto>(
                totalCount,
                entities
            );
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProcessLabelCodeAsync(string input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var processlabel = from po in _processLabelRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   Id = po.Id,
                                   // PlantId = po.PlantId,
                                   Value = po.ProcessLabelBarcode,
                               };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                processlabel = processlabel.Where(x => x.Value.Contains(input)).Distinct();
                return await processlabel?.ToListAsync() ?? default;
            }
            return await processlabel.ToListAsync() ?? default;
        }

        public async Task<List<PackingListDto>> GetAllDetails(string input, int locId)
        {
            var Data = from po in _processLabelRepository.GetAll()
                       join pr in _processOrderRepository.GetAll()
                       on po.ProcessOrderId equals pr.Id
                       join mm in _materialmasterRepository.GetAll()
                       on po.ProductId equals mm.Id
                       join pom in _processOrderMaterialRepository.GetAll()
                       on pr.Id equals pom.ProcessOrderId
                       join loc in _locationmasterRepository.GetAll()
                       on locId equals loc.Id
                       where po.ProcessLabelBarcode.Contains(input.Trim())
                       select new PackingListDto
                       {
                           ProcessOrderNo = pr.ProcessOrderNo,
                           ProductCode = mm.MaterialCode,
                           ProductName = mm.MaterialDescription,
                           BatchNo = pom.ProductBatchNo,
                           StorageLocation = loc.StorageLocationType,
                           ProductCodeId = mm.Id,
                           ProcessOrderId = pr.Id,
                           NoOfContainer = po.NoOfContainer

                           //Id = po.Id,
                           // PlantId = po.PlantId,

                       };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                // input = input.Trim();
                // Data = Data.Where(x => x.Value.Contains(input)).Distinct();
                return await Data?.ToListAsync() ?? default;
            }

            return await Data.ToListAsync() ?? default;
        }

        public bool CheckDuplicate(string input)
        {
            bool isDuplicate = false;

            var IPLBarcode = from a in _packingrepository.GetAll()
                             join ipl in _processLabelRepository.GetAll()
                             on a.ContainerId equals ipl.Id
                             where input == ipl.ProcessLabelBarcode
                             select a;
            if (IPLBarcode.Any())
            {
                isDuplicate = true;
            }
            return isDuplicate;
        }

        public bool CheckProcessORderOfCOntainer(string input, int ProcessorderId)
        {
            bool isDuplicate = true;

            var IPLBarcode = _processLabelRepository.GetAll().Where(a => a.ProcessLabelBarcode == input.Trim() && a.ProcessOrderId == ProcessorderId).Count();

            if (IPLBarcode > 0)
            {
                isDuplicate = false;
            }
            return isDuplicate;
        }

        public async Task<List<PackingListDto>> GetLabelBCDetails(string input)
        {

            //var IPLBarcode = _processLabelRepository.GetAll().Where(a => a.ProcessLabelBarcode == input.Trim()).Count();

            var Data = from po in _processLabelRepository.GetAll()
                       join pr in _processOrderRepository.GetAll()
                       on po.ProcessOrderId equals pr.Id

                       join pom in _processOrderMaterialRepository.GetAll()
                       on pr.Id equals pom.ProcessOrderId

                       where po.ProcessLabelBarcode.Contains(input.Trim())
                       select new PackingListDto
                       {

                           NoOfContainer = po.NoOfContainer,
                           Qty = pom.Quantity

                           //Id = po.Id,
                           // PlantId = po.PlantId,

                       };

            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {

                return await Data?.ToListAsync() ?? default;
            }

            return await Data.ToListAsync() ?? default;

        }

        protected IQueryable<PackingListDto> CreatePickingFilteredQuery(PagedPackingResultRequestDto input)
        {
            var PackingQuery = from pac in _packingrepository.GetAll()
                               join po in _processOrderRepository.GetAll()
                               on pac.ProcessOrderId equals po.Id into pos
                               from po in pos.DefaultIfEmpty()
                               join poMaterial in _processOrdermaterialRepository.GetAll()
                               on po.Id equals poMaterial.ProcessOrderId into pomaterials
                               from poMaterial in pomaterials.DefaultIfEmpty()

                               select new PackingListDto
                               {
                                   Id = pac.Id,
                                   ProductCode = pac.ProductId,
                                   ProcessOrderId = po.Id,
                                   ProcessOrderNo = po.ProcessOrderNo,
                                   BatchNo = poMaterial.SAPBatchNo,
                                   ProductName = po.ProductCode,
                                   ContainerId = pac.ContainerId,
                                   ContainerCount = pac.ContainerCount,
                                   Quantity = pac.Quantity,
                                   IsActive = pac.IsActive

                               };
            if (input.ProcessOrderId != null)
            {
                PackingQuery = PackingQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }
            if (input.ProductId != null)
            {
                PackingQuery = PackingQuery.Where(x => x.ProductCode == input.ProductId);
            }


            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                PackingQuery = PackingQuery.Where(x =>
                x.ProductCode.Contains(input.Keyword));
            }

            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    PackingQuery = PackingQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    PackingQuery = PackingQuery.Where(x => x.IsActive);
                }
            }
            return PackingQuery;
        }

        protected IQueryable<PackingListDto> ApplySorting(IQueryable<PackingListDto> query, PagedPackingResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
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
        protected IQueryable<PackingListDto> ApplyPaging(IQueryable<PackingListDto> query, PagedPackingResultRequestDto input)
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
        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _processOrderRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   Id = po.Id,
                                   // PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }
        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
        {
            //if ((input != null) || (input == null))
            //{
            //    var processOrders = await _processOrderRepository.GetAll().Select(po => new SelectListDto
            //    {
            //        Id = po.Id,
            //        Value = po.ProcessOrderNo,
            //    }).ToListAsync();
            //    return processOrders;
            //}
            if ((input != null) || (input == null))
            {
                var processOrders = await (from p in _recipewisepoRepository.GetAll()
                                           join po in _processOrderRepository.GetAll()
                                           on p.ProcessOrderId equals po.Id
                                           where po.ProductCode == input
                                           select new SelectListDto
                                           {
                                               Id = p.ProcessOrderId,
                                               Value = po.ProcessOrderNo
                                           }).ToListAsync();


                return processOrders;

            }



            return default;
        }

        public async Task<List<PackingListDto>> GetProcessOrderDetailsAsync(int processOrderId)
        {
            var processOrder = await (from processorder in _processOrderRepository.GetAll()
                                      where processorder.Id == processOrderId
                                      orderby processorder.ProcessOrderNo
                                      select new PackingListDto
                                      {

                                          Id = processorder.Id,
                                          //BatchNo = processorder.SAPBatchNo,
                                      }).ToListAsync() ?? default;

            return processOrder;
        }

        public async Task<PackingDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
        {


            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

            var batchNos = from po in _processOrderRepository.GetAll()
                               //join lb in _processLabelRepository.GetAll()
                               //on po.ProcessOrderNo
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == input
                           select new PackingDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               BatchNo = material.SAPBatchNo,
                           };
            var data = batchNos.FirstOrDefault();
            PackingDto packingDto = new PackingDto();
            packingDto.ProcessOrderNo = data.ProcessOrderNo;
            packingDto.ProductCode = data.ProductCode;
            packingDto.BatchNo = data.BatchNo;


            return packingDto;
        }
    }
}
