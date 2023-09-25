using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.EquipmentUsageLog.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ELog.Application.EquipmentUsageLog
{
    public interface IEquipmentUsageLogService : IApplicationService
    {
        Task<EquipmentUsageLogDto> CreateAsync(CreateEquipmentUsageLogDto input);
        Task<EquipmentUsageLogDto> GetAsync(EntityDto<int> input);

        Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId);
        Task<EquipmentUsageLogDto> UpdateAsync(UpdateEquipmentUsageLogDto input);

        Task<PagedResultDto<EquipmentUsageLogListDto>> GetAllAsync(PagedEquipmentUsageLogResultRequestDto input);
    }
}
