using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.PostToSAP.Dto;
using System.Threading.Tasks;

namespace ELog.Application.WIP.PostToSAP
{
    public interface IPostToSAP : IApplicationService
    {
        Task<PostToSAPDto> CreateAsync(CreatePostToSAPDto input);
        Task<PostToSAPDto> GetAsync(EntityDto<int> input);
        Task<PostToSAPDto> UpdateAsync(PostToSAPDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<PostToSAPListDto>> GetAllAsync(PagedPostToSAPResultRequestDto input);
        Task<PagedResultDto<PostToSAPListDto>> GetListAsync();
    }
}
