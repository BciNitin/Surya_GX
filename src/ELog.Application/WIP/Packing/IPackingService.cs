using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.Packing.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.Packing
{
    public interface IPackingService : IApplicationService
    {
        Task<PackingDto> CreateAsync(CreatePackingDto input);
        Task<PackingDto> GetAsync(EntityDto<int> input);
        Task<PackingDto> UpdateAsync(PackingDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<PackingListDto>> GetAllAsync(PagedPackingResultRequestDto input);
    }
}
