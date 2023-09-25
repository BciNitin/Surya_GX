using Abp.Application.Services.Dto;

using ELog.Application.WIP.InProcessLabel.Dto;
using System.Threading.Tasks;

namespace ELog.Application.WIP.InProcessLabel
{
    public interface IProcessLabelAppService
    {
        Task<ProcessLabelDto> CreateAsync(CreateProcessLabelDto input);
        Task<ProcessLabelDto> UpdateAsync(ProcessLabelDto input);

        Task<PagedResultDto<ProcessLabelListDto>> GetAllAsync(PagedProcessLabelResultRequestDto input);
        Task<ProcessLabelDto> GetAsync(EntityDto<int> input);
        Task Print(ProcessLabelDto processLabel);
        Task DeleteAsync(EntityDto<int> input);

        Task<PagedResultDto<ProcessLabelListDto>> GetProductCodeAsync();
    }
}
