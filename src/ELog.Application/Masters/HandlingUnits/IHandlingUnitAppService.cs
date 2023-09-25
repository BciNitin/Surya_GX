using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.HandlingUnits.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.HandlingUnits
{
    public interface IHandlingUnitAppService : IApplicationService
    {
        Task<HandlingUnitDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<HandlingUnitListDto>> GetAllAsync(PagedHandlingUnitResultRequestDto input);

        Task<HandlingUnitDto> CreateAsync(CreateHandlingUnitDto input);

        Task<HandlingUnitDto> UpdateAsync(HandlingUnitDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectHandlingUnitAsync(ApprovalStatusDto input);
    }
}