using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.AreaCleaning.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.AreaCleaning
{
    public interface IAreaUsageLogService : IApplicationService
    {
        Task<AreaUsageLogDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<AreaUsageLogListDto>> GetAllAsync(PagedAreaUsageLogResultRequestDto input);

        Task<AreaUsageLogDto> CreateAsync(CreateAreaUsageLogDto input);

        Task<AreaUsageLogDto> UpdateAsync(UpdateAreaUsageLogDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId);
        Task UpdateAreaUsageLogTransaction(AreaUsageLogDto input);
    }
}
