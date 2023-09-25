using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Devices.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Devices
{
    public interface IDeviceAppService : IApplicationService
    {
        Task<DeviceDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<DeviceListDto>> GetAllAsync(PagedDeviceResultRequestDto input);

        Task<DeviceDto> CreateAsync(CreateDeviceDto input);

        Task<DeviceDto> UpdateAsync(DeviceDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task ApproveOrRejectDeviceAsync(ApprovalStatusDto input);
    }
}