using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Locations.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Locations
{
    public interface ILocationAppService : IApplicationService
    {
        Task<LocationDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<LocationListDto>> GetAllAsync(PagedLocationResultRequestDto input);

        Task<LocationDto> CreateAsync(CreateLocationDto input);

        Task<LocationDto> UpdateAsync(LocationDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectLocationAsync(ApprovalStatusDto input);
    }
}