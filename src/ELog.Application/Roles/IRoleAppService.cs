using Abp.Application.Services.Dto;

using ELog.Application.Roles.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Roles
{
    public interface IRoleAppService
    {
        Task<RoleDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<RoleListDto>> GetAllAsync(PagedRoleResultRequestDto input);

        Task<RoleDto> CreateAsync(CreateRoleDto input);

        Task<RoleDto> UpdateAsync(RoleDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task<List<RolePermissionsDto>> GetAllSubModulesWithPermissions();
    }
}