using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.FinishedGoods.Picking.Dto;
using ELog.Application.FinishedGoods.SAP_FG.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.Picking
{
    public interface ILoadingService : IApplicationService
    {
        Task<int> GetOBDCodeIfAlreadyExist(string input);
        Task<LoadingDto> CreateAsync(LoadingDto input);
        Task<List<LoadingDto>> CreateBulkAsync(List<LoadingDto> input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<LoadingListDto>> GetListAsync();
        Task<LoadingDto> GetAsync(EntityDto<int> input);
        Task<LoadingDto> UpdateAsync(LoadingDto input);
        Task<OBDDetailDto> GetOBDDetailsAsync(string input);
        Task<List<OBDDetailDto>> GetOBDDetailsListAsync();
        Task<PagedResultDto<LoadingListDto>> GetAllAsync(PagedLoadingResultRequestDto input);
        Task<PagedResultDto<LoadingListDto>> GetPendingListAsync();
    }
}
