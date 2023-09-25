using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.LogFormsHistoryApi.Dto;
using System.Threading.Tasks;

namespace ELog.Application.LogFormsHistoryApi
{
    public interface ILogFormHistoryService : IApplicationService
    {
        Task<LogFormHistoryDto> CreateAsync(LogFormHistoryDto input);
        // Task<LogFormHistoryDto> UpdateAsync(LogFormHistoryDto input);

        Task<LogFormHistoryDto> GetAsync(EntityDto<int> input);
        Task<PagedResultDto<LogFormHistoryDto>> GetAllAsync(PagedLogFormHistoryResultRequestDto input);


    }
}
