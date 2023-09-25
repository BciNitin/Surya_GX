using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Masters.StandardWeights.Dto;
using ELog.Application.Masters.WeighingMachines.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Masters.StandardWeights
{
    public interface IStandardWeightAppService : IApplicationService
    {
        Task<StandardWeightDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<StandardWeightListDto>> GetAllAsync(PagedStandardWeightResultRequestDto input);

        Task<StandardWeightDto> CreateAsync(CreateStandardWeightDto input);

        Task<StandardWeightDto> UpdateAsync(StandardWeightDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task<List<StandardWeightStampingDueListDto>> GetStandardWeightStampingDueList();
    }
}