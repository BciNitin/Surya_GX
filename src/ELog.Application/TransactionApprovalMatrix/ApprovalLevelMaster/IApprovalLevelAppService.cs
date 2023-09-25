using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto;

using System.Threading.Tasks;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel
{
    public interface IApprovalLevelAppService : IApplicationService
    {
        Task<ApprovalLevelDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<ApprovalLevelListDto>> GetAllAsync(PagedApprovalLevelResultRequestDto input);

        Task<ApprovalLevelDto> CreateAsync(CreateApprovalLevelDto input);

        Task<ApprovalLevelDto> UpdateAsync(ApprovalLevelDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<ApprovalLevelListDto>> GetListAsync();
    }
}