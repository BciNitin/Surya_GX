using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.ChecklistTypes.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.ChecklistTypes
{
    public interface IChecklistTypeAppService : IApplicationService
    {
        Task<ChecklistTypeDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<ChecklistTypeListDto>> GetAllAsync(PagedChecklistTypeResultRequestDto input);

        Task<ChecklistTypeDto> CreateAsync(CreateChecklistTypeDto input);

        Task<ChecklistTypeDto> UpdateAsync(ChecklistTypeDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task ApproveOrRejectChecklistTypeAsync(ApprovalStatusDto input);

    }
}