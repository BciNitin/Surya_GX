using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.WipPicking.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.WipPicking
{
    public interface IWipPickingService : IApplicationService
    {
        Task<WipPickingDto> CreateAsync(CreateWipPickingDto input);
        Task<WipPickingDto> GetAsync(EntityDto<int> input);
        Task<WipPickingDto> UpdateAsync(WipPickingDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<WipPickingListDto>> GetAllAsync(PagedWipPickingResultRequestDto input);
    }
}
