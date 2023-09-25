using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.FinishedGoods.Picking.Dto;
using ELog.Application.FinishedGoods.SAP_FG.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.Picking
{

    public interface IFgPickingService : IApplicationService
    {
        Task<int> GetOBDCodeIfAlreadyExist(string input);
        Task<FgPickingDto> CreateAsync(FgPickingDto input);
        Task<List<FgPickingDto>> CreateBulkAsync(List<FgPickingDto> input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<FgPickingListDto>> GetListAsync();
        Task<FgPickingDto> GetAsync(EntityDto<int> input);
        Task<FgPickingDto> UpdateAsync(FgPickingDto input);
        Task<OBDDetailDto> GetOBDDetailsAsync(string input);
        Task<List<OBDDetailDto>> GetOBDDetailsListAsync();
        Task<PagedResultDto<FgPickingListDto>> GetAllAsync(PagedFgPickingResultRequestDto input);
        Task<PagedResultDto<FgPickingListDto>> GetPendingListAsync();
        Task<FgPickingDto> GetPickingOBDDetailsIfAlreadyExist(string input);
    }
}
