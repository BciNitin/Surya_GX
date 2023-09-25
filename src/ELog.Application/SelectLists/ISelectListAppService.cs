using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.SelectLists
{
    public interface ISelectListAppService : IApplicationService
    {
        Task<List<SelectListDto>> GetModesAsync();

        Task<List<SelectListDto>> GetDesignationAsync();

        Task<List<SelectListDto>> GetApprovalStatusAsync();

        List<SelectListDto> GetSortByUser();

        Task<List<SelectListDto>> GetPlantAsync();

        Task<List<SelectListDto>> GetPlantByUserIdAsync(long userId);

        Task<List<SelectListDto>> GetReportingManagerUserAsync();

        Task<List<SelectListDto>> GetRolesAsync();

        List<SelectListDto> GetSortByRole();

        List<SelectListDto> GetStatus();
        Task<List<SelectListDtoWithPlantIdPalletization>> GetAllPalletMasterPalletsAsync();

        List<SelectListDto> GetSortByPlant();

        Task<List<SelectListDto>> GetCountriesAsync();

        Task<List<SelectListDto>> GetStatesAsync(int countryId);

        List<SelectListDto> GetSortByGate();

        List<SelectListDto> GetSortByLocation();

        Task<List<SelectListDto>> GetAllMasterPlants();

        Task<List<SelectListDto>> GetAllSubPlants();

        Task<List<SelectListDto>> GetStorageLocationsAsync();

        List<SelectListDto> GetSortByCubicle();

        List<SelectListDto> GetSortByEquipment();

        Task<List<SelectListDto>> GetEquipmentTypesAsync();

        List<SelectListDto> GetSortByHandlingUnit();

        Task<List<SelectListDto>> GetHandlingUnitTypesAsync();

        List<SelectListDto> GetSortByUnitOfMeasurement();

        Task<List<SelectListDto>> GetUnitOfMeasurementTypesAsync();

        Task<List<SelectListDto>> GetConversionUOMMastersAsync();

        List<SelectListDto> GetSortByInspectionChecklist();

        List<SelectListDto> GetSortByWeighingMachine();

        List<SelectListDto> GetSortByDevice();

        Task<List<SelectListDto>> GetPrintersAsync();

        List<SelectListDto> GetSortByArea();

        Task<List<SelectListDto>> GetSubModulesAsync();

        Task<List<SelectListDto>> GetModulesAsync();

        List<SelectListDto> GetSortByDepartment();

        List<SelectListDto> GetSortByStandardWeights();

        Task<List<SelectListDto>> GetSubModuleTypeAsync();

        List<SelectListDto> GetSortByStandardWeightBox();

        List<SelectListDto> GetSortBySubModule();

        Task<List<SelectListDto>> GetAreasAsync();

        Task<List<SelectListDto>> GetCubiclesAsync();

        Task<List<SelectListDto>> GetDeviceTypesAsync();

        Task<List<PurchaseOrderInternalDto>> GetPurchaseOrdersAsync();

        Task<List<PurchaseOrderInternalDto>> GetPurchaseOrdersAutoCompleteAsync(string input);

        List<SelectListDto> GetSortByVehicleInspection();

        List<SelectListDto> GetSortByMaterialInspection();

        List<SelectListDto> GetTemperatureUnit();

        List<SelectListDto> GetWeighingMachineFrequencyType();

        List<SelectListDto> GetWeighingMachineBalancedType();

        List<SelectListDto> GetSortByChecklistType();

        Task<List<SelectListDto>> GetAssociatedPlantByUserIdAsync(long userId);

        bool GetGateEntryStatus();

        Task<List<SelectListDto>> GetTransactionStatusAsync();

        Task<List<SelectListDto>> GetChecklistsAsync(int checklistTypeId);

        List<SelectListDto> GetSortByWeightCapture();

        Task<List<SelectListDto>> GetUnitOfMeasurementByIdAsync(int uomId);

        List<SelectListDto> GetSortByPalletization();

        Task<List<SelectListDto>> GetHolidayTypeAsync();

        List<SelectListDto> GetSortByCalender();
        Task<List<SelectListDto>> GetAllUsers();
        Task<List<SelectListDto>> GetAllApprovelLevels();
        Task<List<SelectListDto>> GetAllSubModules();
        Task<List<SelectListDto>> GetAllcubicals();
        Task<List<SelectListDto>> GetAllActivity(string subModule);
        Task<List<SelectListDto>> GetAllEquipments();

        Task<List<SelectListDtoWithPlantId>> GetAllMaterialSelectListAsync();

        Task<List<SelectListDto>> GetStandardWeightBoxAsync(int plantId);

        List<SelectListDto> GetCalibrationStatusTest();

        List<SelectListDto> GetCalibrationStatus();

        List<SelectListDto> GetSortByWeighingCalibration();
        Task<double> GetWeightByWeighingMachineIdAsync(int weighingMachineId, bool isWeightUOMType);
        Task<List<PurchaseOrderInternalDto>> GetPurchaseOrdersByPlantIdAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetAllWeighingMachineByPlantIdAsync(int? plantId);
        //List<SelectListDto> getSortByWeighingCalibration();
        Task<List<SelectListDto>> GetMaterialMasterAsync();
        Task<List<SelectListDtoWithPlantId>> GetAllCheckList(string subModule);
        Task<List<SelectListDto>> GetAllPalletCode();
        Task<List<SelectListDto>> GetAllShipperCode();
        Task<List<SelectListDto>> GetProcessOrdersAssignedToCubicleAsync();

        Task<string> GetStampingDueOnInfoByIdAsync(int weighingMachineId);

        Task<List<SelectListDtoWithPlantId>> GetAllInspectionCheckList(string subModule);

    }
}