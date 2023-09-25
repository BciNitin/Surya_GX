using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Departments.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Departments
{
    public interface IDepartmentAppService : IApplicationService
    {
        Task<DepartmentDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<DepartmentListDto>> GetAllAsync(PagedDepartmentResultRequestDto input);

        Task<DepartmentDto> CreateAsync(CreateDepartmentDto input);

        Task<DepartmentDto> UpdateAsync(DepartmentDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectDepartmentAsync(ApprovalStatusDto input);

    }
}