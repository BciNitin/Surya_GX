using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Inward.MaterialInspections.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Inward.MaterialInspections
{
    public interface IMaterialInspectionAppService : IApplicationService
    {
        Task<MaterialInspectionDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<MaterialInspectionListDto>> GetAllAsync(PagedMaterialInspectionResultRequestDto input);

        Task<int> CreateAsync(CreateMaterialInspectionDto input);

        Task<MaterialInspectionDto> UpdateAsync(MaterialInspectionDto input);
        Task<bool> IsMaterialInspectionPresent(int? GateEntryId, string PO_Number, string invoice_Number, int? invoiceId = null);
        Task<bool> IsVehicleInspectionIsInProgress(int? GateEntryId, string PO_Number, string invoice_Number, int? invoiceId = null);
    }
}