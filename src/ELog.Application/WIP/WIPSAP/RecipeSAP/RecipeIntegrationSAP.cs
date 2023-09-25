using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.Recipe.Dto;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPSAP.RecipeSAP
{
    public class RecipeIntegrationSAP : ApplicationService, IRecipeIntegrationSAP
    {
        private readonly IRepository<RecipeTransactionHeader> _recipeRepository;
        private readonly IRepository<RecipeTransactionDetails> _recipedetailRepository;
        private readonly IRepository<CubicalRecipeTranDetlMapping> _cubicleRepository;
        private readonly IRepository<CompRecipeTransDetlMapping> _compRepository;
        private readonly IRepository<ApprovalLevelMaster> _approvalLevelMaster;
        private readonly IRepository<MaterialMaster> _materialMaster;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeIntegrationSAP(IRepository<RecipeTransactionHeader> recipeRepository, IRepository<RecipeTransactionDetails> recipedetailRepository, IRepository<CubicalRecipeTranDetlMapping> cubicleRepository,
            IMasterCommonRepository masterCommonRepository, IRepository<CompRecipeTransDetlMapping> compRepository, IRepository<ApprovalLevelMaster> approvalLevelMaster, IHttpContextAccessor httpContextAccessor, IRepository<MaterialMaster> materialMaster)

        {
            _recipeRepository = recipeRepository;
            _recipedetailRepository = recipedetailRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _compRepository = compRepository;
            _httpContextAccessor = httpContextAccessor;
            _approvalLevelMaster = approvalLevelMaster;
            _materialMaster = materialMaster;
        }
        public async Task<string> InsertOrUpdateRecipeAsync(CreateRecipeMasterDto input)
        {
            var plantId = await GetProductCodeIfAlradyExist(input);
            var dictInsertedOrUpdatedRecipe = await InsertOrUpdateRecipeDetails(input, plantId);

            //var dictInsertedOrUpdatedRecipe = await InsertOrUpdateRecipeDetails(recipeId, input.RecipeTransactionDetails);
            List<CreayeRecipeMasterdetail> recipedtails = input.RecipeTransactionDetails;
            //await InsertOrUpdateOperationDetails(input);
            //Audit Events
            return "success";
        }

        private async Task<int> GetProductCodeIfAlradyExist(CreateRecipeMasterDto input)
        {
            var plantId = input.ProductId;

            return await _materialMaster.GetAll().Where(x => x.MaterialCode.ToLower() == input.ProductCode.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }
        private async Task<int> GetRecipeIdIfAlreadyExist(CreateRecipeMasterDto input)
        {
            var recipeNo = input.RecipeNo;
            return await _recipeRepository.GetAll().Where(x => x.RecipeNo == recipeNo).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> CheckMaterialExist(CreateRecipeMasterDto input)
        {

            return await _materialMaster.GetAll().Where(x => x.MaterialCode == input.ProductCode).Select(x => x.Id).FirstOrDefaultAsync();
        }
        private async Task<Dictionary<int, int>> InsertOrUpdateRecipeDetails(CreateRecipeMasterDto input, int productid)
        {
            List<CreayeRecipeMasterdetail> lstRecipeDetals = input.RecipeTransactionDetails;
            int recipehdrId = await GetRecipeIdIfAlreadyExist(input);
            int materialId = await CheckMaterialExist(input);
            Dictionary<int, int> dictInsertedOrUpdatedrecipeId = new Dictionary<int, int>();
            if (materialId > 0)
            {
                //var lstoperation = lstRecipeDetals.Select(x => x.Operation).Distinct();
                //var existingEntitiesRecipeDtls = await _recipedetailRepository.GetAll().Where(x => lstoperation.Contains(x.Operation) && x.RecipeTransactionHeaderId == recipehdrId).ToListAsync();
                //var lstNewOperation = lstoperation.Except(existingEntitiesRecipeDtls.Select(x => x.Operation));
                input.IsActive = true;
                foreach (var repDetails in input.RecipeTransactionDetails)
                {
                    if (repDetails.CreateCompRecipeTransDetlMapping != null)
                    {
                        repDetails.IsActive = true;
                        repDetails.CreateCompRecipeTransDetlMapping.RemoveAll(p => p.ComponentCode.ToUpper() == "DUMMY");
                        foreach (var components in repDetails.CreateCompRecipeTransDetlMapping)
                        {
                            if (components.ComponentCode != null)
                            {
                                int compId = await _materialMaster.GetAll().Where(x => x.MaterialCode.ToLower() == components.ComponentCode.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
                                components.ComponentId = compId;

                            }
                            components.IsActive = true;
                        }
                    }
                }

                if (recipehdrId > 0)
                {
                    var recipeheader = await _recipeRepository.GetAsync(recipehdrId);
                    //recipeheader.RecipeTransactionDetails = existingEntitiesRecipeDtls;

                    ObjectMapper.Map(input, recipeheader);
                    //recipeheader.ApprovalStatus = "";
                    recipeheader.ProductId = productid;
                    recipeheader.IsActive = true;


                    await _recipeRepository.UpdateAsync(recipeheader);
                    CurrentUnitOfWork.SaveChanges();

                }
                else
                {

                    //foreach (var newopt in lstNewOperation)
                    //{
                    //    var recipeDetailsDtoToInsert = lstRecipeDetals.Last(x => x.Operation == newopt);
                    //    input.RecipeTransactionDetails.Add(recipeDetailsDtoToInsert);
                    //}
                    var recipeheader = ObjectMapper.Map<RecipeTransactionHeader>(input);
                    //recipeheader.ApprovalStatus = "";
                    recipeheader.ProductId = productid;
                    var insertedRecipe = await _recipeRepository.InsertAsync(recipeheader);
                    CurrentUnitOfWork.SaveChanges();
                    recipehdrId = insertedRecipe.Id;
                }
            }


            return dictInsertedOrUpdatedrecipeId;
        }
    }
}
