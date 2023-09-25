using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Masters.StandardWeightBoxes.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.StandardWeightBoxes
{
    public interface IStandardWeightBoxAppService : IApplicationService
    {
        Task<StandardWeightBoxDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<StandardWeightBoxListDto>> GetAllAsync(PagedStandardWeightBoxResultRequestDto input);

        Task<StandardWeightBoxDto> CreateAsync(CreateStandardWeightBoxDto input);

        Task<StandardWeightBoxDto> UpdateAsync(StandardWeightBoxDto input);

        Task DeleteAsync(EntityDto<int> input);
    }
}