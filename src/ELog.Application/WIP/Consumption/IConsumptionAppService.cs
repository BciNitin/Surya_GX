using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.WIP.Consumption.Dto;
using System.Threading.Tasks;

namespace ELog.Application.WIP.Consumption
{
    public interface IConsumptionAppService : IApplicationService
    {
        Task<ConsumptionDto> CreateAsync(CreateConsumptionHeaderDto input);
        Task<ConsumptionDto> UpdateAsync(ConsumptionDto input);

        Task<PagedResultDto<ConsumptionListDto>> GetAllAsync(PagedConsumptionResultRequestDto input);
        Task<ConsumptionDto> GetAsync(EntityDto<int> input);
        Task<dynamic> GetProcessOrderByCubicleIdAsync(EntityDto<int> input);
    }
}
