using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.RecipePOMapping.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.RecipePOMapping
{


    [PMMSAuthorize]
    public class RecipePOMappingService : ApplicationService, IRecipePOMappingService
    {
        private readonly IRepository<RecipeWiseProcessOrderMapping> _recipeWisePOMappingRepository;
        private readonly IRepository<ProcessOrderAfterRelease> _poAfterReleaseRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _poMaterialAfterReleaseRepository;
        private readonly IRepository<RecipeTransactionDetails> _recipetransactionRepository;
        private readonly IRepository<RecipeTransactionHeader> _recipetransactionHeaderRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region constructor

        public RecipePOMappingService(IRepository<RecipeWiseProcessOrderMapping> recipeWisePOMappingRepository,
            IRepository<ProcessOrderAfterRelease> poAfterReleaseRepository,
            IRepository<ProcessOrderMaterialAfterRelease> poMaterialAfterReleaseRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<RecipeTransactionDetails> recipetransactionRepository,
            IRepository<MaterialMaster> materialMasterRepository,
            IRepository<RecipeTransactionHeader> recipetransactionHeaderRepository
            )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _recipeWisePOMappingRepository = recipeWisePOMappingRepository;
            _poAfterReleaseRepository = poAfterReleaseRepository;
            _poMaterialAfterReleaseRepository = poMaterialAfterReleaseRepository;
            _recipetransactionRepository = recipetransactionRepository;
            _recipetransactionHeaderRepository = recipetransactionHeaderRepository;
            _materialMasterRepository = materialMasterRepository;


        }

        #endregion constructor


        [PMMSAuthorize(Permissions = "RecipeLinkToProcessOrder.Add")]
        public async Task<RecipeToPOLinkDto> CreateAsync(CreateRecipeToPOLinkDto input)
        {
            var recipewise = from a in _recipeWisePOMappingRepository.GetAll()
                             where a.ProcessOrderId == input.ProcessOrderId
                             select a;
            var recipeToPOLink = ObjectMapper.Map<RecipeWiseProcessOrderMapping>(input);
            if (recipewise.Count() == 0)
            {

                await _recipeWisePOMappingRepository.InsertAsync(recipeToPOLink);
                CurrentUnitOfWork.SaveChanges();
                //recipeToPOLink.IsExists = false;


            }
            else
            {
                // recipeToPOLink.IsExists = true;


            }
            return ObjectMapper.Map<RecipeToPOLinkDto>(recipeToPOLink);
        }


        public async Task<RecipeToPOLinkDto> GetRecipeAsync(EntityDto<int> input)
        {
            var entity = await _recipetransactionHeaderRepository.GetAsync(input.Id);
            return ObjectMapper.Map<RecipeToPOLinkDto>(entity);
        }
        [PMMSAuthorize(Permissions = "RecipeLinkToProcessOrder.View")]
        public async Task<CreateRecipeDetailsDto> GetRecipeHeaderByHdrIdAsync(string input)
        {
            var RecipeQuery = from Recipe in _recipetransactionHeaderRepository.GetAll() //.Where(x => (x.ApprovalStatus == "Approved") && x.IsDeleted == false)//
                                                                                         //join rwpo in _recipeWisePOMappingRepository.GetAll()
                                                                                         // on Recipe.Id equals rwpo.RecipeTransHdrId into ros
                                                                                         //from rwpo in ros.DefaultIfEmpty()
                              join po in _poAfterReleaseRepository.GetAll()
                              on Recipe.ProductId equals po.ProductCodeId into pomaterials
                              from po in pomaterials.DefaultIfEmpty()
                              join matrial in _materialMasterRepository.GetAll()
                              on po.ProductCode equals matrial.MaterialCode into materials
                              from matrial in materials.DefaultIfEmpty()
                              where Recipe.ApprovalStatus == "Approved" && Recipe.IsDeleted == false && po.ProductCode == input
                              select new CreateRecipeDetailsDto
                              {
                                  Id = Recipe.Id,
                                  RecipeNo = Recipe.RecipeNo,
                                  ProductId = Recipe.ProductId,
                                  ProductCode = po.ProductCode,
                                  ProductName = matrial.MaterialDescription,
                                  // ApprovalRemarks = rwpo.Remarks,
                                  ApprovalStatus = Recipe.ApprovalStatus,
                                  ApprovedById = Recipe.ApprovedById,
                                  ApprovedDate = Recipe.ApprovedDate,
                                  ApprovedLevelId = Recipe.ApprovedLevelId,
                                  DocumentVersion = Recipe.DocumentVersion,
                                  IsActive = Recipe.IsActive,
                              };
            return await AsyncQueryableExecuter.FirstOrDefaultAsync(RecipeQuery);
            //var entity =  _recipetransactionHeaderRepository.GetAll().Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault();
            //return ObjectMapper.Map<CreateRecipeMaster1Dto>(entity);
        }
        [PMMSAuthorize(Permissions = "RecipeLinkToProcessOrder.View")]
        public async Task<CreateRecipeDetailsDto> GetRecipeHeaderByHdrCodeIdAsync(int input)
        {
            var RecipeQuery = from Recipe in _recipetransactionHeaderRepository.GetAll() //.Where(x => (x.ApprovalStatus == "Approved") && x.IsDeleted == false)//
                                                                                         //join rwpo in _recipeWisePOMappingRepository.GetAll()
                                                                                         // on Recipe.Id equals rwpo.RecipeTransHdrId into ros
                                                                                         //from rwpo in ros.DefaultIfEmpty()
                              join po in _poAfterReleaseRepository.GetAll()
                              on Recipe.ProductId equals po.ProductCodeId into pomaterials
                              from po in pomaterials.DefaultIfEmpty()
                              join matrial in _materialMasterRepository.GetAll()
                              on po.ProductCode equals matrial.MaterialCode into materials
                              from matrial in materials.DefaultIfEmpty()
                              where Recipe.ApprovalStatus == "Approved" && Recipe.IsDeleted == false && Recipe.Id == input
                              select new CreateRecipeDetailsDto
                              {
                                  Id = Recipe.Id,
                                  RecipeNo = Recipe.RecipeNo,
                                  ProductId = Recipe.ProductId,
                                  ProductCode = po.ProductCode,
                                  ProductName = matrial.MaterialDescription,
                                  // ApprovalRemarks = rwpo.Remarks,
                                  ApprovalStatus = Recipe.ApprovalStatus,
                                  ApprovedById = Recipe.ApprovedById,
                                  ApprovedDate = Recipe.ApprovedDate,
                                  ApprovedLevelId = Recipe.ApprovedLevelId,
                                  DocumentVersion = Recipe.DocumentVersion,
                                  IsActive = Recipe.IsActive,
                              };
            return await AsyncQueryableExecuter.FirstOrDefaultAsync(RecipeQuery);
            //var entity =  _recipetransactionHeaderRepository.GetAll().Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault();
            //return ObjectMapper.Map<CreateRecipeMaster1Dto>(entity);
        }

        [PMMSAuthorize(Permissions = "RecipeLinkToProcessOrder.View")]
        public async Task<RecipeToPODto> GetProcessOrderNoAsync(EntityDto<int> input)
        {


            var entity = from rh in _recipetransactionHeaderRepository.GetAll() //.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault();
                                                                                //join rwpo in _recipeWisePOMappingRepository.GetAll()
                                                                                //on rh.Id equals rwpo.RecipeTransHdrId into ros
                                                                                //from rwpo in ros.DefaultIfEmpty()
                         join po in _poAfterReleaseRepository.GetAll()
                         on rh.ProductId equals po.ProductCodeId into pos
                         from po in pos.DefaultIfEmpty()
                         where input.Id == rh.Id
                         select new RecipeToPODto
                         {
                             RecipeNo = rh.RecipeNo,
                             ProcessOrderNo = po.ProcessOrderNo,
                             ProcessOrderId = po.Id,
                             DocumentVersion = rh.DocumentVersion,
                             RecipeTransHdrId = rh.Id

                         };
            return await AsyncQueryableExecuter.FirstOrDefaultAsync(entity);
        }

        public async Task<List<MaterialListDto>> GetMaterialByProcessOrderAsync(EntityDto<int> input)
        {
            var processOrdersMaterials = await _poMaterialAfterReleaseRepository.GetAll().Where(x => x.ProcessOrderId == input.Id).Select(po => new MaterialListDto
            {
                Id = po.Id,
                ProcessOrderId = po.ProcessOrderId,
                MaterialCode = po.MaterialCode,
                MaterialDescription = po.MaterialDescription,
                ARNO = po.ARNO,
                LotNo = po.LotNo,
                SAPBatchNo = po.SAPBatchNo,
                CurrentStage = po.CurrentStage,
                NextStage = po.NextStage,
                Quantity = po.Quantity,
                UOM = po.UOM,
            }).ToListAsync();

            return processOrdersMaterials;
        }

        public async Task<PagedResultDto<RecipePOMappingListDto>> GetAllRecipeAsync(PagedRecipeToPOLinkResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<RecipePOMappingListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<RecipePOMappingListDto> ApplySorting(IQueryable<RecipePOMappingListDto> query, PagedRecipeToPOLinkResultRequestDto input)
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
        protected IQueryable<RecipePOMappingListDto> ApplyPaging(IQueryable<RecipePOMappingListDto> query, PagedRecipeToPOLinkResultRequestDto input)
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

        protected IQueryable<RecipePOMappingListDto> CreateUserListFilteredQuery(PagedRecipeToPOLinkResultRequestDto input)
        {

            var RecipeQuery = from Recipe in _recipetransactionHeaderRepository.GetAll() //.Where(x => (x.ApprovalStatus == "Approved") && x.IsDeleted == false)//
                              join po in _poAfterReleaseRepository.GetAll()
                              on Recipe.ProductId equals po.ProductCodeId into pomaterials
                              from po in pomaterials.DefaultIfEmpty()
                              where Recipe.ApprovalStatus == "Approved" && Recipe.IsDeleted == false
                              select new RecipePOMappingListDto
                              {
                                  Id = Recipe.Id,
                                  RecipeNo = Recipe.RecipeNo,
                                  ProductId = Recipe.ProductId,
                                  ProductName = Recipe.ProductId.ToString(),
                                  ProductNo = Recipe.ProductId.ToString(),
                                  DocVersion = Recipe.DocumentVersion,
                                  RecipeId = Recipe.Id,
                                  ProductCode = po.ProductCode
                              };
            if (input.ProductCode != null)
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductCode == input.ProductCode);
            }
            if (input.RecipeNo != null)
            {
                RecipeQuery = RecipeQuery.Where(x => x.RecipeNo == input.RecipeNo);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductId.Equals(input.Keyword) || x.DocVersion.Contains(input.Keyword)
                || x.ProductCode.Contains(input.Keyword) || x.RecipeNo.Contains(input.Keyword));
            }

            return RecipeQuery;
        }

        //public Task<PagedResultDto<RecipeListDto>> GetAllRecipeAsync(PagedRecipeToPOLinkResultRequestDto input)
        //{
        //    throw new System.NotImplementedException();
        //}

        public async Task<List<RecipeListDto>> GetRecipeDetailsByHdrIdAsync(EntityDto<int> input)
        {
            //int recipeHdrId = await _recipetransactionHeaderRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.Id).FirstOrDefaultAsync();

            var recipeData = await _recipetransactionRepository.GetAll()
                             .Where(x => x.RecipeTransactionHeaderId == input.Id && x.IsDeleted == false)
                             .Select(recipeDtls => new RecipeListDto
                             {
                                 Id = recipeDtls.Id,
                                 Operation = recipeDtls.Operation,
                                 Stage = recipeDtls.Stage,
                                 NextOperation = recipeDtls.NextOperation,
                                 Component = recipeDtls.Component,
                                 IsWeightRequired = recipeDtls.IsWeightRequired,
                                 IsLebalPrintingRequired = recipeDtls.IsLebalPrintingRequired,
                                 IsVerificationReq = recipeDtls.IsVerificationReq,
                                 InProcessSamplingRequired = recipeDtls.InProcessSamplingRequired,
                                 IsSamplingReq = recipeDtls.IsSamplingReq,
                                 IsActive = recipeDtls.IsActive
                                 //CubicalRecipeTranDetlMapping = recipeDtls.CubicalRecipeTranDetlMapping
                             }).ToListAsync();



            return recipeData;
        }
        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _poAfterReleaseRepository.GetAll()
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
        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(int input)
        {
            if ((input != null) || (input == null))
            {
                var processOrders = await _poAfterReleaseRepository.GetAll().Where(x => x.ProductCodeId == input).Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                }).ToListAsync();
                return processOrders;
            }

            return default;
        }

        public async Task<List<RecipePOMappingListDto>> GetProcessOrderDetailsAsync(int processOrderId)
        {
            var processOrder = await (from processorder in _poAfterReleaseRepository.GetAll()
                                      where processorder.Id == processOrderId
                                      orderby processorder.ProcessOrderNo
                                      select new RecipePOMappingListDto
                                      {

                                          Id = processorder.Id,
                                          //BatchNo = processorder.SAPBatchNo,
                                      }).ToListAsync() ?? default;

            return processOrder;
        }
    }
}
