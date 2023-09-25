using Abp.Application.Services;
using ELog.Application.CommonDto;
using ELog.Application.Dispensing.Picking.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.Picking
{
    public interface IPickingAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetCubicleByBarcode(string input);

        Task<HTTPResponseDto> GetCubicleForSamplingAsync(string input);

        Task<HTTPResponseDto> UpdatePickingDtoByBinBarcode(PickingDto input);

        Task<HTTPResponseDto> GetMaterialCodeByGroupId(int cubicleAssignmentHeaderId);

        Task<PickingDto> UpdatePickingDetailAsync(PickingDto input);

        Task<PickingDto> UpdatePickingDetailForSamplingAsync(PickingDto input);

        Task<HTTPResponseDto> SaveMaterialContainerFromBinAsync(PickingDto input);

        Task<HTTPResponseDto> SaveMaterialContainerFromBinForSamplingAsync(PickingDto input);

        Task<HTTPResponseDto> CompletePickingAsync(PickingDto input);

        Task<HTTPResponseDto> CompletePickingForSamplingAsync(PickingDto input);

        Task<HTTPResponseDto> UpdateSAPBatchNoByMaterialCodeAsync(PickingDto input);

        Task<HTTPResponseDto> UpdateSAPBatchNoByMaterialCodeForSamplingAsync(PickingDto input);
    }
}