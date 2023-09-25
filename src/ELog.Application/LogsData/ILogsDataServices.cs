using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.LogsData.Dto;
using System.Threading.Tasks;

namespace ELog.Application.LogsData
{
    public interface ILogsDataServices : IApplicationService
    {
        Task<LogsDataDto> CreateAsync(LogsDataDto input);

        //    Task<LogsDataDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<LogsDataDto>> GetAllAsync(PagedLogsDataResultRequestDto input);





    }
}
