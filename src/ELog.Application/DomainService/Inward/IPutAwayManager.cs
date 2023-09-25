using Abp.Domain.Services;

using ELog.Application.Inward.PutAways.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.DomainService.Inward
{
    public interface IPutAwayManager : IDomainService
    {
        Task UnloadBinAsync(int putAwayId, Guid? transationId);
        Task<List<PutAwayBinToBinTransferDto>> GetPutAwayAsync(Guid? materialToBinId, int palletToBinId);
        IQueryable<PutAwayBinToBinTransferListDto> CreateUserListFilteredQuery(PagedPutAwayBinToBinTransferResultRequestDto input, int transferPalletToBinId, int materialToBinId);
        List<PutAwayBinToBinTransferListDto> ApplyGroupingOnPutAwayList(List<PutAwayBinToBinTransferListDto> query);
        IQueryable<PutAwayBinToBinTransferListDto> ApplySorting(IQueryable<PutAwayBinToBinTransferListDto> query, PagedPutAwayBinToBinTransferResultRequestDto input);
        IEnumerable<PutAwayBinToBinTransferListDto> ApplyPaging(IEnumerable<PutAwayBinToBinTransferListDto> query, PagedPutAwayBinToBinTransferResultRequestDto input);
        Task MapPalletAndMaterialDetails(CreatePutAwayBinToBinTransferDto input, PutAwayBinToBinTransferDto result, int transferType);
    }
}