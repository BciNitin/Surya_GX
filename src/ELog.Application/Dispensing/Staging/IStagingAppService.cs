using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.Stage.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.Stage
{
    public interface IStagingAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetCubicleByBarcode(string input, bool isSampling);

        Task<HTTPResponseDto> CompleteStagingAsync(StagingDto input);

        Task<HTTPResponseDto> CompleteStagingForSamplingAsync(StagingDto input);

        Task<HTTPResponseDto> GetPreStageMaterialCode(string cubicleCode, string groupCode, bool isSampling);

        Task<StagingDto> UpdateStagingDetailAsync(StagingDto input, bool isSampling);

        Task<HTTPResponseDto> SaveStagingMaterialContainerDetailsAsync(StagingDto input);

        Task<HTTPResponseDto> SaveStagingMaterialContainerDetailsForSamplingAsync(StagingDto input);
    }
}