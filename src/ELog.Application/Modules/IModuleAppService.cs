using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Modules.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Modules
{
    public interface IModuleAppService : IApplicationService
    {
        Task<PagedResultDto<ModuleListDto>> GetAllModuleAsync(PagedModuleResultRequestDto input);

        Task<PagedResultDto<SubModuleListDto>> GetAllSubModuleAsync(PagedSubModuleResultRequestDto input);

        Task<ModuleDto> Update(ModuleDto input);

        Task<ModuleDto> Get(EntityDto<int> input);

        Task<List<SubModuleDto>> GetSubModules(EntityDto<int> input);

        Task<SubModuleDto> GetSubModule(int input);

        Task<UpdateSubModuleDto> UpdateSubModule(UpdateSubModuleDto input);

        Task<ModuleSubModuleDto> AssignSubModules(ModuleSubModuleDto input);
        Task<int> GetModuleByName(string input);
        Task<int> GetSubmoduleByName(string input);
    }
}