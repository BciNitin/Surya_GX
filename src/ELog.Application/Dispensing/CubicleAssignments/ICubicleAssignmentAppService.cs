using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Dispensing.CubicleAssignments.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.CubicleAssignments
{
    public interface ICubicleAssignmentAppService : IApplicationService
    {
        Task<CubicleAssignmentDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<CubicleAssignmentListDto>> GetAllAsync(PagedCubicleAssignmentResultRequestDto input);

        Task<CubicleAssignmentDto> CreateAsync(CreateCubicleAssignmentDto input);

        Task<CubicleAssignmentDto> UpdateAsync(CubicleAssignmentDto input);
    }
}