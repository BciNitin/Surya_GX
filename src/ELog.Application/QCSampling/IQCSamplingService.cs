using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.QCSampling.Dto;
using System.Threading.Tasks;

namespace ELog.Application.QCSampling
{
    public interface IQCSamplingService : IApplicationService
    {
        // Task<QCSamplingDto> CreateAsync(QCSamplingDto input);

        Task<QCSamplingDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<QCSamplingDto>> GetAllAsync(PagedQCSamplingResultRequestDto input);

        // Task<QCSamplingDto> UpdateAsync(QCSamplingDto input);
        //Task DeleteAsync(EntityDto<int> input);

    }
}
