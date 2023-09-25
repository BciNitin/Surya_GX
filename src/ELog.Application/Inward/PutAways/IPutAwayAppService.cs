using Abp.Application.Services.Dto;

using ELog.Application.Inward.PutAways.Dto;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Inward.PutAways
{
    public interface IPutAwaysAppService
    {
        Task<List<PutAwayBinToBinTransferDto>> GetAsync(Guid? transationId, int putAwayId);

        Task<PagedResultDto<PutAwayBinToBinTransferListDto>> GetAllAsync(PagedPutAwayBinToBinTransferResultRequestDto input);

        Task<PutAwayBinToBinTransferDto> CreateAsync(CreatePutAwayBinToBinTransferDto input);
        Task UnloadBinAsync(int putAwayId, Guid? transationId);
    }
}
