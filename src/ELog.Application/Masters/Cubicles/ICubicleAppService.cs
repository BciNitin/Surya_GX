using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Cubicles.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Cubicles
{
    public interface ICubicleAppService : IApplicationService
    {
        Task<CubicleDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<CubicleListDto>> GetAllAsync(PagedCubicleResultRequestDto input);

        Task<CubicleDto> CreateAsync(CreateCubicleDto input);

        Task<CubicleDto> UpdateAsync(CubicleDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectCubicleAsync(ApprovalStatusDto input);
    }
}