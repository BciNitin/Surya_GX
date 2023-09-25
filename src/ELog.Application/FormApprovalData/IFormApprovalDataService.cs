using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.FormApprovalData.Dto;
using System.Threading.Tasks;

namespace ELog.Application.FormApprovalData
{
    public interface IFormApprovalDataService : IApplicationService
    {
        Task<FormApprovalDataDto> CreateAsync(FormApprovalDataDto input);

        //   Task<FormApprovalDataDto> GetAsync(EntityDto<int>input);
        Task<PagedResultDto<FormApprovalDataDto>> GetAllAsync(PagedFormApprovalDataResultRequestDto input);


    }
}



