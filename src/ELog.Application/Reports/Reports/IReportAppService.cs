using Abp.Application.Services;

using ELog.Application.Reports.Dto;
using ELog.Core.SQLDtoEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Masters.Areas
{
    public interface IReportAppService : IApplicationService
    {
        Task<List<VehicleInspectionReportResultDto>> GetVehicleInspectionReportDetailsAsync(VehicleInspectionReportRequestDto input);

        Task<List<VehicleInspectionReportResultDto>> GetMaterialInspectionReportDetailsAsync(VehicleInspectionReportRequestDto input);

        Task<List<AllocationReportResultDto>> GetAllocationReportDetailsAsync(AllocationReportRequestDto input);

        Task<List<DispensingReportDto>> DispensingReportGetDetailsAsync(DispensingReportRequestDto input);
        Task<List<DispatchReportDto>> DispatchReportGetDetailsAsync(DispatchReportRequestDto input);
    }
}