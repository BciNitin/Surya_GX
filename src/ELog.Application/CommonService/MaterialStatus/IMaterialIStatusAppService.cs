using Abp.Application.Services;

using ELog.Application.CommonService.MaterialStatus.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.MaterialStatus
{
    public interface IMaterialIStatusAppService : IApplicationService
    {
        Task<List<MaterialStatusDto>> GetAllMaterilStatusListAsync();
        Task<MaterialDetailDto> GetMaterilDetailByIdAsync(int id, string sapBatchNo);
    }
}
