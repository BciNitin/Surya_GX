using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.WeighingMachines.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Masters.WeighingMachines
{
    public interface IWeighingMachineAppService : IApplicationService
    {
        Task<WeighingMachineDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<WeighingMachineListDto>> GetAllAsync(PagedWeighingMachineResultRequestDto input);

        Task<WeighingMachineDto> CreateAsync(CreateWeighingMachineDto input);

        Task<WeighingMachineDto> UpdateAsync(WeighingMachineDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectWeighingMachineAsync(ApprovalStatusDto input);
        Task<List<WeighingMachineStampingDueOnListDto>> GetStampingDueOnWMListAsync();
    }
}