using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.Destruction.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.Destruction
{
    public interface IDestructionAppService : IApplicationService
    {
        Task<HTTPResponseDto> UpdateMaterialDetailToDestructionDtoByBarcode(DestructionDto input);

        Task<HTTPResponseDto> PostDestructionDetailToSAP();
    }
}