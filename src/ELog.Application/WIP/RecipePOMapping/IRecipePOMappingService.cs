using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.RecipePOMapping.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WIP.RecipePOMapping
{
    public interface IRecipePOMappingService : IApplicationService
    {
        Task<RecipeToPOLinkDto> CreateAsync(CreateRecipeToPOLinkDto input);
        Task<RecipeToPOLinkDto> GetRecipeAsync(EntityDto<int> input);
        //Task<PagedResultDto<MaterialListDto>> GetMaterialByProcessOrderAsync(EntityDto<int> input);
        Task<List<MaterialListDto>> GetMaterialByProcessOrderAsync(EntityDto<int> input);
        // Task<RecipePOMappingListDto> GetAsync(EntityDto<int> input);
        Task<CreateRecipeDetailsDto> GetRecipeHeaderByHdrIdAsync(string input);
        Task<PagedResultDto<RecipePOMappingListDto>> GetAllRecipeAsync(PagedRecipeToPOLinkResultRequestDto input);
    }
}
