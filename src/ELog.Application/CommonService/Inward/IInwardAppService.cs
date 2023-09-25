using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Inward.GateEntries.Dto;
using ELog.Application.Inward.MaterialInspections.Dto;
using ELog.Application.Inward.Palletizations.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Masters.UnitOfMeasurements.Dto;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.Inward
{
    public interface IInwardAppService : IApplicationService
    {
        Task<List<GateEntryDto>> GetGateEntriesAsync(string input);

        Task<List<CheckpointDto>> GetCheckpointsByChecklistIdAsync(int checklistId, int modeId);

        Task<List<MaterialSelectWithDescriptionDto>> GetMaterialSelectListDtoAsync(int purchaseOrderId);

        Task<List<MaterialConsignmentDto>> GetConsignmentByMaterialIdAsync(int materialId);

        Task<List<SelectListDtoWithPlantId>> GetWeighingMachineAutoCompleteAsync(string input);
        Task<WeighingMachineSelectWithDetailsDto> GetWeighingMachineDetailsAsync(string input, bool isWeightUOM);

        Task<List<SelectListDto>> GetInvoiceByPurchaseOrderIdAsync(int purchaseOrderId);

        Task<List<MaterialInternalDto>> GetMaterialByInvoiceIdAsync(int invoiceId);
        Task<List<SelectListDtoWithPlantId>> GetAllPalletsBarcodeAsync(string input);
        Task<List<MaterialBarcodePrintingDto>> GetAllMaterialSelectListDtoAsync(string searchText);

        Task<List<SelectListDto>> GetInvoiceByPurchaseOrderIdAutoCompleteAsync(int purchaseOrderId, string input);
        Task<List<UnitOfMeasurementListDto>> GetUnitOfMeasurementDetailsByIdAsync(int uomId);
        Task<double> GetWeightFromWeighingMachine(string ipAddress, int? portNumber);
        Task<List<PurchaseOrderInternalDto>> GetVehicleInspectionPurchaseOrdersAsync();
        Task<double> GetWeightForTesting(string ipAddress, int portNumber);

        Task<List<PurchaseOrderInternalDto>> GetGateEntryPurchaseOrdersAsync();

        Task<List<GateEntryDto>> validateGateEntriesAsync(string input);
    }
}