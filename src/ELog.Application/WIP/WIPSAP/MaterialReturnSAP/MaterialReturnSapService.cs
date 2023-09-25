using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.WIP.WIPSAP.MaterialReturnSAP.Dto;
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

namespace ELog.Application.WIP.WIPSAP.MaterialReturnSAP
{
    public class MaterialReturnSapService : ApplicationService, IMaterialReturnSapService
    {
        private readonly IRepository<MaterialRteturnDetailsSAP> _materialRteturnDetailsSAP;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<MaterialMaster> _materialMaster;

        public MaterialReturnSapService(IRepository<MaterialMaster> materialMaster, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor, IRepository<MaterialRteturnDetailsSAP> materialRteturnDetailsSAP)

        {

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _httpContextAccessor = httpContextAccessor;
            _materialRteturnDetailsSAP = materialRteturnDetailsSAP;
            _materialMaster = materialMaster;

        }

        public async Task<string> InsertMaterialReturnAsync(MaterialRteturnDetailsSAPDto input)
        {
            var docNo = await GetDocNoIfAlradyExist(input);
            int productId = 0;
            productId = await GetProductCodeIfAlradyExist(input);
            int? MatDetId = null;
            if (docNo > 0)
            {
                var materialRteturn = await _materialRteturnDetailsSAP.GetAsync(docNo);
                ObjectMapper.Map(input, materialRteturn);
                materialRteturn.ProductId = productId;
                var materialRteturnDetails = await _materialRteturnDetailsSAP.UpdateAsync(materialRteturn);
                MatDetId = materialRteturnDetails.Id;
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                var materialRteturnDetails = ObjectMapper.Map<MaterialRteturnDetailsSAP>(input);
                materialRteturnDetails.ProductId = productId;
                var insertedRecipe = await _materialRteturnDetailsSAP.InsertAsync(materialRteturnDetails);
                CurrentUnitOfWork.SaveChanges();
                MatDetId = insertedRecipe.Id;
            }
            return Convert.ToString(MatDetId);
        }
        [PMMSAuthorize(Permissions = "MaterialReturn.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {

            var materialreturn = await _materialRteturnDetailsSAP.GetAsync(input.Id).ConfigureAwait(false);
            await _materialRteturnDetailsSAP.DeleteAsync(materialreturn).ConfigureAwait(false);
        }

        private async Task<int> GetProductCodeIfAlradyExist(MaterialRteturnDetailsSAPDto input)
        {
            return await _materialMaster.GetAll().Where(x => x.MaterialCode.ToLower() == input.ProductName.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }
        private async Task<int> GetDocNoIfAlradyExist(MaterialRteturnDetailsSAPDto input)
        {
            var docNo = input.MaterialDocumentNo;

            return await _materialRteturnDetailsSAP.GetAll().Where(x => x.MaterialDocumentNo.ToLower() == docNo.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<List<MaterialRteturnDetailsSAP>> GetMaterialReturnSAPDetailsList()
        {
            var entity = await _materialRteturnDetailsSAP.GetAllListAsync();
            //var materialRteturnDetails = ObjectMapper.Map<MaterialRteturnDetailsSAPDto>(entity);
            return entity;
        }
    }
}
