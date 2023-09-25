using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.PreStage.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.PreStage
{
    public interface IPreStageAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetCubicleByBarcode(string input, bool isSampling);

        Task<HTTPResponseDto> CompletePreStageAsync(PreStageDto input);

        Task<HTTPResponseDto> CompletePreStageForSamplingAsync(PreStageDto input);

        Task<HTTPResponseDto> GetPickedMaterialCode(string cubicleCode, string groupCode, bool isSampling);

        Task<HTTPResponseDto> GetPickedSAPBatchNos(string cubicleCode, string groupCode, string materialCode, bool isSampling);

        Task<PreStageDto> UpdatePreStageDetailAsync(PreStageDto input, bool isSampling);

        Task<HTTPResponseDto> SavePickedMaterialContainerDetailsAsync(PreStageDto input);

        Task<HTTPResponseDto> SavePickedMaterialContainerDetailsForSamplingAsync(PreStageDto input);
    }
}