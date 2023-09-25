using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Masters.Areas.Dto;
using System.Threading.Tasks;

namespace MobiVueEvo.Application.Masters.Areas
{
    public interface IAreaAppService : IApplicationService
    {
        Task<AreaDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<AreaListDto>> GetAllAsync(PagedAreaResultRequestDto input);

        Task<AreaDto> CreateAsync(CreateAreaDto input);

        Task<AreaDto> UpdateAsync(AreaDto input);

        Task DeleteAsync(EntityDto<int> input);
    }
}