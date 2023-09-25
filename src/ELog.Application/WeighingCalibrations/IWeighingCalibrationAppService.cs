using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.SelectLists.Dto;
using ELog.Application.WeighingCalibrations.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WeighingCalibrations
{
    public interface IWeighingCalibrationAppService : IApplicationService
    {
        Task<WeighingCalibrationDto> GetAsync(int weighingMachineId);

        Task<PagedResultDto<WeighingCalibrationListDto>> GetAllAsync(PagedWeighingCalibrationResultRequestDto input);

        Task<List<SelectListDto>> GetWeighingMachineSelectListAutoComplete(string weighingMachineCode);

        Task<WeighingCalibrationDto> UpdateAsync(WeighingCalibrationDto input);

        Task<List<SelectListDto>> GetAllStandardWeightByBoxId(int standardWeightBoxId);

        Task<List<SelectListDto>> GetAllStandardWeightBoxAutoComplete(string standardWeightBoxId);

        Task<CreateWeighingCalibrationResultDto> CreateAsync(CreateWeighingCalibrationDto input);

        Task<WeighingCalibrationDto> GetCalibrationAsync(int WeighingCalibrationHeaderId);

        Task<double> GetWeightByWeighingMachineCode(int weighingMachineId, double? testWeight = null);
        Task<bool> IsWeighingMachineCalibrated(int? WeighingMachineId);

        Task<bool> isNewCalibration(int weighingMachineId, int? frequencyId);
    }
}