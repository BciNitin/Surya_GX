using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Plants.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Plants
{
    public interface IPlantAppService : IApplicationService
    {
        Task<PlantDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<PlantListDto>> GetAllAsync(PagedPlantResultRequestDto input);

        Task<PlantDto> CreateAsync(CreatePlantDto input);

        Task<PlantDto> UpdateAsync(PlantDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectPlantAsync(ApprovalStatusDto input);
    }
}