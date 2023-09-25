using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.StageOut.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.StageOut
{
    public interface IStageOutAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetGroupIdByCubicleBarcodeAsync(string input);

        Task<HTTPResponseDto> GetGroupIdByCubicleBarcodeForSamplingAsync(string input);

        Task<HTTPResponseDto> GetMaterialCodeByGroupIdAsync(int cubicleAssignmentHeaderId);

        Task<HTTPResponseDto> GetMaterialCodeByGroupIdForSamplingAsync(int inspectionLotId);

        Task<HTTPResponseDto> UpdateStageOutSAPBatchNoMaterialCodeAsync(StagingOutDto input);

        Task<HTTPResponseDto> UpdateStageOutSAPBatchNoMaterialCodeForSamplingAsync(StagingOutDto input);

        Task<HTTPResponseDto> UpdateMaterialContainerByBarcodeAsync(StagingOutDto input);

        Task<HTTPResponseDto> UpdateMaterialContainerByBarcodeForSamplingAsync(StagingOutDto input);

        Task<StagingOutDto> UpdateStageOutContainerCountAndBalanceQuantity(StagingOutDto input);

        Task<HTTPResponseDto> CompleteStageOutAsync(StagingOutDto input);

        Task<HTTPResponseDto> CompleteStageOutForSamplingAsync(StagingOutDto input);
    }
}