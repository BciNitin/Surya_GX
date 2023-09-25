using Abp.Application.Services.Dto;

using ELog.Application.Inward.PutAways.Dto;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Inward.BinToBinTransfers
{
    public interface IBinToBinTransferAppService
    {
        Task<List<PutAwayBinToBinTransferDto>> GetAsync(Guid? materialToBinId, int palletToBinId);
        Task<PagedResultDto<PutAwayBinToBinTransferListDto>> GetAllAsync(PagedPutAwayBinToBinTransferResultRequestDto input);
    }
}