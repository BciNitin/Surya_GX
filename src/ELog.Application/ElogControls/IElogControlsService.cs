
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Elog.Application.ElogControls.Dto;

using System.Threading.Tasks;

namespace Elog.Application.ElogControls
{
    public interface IElogControlsService : IApplicationService
    {
        Task<ElogControlsDto> CreateAsync(ElogControlsDto input);

        Task<ElogControlsDto> GetAsync(EntityDto<int> input);
        Task<PagedResultDto<ElogControlsDto>> GetAllAsync(PagedElogControlsResultRequestDto input);

        //        Task<PagedResultDto<ClientFormsDto>> GetAllAsync(ClientFormsDto input);

        Task<ElogControlsDto> UpdateAsync(ElogControlsDto input);
        // Task DeleteAsync(EntityDto<int> input);
    }


}
