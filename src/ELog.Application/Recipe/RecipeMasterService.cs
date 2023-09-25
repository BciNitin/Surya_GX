using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.Recipe.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.Recipe
{
    [PMMSAuthorize]
    public class RecipeMasterService : ApplicationService, IRecipeMasterService
    {
        private readonly IRepository<RecipeTransactionHeader> _recipeRepository;
        private readonly IRepository<RecipeTransactionDetails> _recipedetailRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<MaterialMaster> _materialRepository;
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<CubicalRecipeTranDetlMapping> _cubicleRepository;
        private readonly IRepository<CompRecipeTransDetlMapping> _compRepository;
        private readonly IRepository<ApprovalLevelMaster> _approvalLevelMaster;
        private readonly IRepository<RecipeWiseProcessOrderMapping> _recipeWisePOMappingRepository;
        private readonly IRepository<ProcessOrderAfterRelease> _poAfterReleaseRepository;
        private readonly IRepository<MaterialMaster> _materiaMasterRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeMasterService(IRepository<RecipeTransactionHeader> recipeRepository, IRepository<MaterialMaster> materialRepository, IRepository<RecipeTransactionDetails> recipedetailRepository, IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
            IRepository<StandardWeightMaster> standardWeightRepository, IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
            IRepository<LocationMaster> locationRepository, IRepository<CubicalRecipeTranDetlMapping> cubicleRepository,
            IMasterCommonRepository masterCommonRepository, IRepository<CompRecipeTransDetlMapping> compRepository, IRepository<ApprovalLevelMaster> approvalLevelMaster, IHttpContextAccessor httpContextAccessor,
            IRepository<RecipeWiseProcessOrderMapping> recipeWisePOMappingRepository,
            IRepository<ProcessOrderAfterRelease> poAfterReleaseRepository, IRepository<MaterialMaster> materiaMasterRepository
            )

        {
            _recipeRepository = recipeRepository;
            _recipedetailRepository = recipedetailRepository;
            _plantRepository = plantRepository;
            _materialRepository = materialRepository;
            _standardWeightRepository = standardWeightRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _cubicleRepository = cubicleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _compRepository = compRepository;
            _httpContextAccessor = httpContextAccessor;
            _approvalLevelMaster = approvalLevelMaster;
            _recipeWisePOMappingRepository = recipeWisePOMappingRepository;
            _poAfterReleaseRepository = poAfterReleaseRepository;
            _materiaMasterRepository = materiaMasterRepository;

        }

        //[PMMSAuthorize(Permissions = "RecipeMaster.Add")]
        //public async Task<RecipeMasterDto> CreateAsync(RecipeMasterDto input)
        //{
        //    var recipe = ObjectMapper.Map<RecipeMaster>(input);
        //    recipe.TenantId = AbpSession.TenantId;
        //    var currentDate = DateTime.UtcNow;
        //    //recipe.AreaCode = $"A{currentDate.Month:D2}{currentDate:yy}{ _masterCommonRepository.GetNextUOMSequence():D4}";
        //    await _recipeRepository.InsertAsync(recipe);

        //    CurrentUnitOfWork.SaveChanges();
        //    return ObjectMapper.Map<RecipeMasterDto>(recipe);
        //}




        //public Task<RecipeMasterDto> GetAsync(EntityDto<int> input)
        //{
        //    throw new NotImplementedException();
        //}

        [PMMSAuthorize(Permissions = "RecipeMaster.View")]
        public async Task<PagedResultDto<RecipeMasterListDto>> GetAllAsync(PagedRecipeMasterResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<RecipeMasterListDto>(
                totalCount,
                entities
            );
        }


        public async Task<List<SelectListDto>> GetMaterialAsync(string input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materials = from po in _materialRepository.GetAll()
                            select new SelectListDto
                            {
                                Id = po.Id,
                                // PlantId = po.PlantId,
                                Value = po.MaterialCode,
                            };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                materials = materials.Where(x => x.Value.Contains(input)).Distinct();
                return await materials?.ToListAsync() ?? default;
            }
            return await materials.ToListAsync() ?? default;
        }

        [PMMSAuthorize(Permissions = "RecipeApproval.View")]
        public async Task<PagedResultDto<RecipeMasterListDto>> GetAllRecipeApprovalAsync(PagedRecipeMasterResultRequestDto input)
        {
            var query = CreateRecipeAppListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<RecipeMasterListDto>(
                totalCount,
                entities
            );
        }


        //[PMMSAuthorize(Permissions = "RecipeMaster.View")]
        //[PMMSAuthorize(Permissions = "RecipeApproval.View")]
        public async Task<List<CreayeRecipeMasterdetail>> GetRecipeDetailsByHdrIdAsync(EntityDto<int> input)
        {
            var recipeHdrId = _recipeRepository.GetAll().Where(x => x.ProductId == input.Id).FirstOrDefault();
            var productCode = "";
            int productId;


            var productname = await _materiaMasterRepository.GetAsync(input.Id);
            productCode = productname.MaterialCode;
            productId = productname.Id;

            if (recipeHdrId == null)
            {
                return null;
            }
            var recipeCubicalData = from recipeDtls in _recipedetailRepository.GetAll()
                                    where recipeDtls.RecipeTransactionHeaderId == recipeHdrId.Id
                                    join recipeCube in _cubicleRepository.GetAll()
                                     on recipeDtls.Id equals recipeCube.RecipeTransactiondetailId into areaps
                                    from recipeCube in areaps.DefaultIfEmpty()
                                    join recipeComp in _compRepository.GetAll()
                                    on recipeDtls.Id equals recipeComp.RecipeTransactiondetailId into compsarea
                                    from recipeComp in compsarea.DefaultIfEmpty()
                                        //where recipeCube.IsDeleted == false
                                    select new RecipeTransactionDetails
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
                                        IsActive = recipeDtls.IsActive,
                                        CubicalRecipeTranDetlMapping = recipeDtls.CubicalRecipeTranDetlMapping,
                                        CompRecipeTransDetlMapping = recipeDtls.CompRecipeTransDetlMapping,
                                        // RecipeTransactionHeader = recipeHdrId,
                                        DocumentVersion = recipeHdrId.DocumentVersion,
                                        RecipeNo = recipeHdrId.RecipeNo,
                                        MaterialDescription = productname.MaterialDescription,


                                    };

            //recipeCubicalData.Where(x => x.RecipeTransactionHeaderId == recipeHdrId);
            //var entity = _recipedetailRepository.GetAll().Where(x => x.RecipeTransactionHeaderId == recipeHdrId && x.IsDeleted == false);
            //List<CubicalRecipeTranDetlMapping> cubicalRecipeTrans = new List<CubicalRecipeTranDetlMapping>();
            //List<RecipeTransactionDetails> recipeTransactions = new List<RecipeTransactionDetails>();
            //foreach (var recipedtls in entity)
            //{
            //    cubicalRecipeTrans = await _cubicleRepository.GetAll().Where(x => x.RecipeTransactiondetailId == recipedtls.Id && x.IsDeleted == false).ToListAsync();

            //}

            //foreach (var recip in entity)
            //{
            //    recip.CubicalRecipeTranDetlMapping = cubicalRecipeTrans.Where(x => x.RecipeTransactiondetailId == recip.Id).ToList();
            //    recipeTransactions.Add(recip);

            //}
            try
            {

                var data = ObjectMapper.Map<List<CreayeRecipeMasterdetail>>(recipeCubicalData);
                data[0].ProductName = productCode;
                data[0].ProductId = productId;
                data[0].ApprovalRemarks = recipeHdrId.ApprovalRemarks;
                return data;
            }
            catch
            {
                return null;
            }

        }

        //[PMMSAuthorize(Permissions = "RecipeMaster.View")]
        //[PMMSAuthorize(Permissions = "RecipeApproval.View")]
        public async Task<CreateRecipeMasterDto> GetRecipeHeaderByHdrIdAsync(EntityDto<int> input)
        {

            // var entity = _recipeRepository.GetAll().Where(x => x.ProductId == input.Id && x.IsDeleted == false).FirstOrDefault();
            //List<RecipeTransactionDetails> personViews = ObjectMapper.Map<List<RecipeTransactionDetails>, List<CreayeRecipeMasterdetail>>(entity);
            //List<RecipeTransactionDetails> personViews = ObjectMapper.Map<List<RecipeTransactionDetails>>(entity);

            var entity = (from rec in _recipeRepository.GetAll()
                          join mat in _materialRepository.GetAll()
                          on rec.ProductId equals mat.Id
                          where rec.ProductId == input.Id && rec.IsDeleted == false
                          select new CreateRecipeMasterDto
                          {
                              ProductId = rec.ProductId,
                              DocumentVersion = rec.DocumentVersion,
                              RecipeNo = rec.RecipeNo,
                              MaterialDescription = mat.MaterialDescription,
                              ProductCode = mat.MaterialCode
                          }
                          ).FirstOrDefault();
            return ObjectMapper.Map<CreateRecipeMasterDto>(entity);
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        public async Task<CreateRecipeMasterDto> GetRecipeHeaderByProductCodeAsync(string input)
        {

            var isdigit = IsDigitsOnly(input);


            var entity = (from Recipe in _recipeRepository.GetAll()//.Where(x => x.ApprovalStatus != null)
                                                                   //join po in _poAfterReleaseRepository.GetAll()
                                                                   //on Recipe.ProductId equals po.ProductCodeId into pomaterials
                                                                   //from po in pomaterials.DefaultIfEmpty()
                          join mm in _materiaMasterRepository.GetAll()
                          on Recipe.ProductId equals mm.Id
                          where
                          // Recipe.ApprovalStatus == "Approved" &&
                          Recipe.IsDeleted == false && (Recipe.ProductId.ToString() == input)
                          select new CreateRecipeMasterDto
                          {
                              ProductCode = mm.MaterialCode,
                              RecipeNo = Recipe.RecipeNo,
                              ApprovalRemarks = Recipe.ApprovalRemarks,
                              ApprovalStatus = Recipe.ApprovalStatus,
                              ApprovedById = Recipe.ApprovedById,
                              ApprovedDate = Recipe.ApprovedDate,
                              ApprovedLevelId = Recipe.ApprovedLevelId,
                              DocumentVersion = Recipe.DocumentVersion,
                              IsActive = Recipe.IsActive,
                              ProductId = Recipe.ProductId,
                              MaterialDescription = mm.MaterialDescription

                          }
                         );

            var entities = await AsyncQueryableExecuter.FirstOrDefaultAsync(entity);
            return ObjectMapper.Map<CreateRecipeMasterDto>(entities);
        }


        [PMMSAuthorize(Permissions = "RecipeMaster.Edit")]
        public async Task<string> UpdateAsync(CreateRecipeMasterDto input)
        {

            //var recipeId = await InsertOrUpdateRecipeHeader(input);
            var dictInsertedOrUpdatedRecipe = await InsertOrUpdateRecipeDetails(input);
            //var dictInsertedOrUpdatedRecipe = await InsertOrUpdateRecipeDetails(recipeId, input.RecipeTransactionDetails);
            List<CreayeRecipeMasterdetail> recipedtails = input.RecipeTransactionDetails;
            //await InsertOrUpdateOperationDetails(input);
            //Audit Events
            return "success";

        }


        public async Task<int> CheckRecipeIdIfAlreadyExist(int input)
        {
            return await _recipeRepository.GetAll().Where(x => x.ProductId == input && (x.ApprovalStatus == "In progress" || x.ApprovalStatus == "Approved")).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> GetRecipeIdIfAlreadyExist(CreateRecipeMasterDto input)
        {
            var recipeNo = input.RecipeNo;
            return await _recipeRepository.GetAll().Where(x => x.RecipeNo == recipeNo).Select(x => x.Id).FirstOrDefaultAsync();
        }


        //private async Task<int> InsertOrUpdateRecipeHeader(CreateRecipeMasterDto input)
        //{
        //    int recipeId = await GetRecipeIdIfAlreadyExist(input);
        //    if (recipeId > 0)
        //    {
        //        var recipeheader = await _recipeRepository.GetAsync(recipeId);
        //        ObjectMapper.Map(input, recipeheader);
        //        //purchaseOrder.PlantId = plantId;
        //        await _recipeRepository.UpdateAsync(recipeheader);
        //    }
        //    else
        //    {
        //        var recipeheader = ObjectMapper.Map<RecipeTransactionHeader>(input);
        //        //purchaseOrder.PlantId = plantId;
        //        var insertedRecipe = await _recipeRepository.InsertAsync(recipeheader);
        //        CurrentUnitOfWork.SaveChanges();
        //        recipeId = insertedRecipe.Id;
        //    }
        //    return recipeId;
        //}

        private async Task<Dictionary<int, int>> InsertOrUpdateRecipeDetails(CreateRecipeMasterDto input)
        {
            List<CreayeRecipeMasterdetail> lstRecipeDetals = input.RecipeTransactionDetails;
            int recipehdrId = await GetRecipeIdIfAlreadyExist(input);

            Dictionary<int, int> dictInsertedOrUpdatedrecipeId = new Dictionary<int, int>();
            var lstoperation = lstRecipeDetals.Select(x => x.Operation).Distinct();
            var existingEntitiesRecipeDtls = await _recipedetailRepository.GetAll().Where(x => lstoperation.Contains(x.Operation) && x.RecipeTransactionHeaderId == recipehdrId).ToListAsync();
            var lstNewOperation = lstoperation.Except(existingEntitiesRecipeDtls.Select(x => x.Operation));
            if (recipehdrId > 0)
            {
                var recipeheader = await _recipeRepository.GetAsync(recipehdrId);
                recipeheader.RecipeTransactionDetails = existingEntitiesRecipeDtls;
                ObjectMapper.Map(input, recipeheader);
                recipeheader.ApprovalStatus = "Submitted";
                await _recipeRepository.UpdateAsync(recipeheader);
                CurrentUnitOfWork.SaveChanges();

            }
            else
            {

                foreach (var newopt in lstNewOperation)
                {
                    var recipeDetailsDtoToInsert = lstRecipeDetals.Last(x => x.Operation == newopt);
                    input.RecipeTransactionDetails.Add(recipeDetailsDtoToInsert);
                }
                var recipeheader = ObjectMapper.Map<RecipeTransactionHeader>(input);
                recipeheader.ApprovalStatus = "Submitted";
                var insertedRecipe = await _recipeRepository.InsertAsync(recipeheader);
                CurrentUnitOfWork.SaveChanges();
                recipehdrId = insertedRecipe.Id;
            }


            return dictInsertedOrUpdatedrecipeId;
        }



        private async Task<Dictionary<int, int>> InsertOrUpdateOperationDetails(CreateRecipeMasterDto input)
        {
            Dictionary<int, int> dictInsertedOrUpdatedrecipeId = new Dictionary<int, int>();
            List<CreayeRecipeMasterdetail> creayeRecipes = input.RecipeTransactionDetails;
            List<CreateCubicalRecipeTranDetlMapping> CubicalRecipes = new List<CreateCubicalRecipeTranDetlMapping>();
            //List<CreateCubicalRecipeTranDetlMapping> CubicalRecipesInsert = new List<CreateCubicalRecipeTranDetlMapping>();
            CreateCubicalRecipeTranDetlMapping cubicalRecipeTran = new CreateCubicalRecipeTranDetlMapping();

            foreach (var ops in creayeRecipes)
            {
                CubicalRecipes = ops.CreateCubicalRecipeTranDetlMapping;
                foreach (var cube in CubicalRecipes)
                {
                    var result = _recipedetailRepository.GetAll().Where(x => x.Operation.ToLower() == cube.Operation.ToLower() && x.IsDeleted == false).SingleOrDefault();
                    cube.RecipeTransactiondetailId = result.Id;
                    //CubicalRecipesInsert.Add(cube);
                    cubicalRecipeTran = cube;
                    //ops.CreateCubicalRecipeTranDetlMapping.Add(cube);

                    int recipeOpsrId = await GetRecipeOpsIdIfAlreadyExist(cubicalRecipeTran);
                    if (recipeOpsrId > 0)
                    {
                        var cubicalOps = ObjectMapper.Map<CubicalRecipeTranDetlMapping>(cubicalRecipeTran);
                        cubicalOps.Id = recipeOpsrId;
                        await _cubicleRepository.UpdateAsync(cubicalOps);
                        CurrentUnitOfWork.SaveChanges();
                    }
                    else
                    {
                        var cubicalOps = ObjectMapper.Map<CubicalRecipeTranDetlMapping>(cubicalRecipeTran);
                        await _cubicleRepository.InsertAsync(cubicalOps);
                        CurrentUnitOfWork.SaveChanges();
                    }


                }


            }

            return dictInsertedOrUpdatedrecipeId;
        }

        private async Task<int> GetRecipeOpsIdIfAlreadyExist(CreateCubicalRecipeTranDetlMapping input)
        {

            return await _cubicleRepository.GetAll().Where(x => x.Operation == input.Operation && x.CubicalId == input.CubicalId && x.RecipeTransactiondetailId == input.RecipeTransactiondetailId && x.IsDeleted == false).Select(x => x.Id).FirstOrDefaultAsync();
            //return await _recipeRepository.GetAll().Where(x => x.RecipeNo.Trim().ToLower() == recipeNo).Select(x => x.Id).FirstOrDefaultAsync();
        }

        protected IQueryable<RecipeMasterListDto> ApplySorting(IQueryable<RecipeMasterListDto> query, PagedRecipeMasterResultRequestDto input)
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
        protected IQueryable<RecipeMasterListDto> ApplyPaging(IQueryable<RecipeMasterListDto> query, PagedRecipeMasterResultRequestDto input)
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
        protected IQueryable<RecipeMasterListDto> CreateUserListFilteredQuery(PagedRecipeMasterResultRequestDto input)
        {
            var RecipeQuery = from Recipe in _recipeRepository.GetAll()//.Where(x => x.ApprovalStatus != null)
                              join mm in _materialRepository.GetAll()
                             on Recipe.ProductId equals mm.Id into pomaterials
                              from po in pomaterials.DefaultIfEmpty()
                              where
                              !string.IsNullOrEmpty(Recipe.ApprovalStatus) &&
                              // (Recipe.ApprovalStatus == "Approved" || Recipe.ApprovalStatus == "Rejected") &&
                              Recipe.IsDeleted == false
                              select new RecipeMasterListDto
                              {
                                  Id = Recipe.Id,
                                  RecipeNo = Recipe.RecipeNo,
                                  ProductId = Recipe.ProductId,
                                  ProductCode = po.MaterialCode,
                                  ProductName = Recipe.ProductId.ToString(),
                                  ProductNo = Recipe.ProductId.ToString(),
                                  DocVersion = Recipe.DocumentVersion,
                                  ApprovalStatus = Recipe.ApprovalStatus,

                              };
            if (!(string.IsNullOrEmpty(input.ProductCode) || string.IsNullOrWhiteSpace(input.ProductCode)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductCode.Equals(input.ProductCode));
            }

            if (!(string.IsNullOrEmpty(input.DocumentVersion) || string.IsNullOrWhiteSpace(input.DocumentVersion)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.DocVersion.Equals(input.DocumentVersion));
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductCode.Contains(input.Keyword) || x.DocVersion.Contains(input.Keyword) || x.RecipeNo.Contains(input.Keyword));
            }

            return RecipeQuery;
        }

        protected IQueryable<RecipeMasterListDto> CreateRecipeAppListFilteredQuery(PagedRecipeMasterResultRequestDto input)
        {
            int? levelId = 0;
            if (!(string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["UserApprovalLevelId"].FirstOrDefault()) ||
                string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.Request.Headers["UserApprovalLevelId"].FirstOrDefault())))
            {
                levelId = int.Parse(_httpContextAccessor.HttpContext.Request.Headers["UserApprovalLevelId"].FirstOrDefault());
                levelId = levelId - 1;
            }

            //&& x.ApprovedLevelId < levelId
            var RecipeQuery = from Recipe in _recipeRepository.GetAll().Where(x => (x.ApprovalStatus == "Submitted" || x.ApprovalStatus == "In progress") && x.IsDeleted == false)
                              join mm in _materialRepository.GetAll()
                              on Recipe.ProductId equals mm.Id into pomaterials
                              from po in pomaterials.DefaultIfEmpty()
                                  // where Recipe.ApprovalStatus == "Approved" && Recipe.IsDeleted == false && Recipe.ProductId == input.ProductId
                                  // where Recipe.IsDeleted == false
                              select new RecipeMasterListDto
                              {
                                  Id = Recipe.Id,
                                  RecipeNo = Recipe.RecipeNo,
                                  ProductId = Recipe.ProductId,
                                  ProductCode = po.MaterialCode,
                                  ProductName = Recipe.ProductId.ToString(),
                                  ProductNo = Recipe.ProductId.ToString(),
                                  DocVersion = Recipe.DocumentVersion,
                                  ApprovedLevelId = Recipe.ApprovedLevelId

                              };
            if (levelId == 0)
            {
                RecipeQuery = RecipeQuery.Where(x => x.ApprovedLevelId == null || x.ApprovedLevelId == 0);
            }
            else
            {
                RecipeQuery = RecipeQuery.Where(x => x.ApprovedLevelId.Equals(levelId));
            }
            if (!(string.IsNullOrEmpty(input.ProductCode) || string.IsNullOrWhiteSpace(input.ProductCode)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductCode.Equals(input.ProductCode));
            }
            if (!(string.IsNullOrEmpty(input.DocumentVersion) || string.IsNullOrWhiteSpace(input.DocumentVersion)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.DocVersion.Equals(input.DocumentVersion));
            }

            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                RecipeQuery = RecipeQuery.Where(x => x.ProductCode.Contains(input.Keyword) || x.DocVersion.Contains(input.Keyword) || x.RecipeNo.Contains(input.Keyword));
            }

            return RecipeQuery;
        }

        [PMMSAuthorize(Permissions = "RecipeApproval.Edit")]
        public async Task<string> ApproveRecipeMaster(CreateRecipeMasterDto input)
        {
            int logedinUserLevel = int.Parse(_httpContextAccessor.HttpContext.Request.Headers["UserApprovalLevelId"].FirstOrDefault());
            var maxLevel = _approvalLevelMaster.GetAll().Where(x => x.IsDeleted == false).Select(x => x.LevelCode).Max();
            string approval_status = "";
            if (logedinUserLevel < maxLevel)
            {
                approval_status = "In progress";
            }
            else if (logedinUserLevel == maxLevel)
            {
                approval_status = "Approved";
            }

            RecipeTransactionHeader recipeheader = new RecipeTransactionHeader();
            int recipeheaderid = await _recipeRepository.GetAll().Where(x => x.ProductId == input.ProductId && x.IsDeleted == false).Select(x => x.Id).FirstOrDefaultAsync();
            ObjectMapper.Map(input, recipeheader);
            recipeheader.Id = recipeheaderid;
            recipeheader.ApprovalStatus = approval_status;
            recipeheader.ApprovedById = (int?)AbpSession.UserId;
            recipeheader.ApprovedLevelId = logedinUserLevel;
            recipeheader.ApprovedDate = DateTime.Now;
            var a = await _recipeRepository.UpdateAsync(recipeheader);
            CurrentUnitOfWork.SaveChanges();
            return "success";
        }


        [PMMSAuthorize(Permissions = "RecipeApproval.Edit")]
        public async Task<string> RejectRecipeMaster(CreateRecipeMasterDto input)
        {
            RecipeTransactionHeader recipeheader = new RecipeTransactionHeader();
            int logedinUserLevel = int.Parse(_httpContextAccessor.HttpContext.Request.Headers["UserApprovalLevelId"].FirstOrDefault());
            int recipeheaderid = await _recipeRepository.GetAll().Where(x => x.ProductId == input.ProductId && x.IsDeleted == false).Select(x => x.Id).FirstOrDefaultAsync();
            ObjectMapper.Map(input, recipeheader);
            recipeheader.Id = recipeheaderid;
            recipeheader.ApprovalStatus = "Rejected";
            recipeheader.RejectedById = (int?)AbpSession.UserId;
            recipeheader.RejectedDate = DateTime.Now;
            recipeheader.ApprovedLevelId = logedinUserLevel;
            await _recipeRepository.UpdateAsync(recipeheader);
            CurrentUnitOfWork.SaveChanges();
            return "success";
        }
        //private async Task<CreateRecipeMasterDto> ApproveRecipeMaster(CreateRecipeMasterDto input)
        //{


        //    return input;


        //}
        [PMMSAuthorize(Permissions = "RecipeMaster.Delete")]
        public async Task DeleteAsync(int input)
        {
            int recipeheaderid = await _recipeRepository.GetAll().Where(x => x.ProductId == input && x.IsDeleted == false).Select(x => x.Id).FirstOrDefaultAsync();
            var approvalUserModule = await _recipeRepository.GetAsync(recipeheaderid).ConfigureAwait(false);
            await _recipeRepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }
    }
}
