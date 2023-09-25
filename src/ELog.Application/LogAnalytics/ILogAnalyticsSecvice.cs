using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.LogAnalytics.Dto;
using System.Threading.Tasks;

namespace ELog.Application.LogAnalytics
{
    public interface ILogAnalyticsSecvice : IApplicationService
    {
        Task<LogAnalyticsDto> GetAsync(EntityDto<int> input);
        Task<PagedResultDto<LogAnalyticsDto>> GetAllAsync(PagedLogAnalyticsResultRequestDto input);


    }
}
