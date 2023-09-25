using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.MaterialReturn.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.MaterialReturn
{
    public interface IMaterialReturnService : IApplicationService
    {
        Task<MaterialReturnDto> CreateAsync(CreateMaterialReturnDto input);
        Task<MaterialReturnDto> GetAsync(EntityDto<int> input);
        Task<MaterialReturnDto> UpdateAsync(MaterialReturnDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<MaterialReturnListDto>> GetAllAsync(PagedMaterialReturnRequestDto input);
    }
}
