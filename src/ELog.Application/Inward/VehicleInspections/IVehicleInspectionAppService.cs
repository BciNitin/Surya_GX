using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Inward.VehicleInspections.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Inward.VehicleInspections
{
    public interface IVehicleInspectionAppService : IApplicationService
    {
        Task<VehicleInspectionDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<VehicleInspectionListDto>> GetAllAsync(PagedVehicleInspectionResultRequestDto input);

        Task<VehicleInspectionDto> CreateAsync(CreateVehicleInspectionDto input);

        Task<VehicleInspectionDto> UpdateAsync(UpdateVehicleInspectionDto input);
    }
}