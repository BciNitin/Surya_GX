using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.CommonDto;
using ELog.Application.Masters.PRNEntry.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Masters.PRNEntry
{
    public interface IPRNEntryService : IApplicationService
    {
        Task<PRNEntryDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<PRNEntryListDto>> GetAllAsync(PagedPRNEntryResultRequestDto input);

        Task<PRNEntryDto> CreateAsync(CreatePRNEntryDto input);

        Task<PRNEntryDto> UpdateAsync(PRNEntryDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectActivityAsync(ApprovalStatusDto input);
    }
}
