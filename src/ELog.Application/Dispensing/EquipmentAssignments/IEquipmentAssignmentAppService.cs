using Abp.Application.Services;
using ELog.Application.CommonDto;
using ELog.Application.Dispensing.EquipmentAssignments.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.EquipmentAssignments
{
    public interface IEquipmentAssignmentAppService : IApplicationService
    {
        Task<EquipmentAssignmentDto> GetCubicleAsync(string input);

        Task<HTTPResponseDto> AssignEquipmentForSamplingAsync(EquipmentAssignmentDto input);

        Task<HTTPResponseDto> AssignEquipmentAsync(EquipmentAssignmentDto input);

        Task<HTTPResponseDto> DeAssignEquipmentAsync(EquipmentAssignmentDto input);

        Task<HTTPResponseDto> DeAssignEquipmentForSamplingAsync(EquipmentAssignmentDto input);

        Task<HTTPResponseDto> GetAllEquipmentAssignmentBarcodeAsync(EquipmentBarcodeRequestDto input);

        Task<HTTPResponseDto> GetAllEquipmentAssignmentBarcodeForSamplingAsync(EquipmentBarcodeRequestDto input);
    }
}