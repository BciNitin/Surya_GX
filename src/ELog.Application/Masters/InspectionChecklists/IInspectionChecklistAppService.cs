using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.InspectionChecklists.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.InspectionChecklists
{
    public interface IInspectionChecklistAppService : IApplicationService
    {
        Task<InspectionChecklistDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<InspectionChecklistListDto>> GetAllAsync(PagedInspectionChecklistResultRequestDto input);

        Task<InspectionChecklistDto> CreateAsync(CreateInspectionChecklistDto input);

        Task<InspectionChecklistDto> UpdateAsync(InspectionChecklistDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task ApproveOrRejectInspectionCheklistAsync(ApprovalStatusDto input);
    }
}