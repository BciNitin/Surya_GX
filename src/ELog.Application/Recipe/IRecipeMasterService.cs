using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Recipe.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Recipe
{
    public interface IRecipeMasterService : IApplicationService
    {
        Task<string> UpdateAsync(CreateRecipeMasterDto input);

        Task<List<CreayeRecipeMasterdetail>> GetRecipeDetailsByHdrIdAsync(EntityDto<int> input);
        Task<CreateRecipeMasterDto> GetRecipeHeaderByHdrIdAsync(EntityDto<int> input);
        Task<PagedResultDto<RecipeMasterListDto>> GetAllRecipeApprovalAsync(PagedRecipeMasterResultRequestDto input);
        Task<string> ApproveRecipeMaster(CreateRecipeMasterDto input);
        Task<string> RejectRecipeMaster(CreateRecipeMasterDto input);
        Task<int> CheckRecipeIdIfAlreadyExist(int input);
        Task DeleteAsync(int input);
        Task<CreateRecipeMasterDto> GetRecipeHeaderByProductCodeAsync(string input);
    }

}
