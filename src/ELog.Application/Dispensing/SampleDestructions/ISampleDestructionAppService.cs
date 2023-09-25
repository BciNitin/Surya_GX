using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.SampleDestructions.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.SampleDestructions
{
    public interface ISampleDestructionAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetAllSampleInspectionLotNosAsync();

        Task<HTTPResponseDto> GetMaterialByInspectionLotId(int inspectionLotId);

        Task<HTTPResponseDto> GetSamplingSapBatchNumbersAsync(int? inspectionLotId, string materialCode);

        Task<HTTPResponseDto> UpdateSampleDestructionByContainerBarcode(SampleDestructionDto input);
    }
}