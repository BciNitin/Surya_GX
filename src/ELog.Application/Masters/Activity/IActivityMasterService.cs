using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Activity.Dto;
using ELog.Application.CommonDto;

using System.Threading.Tasks;

namespace ELog.Application.Activity
{
    public interface IActivityMasterService : IApplicationService
    {
        Task<ActivityDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<ActivityListDto>> GetAllAsync(PagedActivityResultRequestDto input);

        Task<ActivityDto> CreateAsync(CreateActivityDto input);

        Task<ActivityDto> UpdateAsync(ActivityDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectActivityAsync(ApprovalStatusDto input);
    }
}
