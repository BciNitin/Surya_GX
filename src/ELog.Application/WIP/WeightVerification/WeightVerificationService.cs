using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.WeightVerification.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WeightVerification
{
    [PMMSAuthorize]
    public class WeightVerificationService : ApplicationService, IWeightVerificationService
    {
        private readonly IRepository<WeightVerificationHeader> _weightVerificationHeader;
        //private readonly IRepository<WeightVerificationDetail> _weightVerificationDetail;
        private readonly IRepository<DispensingDetail> _dispensingDetail;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderAfterRelease;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialAfterRelease;
        private readonly IRepository<CubicleMaster> _cubicleMaster;
        private readonly IRepository<ProcessOrder> _ProcessOrders;
        private readonly IRepository<DispensingHeader> _dispensingHeaders;

        private readonly IRepository<UnitOfMeasurementMaster> _uomMaster;
        private readonly IRepository<MaterialMaster> _materialMaster;




        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeightVerificationService(IRepository<PurchaseOrder> purchaseOrderRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<WeightVerificationHeader> weightVerificationHeader,
            //IRepository<WeightVerificationDetail> weightVerificationDetail,
            IRepository<ProcessOrderAfterRelease> processOrderAfterRelease,
            IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialAfterRelease,
            IRepository<DispensingDetail> dispensingDetail,
            IRepository<CubicleMaster> cubicleMaster,
            IRepository<DispensingHeader> dispensingHeaders,
            IRepository<ProcessOrder> processOrders,
            IRepository<UnitOfMeasurementMaster> uomMaster,
            IRepository<MaterialMaster> materialmaster)

        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _weightVerificationHeader = weightVerificationHeader;
            //_weightVerificationDetail = weightVerificationDetail;
            _dispensingDetail = dispensingDetail;
            _processOrderAfterRelease = processOrderAfterRelease;
            _processOrderMaterialAfterRelease = processOrderMaterialAfterRelease;
            _cubicleMaster = cubicleMaster;
            _dispensingHeaders = dispensingHeaders;
            _ProcessOrders = processOrders;
            _uomMaster = uomMaster;
            _materialMaster = materialmaster;


        }

        public async Task<List<SelectListDto>> GetDispenseBarcode(string input)
        {
            return await _dispensingDetail.GetAll().Where(x => x.DispenseBarcode.ToLower() == input.ToLower())
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.DispenseBarcode })?
                        .ToListAsync() ?? default;
        }

        public async Task<WeightVerificationDto> GetDispenseDetailsrAsync(int input)
        {

            var processOrders = await _dispensingDetail.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();

            var UOM = (from uom in _uomMaster.GetAll()
                       where uom.Id == processOrders.UnitOfMeasurementId
                       select uom).FirstOrDefault();
            var MM = (from d in _dispensingHeaders.GetAll()
                      join mm in _materialMaster.GetAll()
                      on d.MaterialCodeId equals mm.MaterialCode
                      select mm).FirstOrDefault();



            WeightVerificationDto weightVerificationDto = new WeightVerificationDto();
            weightVerificationDto.GrossWeight = processOrders.GrossWeight;
            weightVerificationDto.NetWeight = processOrders.NetWeight;
            weightVerificationDto.TareWeight = processOrders.TareWeight;
            weightVerificationDto.IsGrossWeight = processOrders.IsGrossWeight;
            weightVerificationDto.NoOfPacks = processOrders.NoOfPacks;
            weightVerificationDto.NoOfContainers = processOrders.NoOfContainers;
            weightVerificationDto.UOM = UOM.Name;
            weightVerificationDto.MatCode = MM.MaterialCode;
            weightVerificationDto.MatDesc = MM.MaterialDescription;

            return weightVerificationDto;
        }

        public async Task<WeightVerificationDto> GetWeightVerificationfromDispenseIdAsync(int input)
        {

            var processOrders = await _weightVerificationHeader.GetAll()

                .Where(x => x.DispensedId == input).Take(1).FirstOrDefaultAsync();
            WeightVerificationDto weightVerificationDto = new WeightVerificationDto();
            weightVerificationDto.GrossWeight = processOrders.GrossWeight;
            weightVerificationDto.NetWeight = processOrders.NetWeight;
            weightVerificationDto.TareWeight = processOrders.TareWeight;
            weightVerificationDto.IsGrossWeight = processOrders.IsGrossWeight;
            weightVerificationDto.NoOfPacks = processOrders.NoOfPacks;
            weightVerificationDto.NoOfContainers = processOrders.NoOfContainers;
            weightVerificationDto.ProcessOrderId = processOrders.ProcessOrderId;
            weightVerificationDto.DispensedId = processOrders.DispensedId;
            weightVerificationDto.ProcessOrderId = processOrders.ProcessOrderId;
            return weightVerificationDto;
        }

        public async Task<SelectListDto> GetDispenseBarcodefromIdAsync(int input)
        {

            var disp = await (from dp in _dispensingDetail.GetAll()
                              where dp.Id == input
                              select new SelectListDto
                              {
                                  Id = dp.Id,
                                  Value = dp.DispenseBarcode
                              }).FirstOrDefaultAsync();
            return disp;

        }

        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
        {
            if ((input != null) || (input == null))
            {
                var processOrders = await (from p in _processOrderAfterRelease.GetAll()
                                           join m in _materialMaster.GetAll()
                                           on p.ProductCodeId equals m.Id
                                           where m.MaterialCode == input.Trim()
                                           select new SelectListDto
                                           {
                                               Id = p.Id,
                                               Value = p.ProcessOrderNo,
                                           }).ToListAsync();


                //var processOrders = await _processOrderAfterRelease.GetAll().Select(po => new SelectListDto
                //{
                //    Id = po.Id,
                //    Value = po.ProcessOrderNo,
                //}).ToListAsync();
                return processOrders;
            }

            return default;
        }

        public async Task<WeightVerificationDto> GetBatchDetailsOfProcessOrderAsync(int input)
        {
            if ((input != null) || (input == null))
            {
                var processOrders = await _processOrderMaterialAfterRelease.GetAll().Where(x => x.ProcessOrderId == input).FirstOrDefaultAsync();
                WeightVerificationDto weightVerificationDto = new WeightVerificationDto();
                weightVerificationDto.Batchno = processOrders.SAPBatchNo;
                weightVerificationDto.Lotno = processOrders.LotNo;
                weightVerificationDto.ProductName = processOrders.MaterialDescription;

                return weightVerificationDto;

            }

            return default;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _processOrderAfterRelease.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   //   Id = po.Id,
                                   PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }

        [PMMSAuthorize(Permissions = "WeightVerification.Add")]
        public async Task<WeightVerificationDto> CreateAsync(CreateWeightVerificationDto input)
        {
            //var processorderDBC = from DD in _dispensingDetail.GetAll()
            //                      join DH in _dispensingHeaders.GetAll()
            //                      on DD.DispensingHeaderId equals DH.Id into Disp
            //                      from di in Disp.DefaultIfEmpty()
            //                      join po in _ProcessOrders.GetAll()
            //                      on di.ProcessOrderId equals po.Id into pod

            var DispensedProcessid = _dispensingHeaders.GetAll().Where(l => _dispensingDetail.GetAll().Any(s => (l.Id == s.DispensingHeaderId && s.Id == input.DispensedId))).Select(d => d.ProcessOrderId).FirstOrDefault();

            var DispensedProcessNo = _ProcessOrders.GetAll().Where(p => p.Id == DispensedProcessid).Select(s => s.ProcessOrderNo).FirstOrDefault();

            var ProductProcessNo = _processOrderAfterRelease.GetAll().Where(p => p.ProductCode == input.ProductCode && p.Id == input.ProcessOrderId).Select(s => s.ProcessOrderNo).FirstOrDefault();
            var weightCapture = ObjectMapper.Map<WeightVerificationHeader>(input);
            if (DispensedProcessNo == ProductProcessNo)
            {


                //weightCapture.TenantId = AbpSession.TenantId;
                await _weightVerificationHeader.InsertAsync(weightCapture);
                CurrentUnitOfWork.SaveChanges();

                return ObjectMapper.Map<WeightVerificationDto>(weightCapture);
            }
            else
            {
                return ObjectMapper.Map<WeightVerificationDto>(weightCapture);

            }
        }

        public async Task<PagedResultDto<WeightVerificationListDto>> GetAllAsync(PagedWeightVrifyReturnRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WeightVerificationListDto>(
                totalCount,
                entities
            );
        }

        public async Task<WeightVerificationDto> UpdateAsync(WeightVerificationDto input)
        {

            var DispensedProcessid = _dispensingHeaders.GetAll().Where(l => _dispensingDetail.GetAll().Any(s => (l.Id == s.DispensingHeaderId && s.Id == input.DispensedId))).Select(d => d.ProcessOrderId).FirstOrDefault();

            var DispensedProcessNo = _ProcessOrders.GetAll().Where(p => p.Id == DispensedProcessid).Select(s => s.ProcessOrderNo).FirstOrDefault();

            var ProductProcessNo = _processOrderAfterRelease.GetAll().Where(p => p.ProductCode == input.ProductCode && p.Id == input.ProcessOrderId).Select(s => s.ProcessOrderNo).FirstOrDefault();
            if (DispensedProcessNo == ProductProcessNo)
            {
                input.IsSuccess = true;
                var putaway = await _weightVerificationHeader.GetAsync(input.Id);
                ObjectMapper.Map(input, putaway);
                await _weightVerificationHeader.UpdateAsync(putaway);
                return await GetAsync(input);

            }
            else
            {
                input.IsSuccess = false;

                return await GetAsync(input);
            }
        }

        public async Task<WeightVerificationDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _weightVerificationHeader.GetAsync(input.Id);
            var processOrders = await _processOrderMaterialAfterRelease.GetAll().Where(x => x.ProcessOrderId == entity.ProcessOrderId).FirstOrDefaultAsync();
            WeightVerificationDto weightVerificationDto = new WeightVerificationDto();

            ObjectMapper.Map(entity, weightVerificationDto);
            weightVerificationDto.Batchno = processOrders.ProductBatchNo;
            weightVerificationDto.Lotno = processOrders.LotNo;
            return weightVerificationDto;
        }

        protected IQueryable<WeightVerificationListDto> CreateUserListFilteredQuery(PagedWeightVrifyReturnRequestDto input)
        {

            var ApprovalusermodulemappingQuery = from we in _weightVerificationHeader.GetAll()
                                                 join po in _processOrderAfterRelease.GetAll()
                                                 on we.ProcessOrderId equals po.Id into pos
                                                 from po in pos.DefaultIfEmpty()
                                                 join pom in _processOrderMaterialAfterRelease.GetAll()
                                                 on po.Id equals pom.ProcessOrderId into poms
                                                 from pom in poms.DefaultIfEmpty()
                                                 join ds in _dispensingDetail.GetAll()
                                                 on we.DispensedId equals ds.Id into dos
                                                 from ds in dos.DefaultIfEmpty()
                                                 join cm in _cubicleMaster.GetAll()
                                                 on we.CubicalId equals cm.Id into cos
                                                 from cm in cos.DefaultIfEmpty()
                                                 select new WeightVerificationListDto
                                                 {
                                                     Id = we.Id,
                                                     ProductCode = we.ProductCode,
                                                     ScanBalanceNo = we.ScanBalanceNo,
                                                     ProcessOrderno = po.ProcessOrderNo,
                                                     Dispensedno = ds.DispenseBarcode,
                                                     Cubicleno = cm.CubicleCode,
                                                     Batchno = pom.SAPBatchNo,
                                                     MaterialCode = pom.MaterialCode,
                                                     ARNo = pom.ARNO,
                                                     UOM = pom.UOM
                                                 };
            if (input.ProductCode != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ProductCode == input.ProductCode);
            }
            if (input.ProcessOrderNo != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ProductCode == input.ProcessOrderNo);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x =>
                x.ScanBalanceNo.Contains(input.Keyword) || x.Cubicleno.Contains(input.Keyword) || x.ProductCode.Contains(input.Keyword) || x.ProcessOrderno.Contains(input.Keyword) || x.Dispensedno.Contains(input.Keyword));
            }

            return ApprovalusermodulemappingQuery;
        }

        protected IQueryable<WeightVerificationListDto> ApplySorting(IQueryable<WeightVerificationListDto> query, PagedWeightVrifyReturnRequestDto input)
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
        protected IQueryable<WeightVerificationListDto> ApplyPaging(IQueryable<WeightVerificationListDto> query, PagedWeightVrifyReturnRequestDto input)
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
    }
}
