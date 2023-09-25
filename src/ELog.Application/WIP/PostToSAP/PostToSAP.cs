using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.PostToSAP.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.PostToSAP
{
    [PMMSAuthorize]
    public class PostToSAP : ApplicationService, IPostToSAP
    {
        private readonly IRepository<PostWIPDataToSAP> _postToSAPrepository;
        private readonly IRepository<InProcessLabelDetails> _processLabelrepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostToSAP(IRepository<PostWIPDataToSAP> postToSAPrepository,
            IRepository<InProcessLabelDetails> processLabelrepository,
            IRepository<ProcessOrderAfterRelease> processOrderRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository, IRepository<MaterialMaster> materialMasterRepository)
        {
            _postToSAPrepository = postToSAPrepository;
            _processLabelrepository = processLabelrepository;
            _processOrderRepository = processOrderRepository;
            _processOrdermaterialRepository = processOrdermaterialRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _materialMasterRepository = materialMasterRepository;
        }
        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.Add")]
        public async Task<PostToSAPDto> CreateAsync(CreatePostToSAPDto input)
        {
            if (await IsProductCodePresent(input.ProcessOrderId))
            {
                throw new Abp.UI.UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.PostToSAPProductCodeAlreadyExist, input.ProcessOrderId));
            }
            var postToSAP = ObjectMapper.Map<PostWIPDataToSAP>(input);
            await _postToSAPrepository.InsertAsync(postToSAP);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PostToSAPDto>(postToSAP);
        }

        public async Task<bool> IsProductCodePresent(int? ProcessOrderId)
        {

            if (ProcessOrderId != null)
            {
                return await _postToSAPrepository.GetAll().AnyAsync(x => x.ProcessOrderId == ProcessOrderId);
            }

            return false;
        }
        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {

            var posttoSAPModule = await _postToSAPrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _postToSAPrepository.DeleteAsync(posttoSAPModule).ConfigureAwait(false);
        }


        //public async Task<PostToSAPDto> GetAsync(EntityDto<int> input)
        //{
        //    var entity = await _postToSAPrepository.GetAsync(input.Id);
        //    return ObjectMapper.Map<PostToSAPDto>(entity);
        //}
        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.View")]
        public async Task<PostToSAPDto> GetAsync(EntityDto<int> input)
        {

            var entity = await _postToSAPrepository.GetAsync(input.Id);
            var batchNos = from po in _processOrderRepository.GetAll()
                           join materialMaster in _materialMasterRepository.GetAll()
                           on po.ProductCodeId equals materialMaster.Id into masters
                           from materialMaster in masters.DefaultIfEmpty()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from materialpo in areaps.DefaultIfEmpty()
                           where po.Id == entity.ProcessOrderId
                           select new PostToSAPDto
                           {
                               ProcessOrderId = po.Id,
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = materialMaster.MaterialCode,
                               BatchNo = materialpo.SAPBatchNo,
                               ProductName = materialMaster.MaterialDescription,


                           };
            var data = batchNos.FirstOrDefault();
            PostToSAPDto postToSAPDto = new PostToSAPDto();
            postToSAPDto.ProcessOrderNo = data.ProcessOrderNo;
            postToSAPDto.ProductCode = data.ProductCode;
            postToSAPDto.BatchNo = data.BatchNo;
            postToSAPDto.ProductName = data.ProductName;
            postToSAPDto.ProcessOrderId = data.ProcessOrderId;
            postToSAPDto.Id = entity.Id;

            return ObjectMapper.Map<PostToSAPDto>(postToSAPDto);
        }

        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.Edit")]
        public async Task<PostToSAPDto> UpdateAsync(PostToSAPDto input)
        {
            var postToSAP = await _postToSAPrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, postToSAP);
            await _postToSAPrepository.UpdateAsync(postToSAP);
            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.View")]
        public async Task<PagedResultDto<PostToSAPListDto>> GetListAsync()
        {

            var query = from posttosap in _postToSAPrepository.GetAll()
                        join po in _processOrderRepository.GetAll()
                        on posttosap.ProcessOrderId equals po.Id into pos
                        from order in pos.DefaultIfEmpty()
                        join materialMaster in _materialMasterRepository.GetAll()
                        on posttosap.ProductId equals materialMaster.Id into masters
                        from materialMaster in masters.DefaultIfEmpty()
                        from poMaterial in _processOrdermaterialRepository.GetAll()
                         .Where(x => x.ProcessOrderId == order.Id).Take(1)
                        select new PostToSAPListDto
                        {

                            ProductId = (int)posttosap.ProductId,
                            ProcessOrderId = (int)posttosap.ProcessOrderId,
                            ProductCode = order.ProductCode,
                            ProductName = materialMaster.MaterialDescription,
                            BatchNo = poMaterial.ProductBatchNo,
                            ProcessOrderNo = order.ProcessOrderNo,



                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PostToSAPListDto>(
                totalCount,
                entities
            );
        }
        [PMMSAuthorize(Permissions = "PostWIPDataToSAP.View")]
        public async Task<PagedResultDto<PostToSAPListDto>> GetAllAsync(PagedPostToSAPResultRequestDto input)
        {
            var query = CreatePostToSapFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PostToSAPListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<PostToSAPListDto> CreatePostToSapFilteredQuery(PagedPostToSAPResultRequestDto input)
        {
            var PostToSapQuery = from pac in _postToSAPrepository.GetAll()
                                 join po in _processOrderRepository.GetAll()
                                 on pac.ProcessOrderId equals po.Id into pos
                                 from po in pos.DefaultIfEmpty()
                                 join materialMaster in _materialMasterRepository.GetAll()
                                 on po.ProductCodeId equals materialMaster.Id into masters
                                 from materialMaster in masters.DefaultIfEmpty()
                                 join pl in _processLabelrepository.GetAll()
                               on po.Id equals pl.ProcessOrderId into poss
                                 from pl in poss.DefaultIfEmpty()
                                 join poMaterial in _processOrdermaterialRepository.GetAll()
                                 on po.Id equals poMaterial.ProcessOrderId into pomaterials
                                 from poMaterial in pomaterials.DefaultIfEmpty()


                                 select new PostToSAPListDto
                                 {
                                     Id = pac.Id,
                                     ProductCode = po.ProductCode,
                                     ProductName = materialMaster.MaterialDescription,
                                     ProcessOrderId = po.Id,
                                     ProcessOrderNo = po.ProcessOrderNo,
                                     BatchNo = poMaterial.SAPBatchNo,
                                     IsSent = pac.IsSent,
                                     IsActive = pac.IsActive


                                 };


            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                PostToSapQuery = PostToSapQuery.Where(x =>
                x.ProductCode.Contains(input.Keyword));
            }

            return PostToSapQuery;
        }
        public async Task<PostToSAPDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
        {


            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

            var batchNos = from po in _processOrderRepository.GetAll()
                           join materialMaster in _materialMasterRepository.GetAll()
                           on po.ProductCodeId equals materialMaster.Id into masters
                           from materialMaster in masters.DefaultIfEmpty()
                           join material in _processOrdermaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == input
                           select new PostToSAPDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               ProductName = materialMaster.MaterialDescription,
                               BatchNo = material.SAPBatchNo,
                               ProcessOrderId = po.Id,
                           };
            var data = batchNos.FirstOrDefault();
            PostToSAPDto postToSAPDto = new PostToSAPDto();
            postToSAPDto.ProcessOrderId = data.ProcessOrderId;
            postToSAPDto.ProcessOrderNo = data.ProcessOrderNo;
            postToSAPDto.ProductCode = data.ProductCode;
            postToSAPDto.BatchNo = data.BatchNo;
            postToSAPDto.ProductName = data.ProductName;


            return postToSAPDto;
        }
        protected IQueryable<PostToSAPListDto> ApplySorting(IQueryable<PostToSAPListDto> query, PagedPostToSAPResultRequestDto input)
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
        protected IQueryable<PostToSAPListDto> ApplyPaging(IQueryable<PostToSAPListDto> query, PagedPostToSAPResultRequestDto input)
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
            if ((input != null) || (input == null))
            {
                var processOrders = await _processOrderRepository.GetAll().Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                }).ToListAsync();
                return processOrders;
            }

            return default;
        }

        public async Task<List<PostToSAPListDto>> GetProcessOrderDetailsAsync(int processOrderId)
        {
            var processOrder = await (from processorder in _processOrderRepository.GetAll()
                                      where processorder.Id == processOrderId
                                      orderby processorder.ProcessOrderNo
                                      select new PostToSAPListDto
                                      {

                                          Id = processorder.Id,
                                          //BatchNo = processorder.SAPBatchNo,
                                      }).ToListAsync() ?? default;

            return processOrder;
        }

    }
}
