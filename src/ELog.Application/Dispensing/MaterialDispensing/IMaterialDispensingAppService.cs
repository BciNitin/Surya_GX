using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.MaterialDispensing.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.MaterialDispensing
{
    public interface IMaterialDispensingAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetRLAFByBarcode(string input);

        Task<HTTPResponseDto> GetMaterialByProcessOrderId(int processOrderId);

        Task<HTTPResponseDto> UpdateSAPBatchByMaterialCode(MaterialDispensingDto input);

        Task<MaterialDispensingDto> UpdateRequiredQuantityToDispensingDto(MaterialDispensingDto input);

        Task<HTTPResponseDto> UpdateMaterialBatchDispensingVerify(GetDespensingDetailsStatus input);

        Task<List<MaterialDispensingInternalDto>> GetAllUOMByMaterialCodeAsync(string materialCode, string sapBatchNumber, int processOrderId);

        Task<HTTPResponseDto> GetBalanceByBarcode(string input, int uomId);

        Task<HTTPResponseDto> UpdateMaterialContainerByBarcode(MaterialDispensingDto input);



        Task<List<SelectListDto>> GetAllSuggestedCalibratedBalancesAsync(int uomId);
    }
}