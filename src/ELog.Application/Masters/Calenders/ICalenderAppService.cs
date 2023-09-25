using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Masters.Calenders.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Calenders
{
    public interface ICalenderAppService : IApplicationService
    {
        Task<CalenderDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<CalenderListDto>> GetAllAsync(PagedCalenderResultRequestDto input);

        Task<CalenderDto> CreateAsync(CreateCalenderDto input);

        Task<CalenderDto> UpdateAsync(CalenderDto input);

        Task DeleteAsync(EntityDto<int> input);
    }
}