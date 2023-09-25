using Abp.Application.Services;
using ELog.Application.Recipe.Dto;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPSAP.RecipeSAP
{
    public interface IRecipeIntegrationSAP : IApplicationService
    {
        Task<string> InsertOrUpdateRecipeAsync(CreateRecipeMasterDto input);
    }
}
