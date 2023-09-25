using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.FinishedGoods.PutAway.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.PutAway
{
    public interface IFgPutAwayService : IApplicationService
    {
        Task<FgPutAwayDto> CreateAsync(FgPutAwayDto input);
        Task<FgPutAwayDto> GetAsync(EntityDto<int> input);
        Task<FgPutAwayDto> UpdateAsync(FgPutAwayDto input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PagedResultDto<FgPutAwayListDto>> GetAllAsync(PagedFgPutAwayResultRequestDto input);
        Task<List<FgPutAwayDto>> CreateBulkAsync(List<FgPutAwayDto> input);
        Task UnloadBinAsync(int putAwayId, Guid? transactionId);
        Task<PagedResultDto<FgPutAwayListDto>> GetListAsync();
        Task<PagedResultDto<FgPutAwayListDto>> GetPendingListAsync();

        Task<FgPutAwayListDto> GetSuggestedLocationAsync(string ProductBatchNo);

    }
}
