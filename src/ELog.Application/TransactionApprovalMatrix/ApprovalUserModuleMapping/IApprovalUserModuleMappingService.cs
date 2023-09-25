using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto;

using System.Threading.Tasks;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping
{
    public interface IApprovalUserModuleMappingService : IApplicationService
    {
        Task<ApprovalUserModuleMappingDto> CreateAsync(CreateApprovalUserModuleMappingDto input);
        Task<ApprovalUserModuleMappingDto> GetAsync(EntityDto<int> input);
        Task<ApprovalUserModuleMappingDto> UpdateAsync(ApprovalUserModuleMappingDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<ApprovalUserModuleMappingListDto>> GetAllAsync(PagedApprovalUserModuleMapRequestDto input);
    }
}
