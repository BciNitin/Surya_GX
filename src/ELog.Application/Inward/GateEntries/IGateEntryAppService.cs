using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Inward.GateEntries.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Inward.GateEntries
{
    public interface IGateEntryAppService : IApplicationService
    {
        Task<GateEntryDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<GateEntryListDto>> GetAllAsync(PagedGateEntryResultRequestDto input);

        Task<GateEntryDto> CreateAsync(CreateGateEntryDto input);

        Task<GateEntryDto> UpdateAsync(UpdateGateEntryDto input);

        Task<GateEntryDto> CreateGatePassAsync(CreateGateEntryDto[] input);

    }
}