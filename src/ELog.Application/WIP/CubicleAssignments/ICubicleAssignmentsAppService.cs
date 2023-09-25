using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.WIP.CubicleAssignments.Dto;
using System.Threading.Tasks;


namespace ELog.Application.WIP.CubicleAssignments
{
    public interface ICubicleAssignmentsAppService : IApplicationService
    {
        Task<CubicleAssignmentsDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<CubicleAssignmentsListDto>> GetAllAsync(PagedCubicleAssignmenstResultRequestDto input);

        Task<CubicleAssignmentsDto> CreateAsync(CreateCubicleAssignmentsDto input);
        Task<CubicleAssignmentsDto> UpdateAsync(CubicleAssignmentsDto input);
        // Task DeleteAsync(EntityDto<int> input);
    }
}
