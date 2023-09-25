

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Elog.Application.ClientForms.Dto;

using System.Threading.Tasks;

namespace Elog.Application.ClientForms
{
    public interface IClientFormsService : IApplicationService
    {
        Task<ClientFormsDto> CreateAsync(ClientFormsDto input);

        Task<ClientFormsDto> GetAsync(EntityDto<int> input);
        Task<PagedResultDto<ClientFormsDto>> GetAllAsync(PagedClientFormsResultRequestDto input);

        //        Task<PagedResultDto<ClientFormsDto>> GetAllAsync(ClientFormsDto input);

        Task<ClientFormsDto> UpdateAsync(ClientFormsDto input);
        // Task DeleteAsync(EntityDto<int> input);
    }


}
