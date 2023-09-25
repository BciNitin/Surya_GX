using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.Palletizations.Dto
{
    public class PagedPalletizationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? PalletId { get; set; }
        public int? MaterialId { get; set; }
    }
}