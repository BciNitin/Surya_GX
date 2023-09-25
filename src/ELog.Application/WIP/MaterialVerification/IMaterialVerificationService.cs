using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.WIP.MaterialVerification.Dto;
using System.Threading.Tasks;

namespace ELog.Application.WIP.MaterialVerification
{
    public interface IMaterialVerificationService : IApplicationService
    {
        Task<MaterialVerificationDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<MaterialVerificationListDto>> GetAllAsync(PagedMaterialVerificationResultRequestDto input);

        Task<MaterialVerificationDto> CreateAsync(CreateMaterialVerificationDto input);

        Task<MaterialVerificationDto> UpdateAsync(MaterialVerificationDto input);
        Task DeleteAsync(EntityDto<int> input);
    }
}
