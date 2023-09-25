using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing.Dto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.Dispensing.MaterialDispensing.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.Dispensing
{
    public interface IDispensingAppService : IApplicationService
    {
        Task<List<CubicleBarcodeDto>> GetAllCubicleBarcodeAsync(string input);

        Task<List<SelectListDto>> GetAllCubicleCleaningType();

        Task<List<CheckpointDto>> GetCheckpointsBySubModuleIdIdAsync(string subModuleName, int modeId);

        Task<int> GetStatusByModuleSubModuleName(string module, string submodule, string status);

        Task<List<SelectListDto>> GetAllEquipmentCleaningType();

        Task<List<EquipmentCleaningBarcodeDto>> GetAllEquipmentBarcodeAsync(string input, bool isSampling);

        Task RejectLineClearanceTransaction(int? cubicleId, int? groupId, bool isSampling);

        Task<List<SelectListDto>> GetAllGroupCodeAsync(int cubicleId, bool isSampling);

        Task<BarcodeValidationDto> IsCubicleCleaned(int cubicleId, bool isSampling);

        Task<List<StatusMaster>> GetStatusListByModuleSubModuleName(string module, string submodule);

        HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, string ValidationError);
        Task<List<MaterialDispensingInternalDto>> GetAllUOMByMaterialCodeAsync(string input);
        Task<HTTPResponseDto> GetBalanceByBarcode(string input, int uomId);

        Task<List<SelectListDto>> GetAllSuggestedBalancesAsync(int uomId);

        Task<float> GetConvertedQuantityByUOM(string materialCode, int baseUOMId, int ConversionUOMId, float quantity);
        Task<List<MaterialDispensingInternalDto>> GetAllSamplingUOMByMaterialCodeAsync(string materialCode, string sapBatchNumber, int inspectionLotId);
        Task<DispensingUnitOfMeasurementDto> GetSamplingBaseUOMAsync(string materialCode, int? inspectionLotId, string SAPBatchNo);
    }
}