using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.PutAways.Dto
{
    public class PagedPutAwayBinToBinTransferResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? PalletId { get; set; }
        public int? MaterialId { get; set; }
        public int? LocationId { get; set; }
    }
}