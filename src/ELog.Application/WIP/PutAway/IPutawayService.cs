using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.PutAway.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.PutAway
{
    public interface IPutawayService : IApplicationService
    {
        Task<PutawayDto> CreateAsync(CreatePutawayDto input);
        Task<PutawayDto> GetAsync(EntityDto<int> input);
        Task<PutawayDto> UpdateAsync(PutawayDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<PutawayListDto>> GetAllAsync(PagedPutawayResultRequestDto input);
        Task<PagedResultDto<PutawayListDto>> GetListAsync();


    }
}
