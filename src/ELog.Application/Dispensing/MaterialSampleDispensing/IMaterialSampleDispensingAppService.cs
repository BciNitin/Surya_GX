using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.MaterialSampleDispensing.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.MaterialSampleDispensing
{
    public interface IMaterialSampleDispensingAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetRLAFByBarcode(string input);

        Task<HTTPResponseDto> GetMaterialByInspectionLotId(int inspectionLotId);

        Task<HTTPResponseDto> UpdateSAPBatchByMaterialCode(MaterialSampleDispensingDto input);
        Task<MaterialSampleDispensingDto> UpdateRequiredQuantityToSampleDto(MaterialSampleDispensingDto input);

        Task<HTTPResponseDto> UpdateMaterialContainerByBarcode(MaterialSampleDispensingDto input);

        Task<List<SelectListDto>> GetSamplingTypesAsync();

        Task<HTTPResponseDto> CompleteSamplingAsync(MaterialSampleDispensingDto input);

        Task<HTTPResponseDto> PrintSamplingBarcodeAsync(MaterialSampleDispensingDto input);

        Task<List<SelectListDto>> GetAllSuggestedBalancesAsync(int uomId);

        Task<bool> ReprintSamplingDetailAsync(MaterialSampleDispensingDto input);

        Task<List<MaterialSampleDispensingDetailDto>> UpdateAllMaterialDispensingDetailBySAPBatch(MaterialSampleDispensingDto input);
    }
}