using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Gates.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Gates
{
    public interface IGateAppService : IApplicationService
    {
        Task<GateDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<GateListDto>> GetAllAsync(PagedGateResultRequestDto input);

        Task<GateDto> CreateAsync(CreateGateDto input);

        Task<GateDto> UpdateAsync(GateDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectGateAsync(ApprovalStatusDto input);
    }
}