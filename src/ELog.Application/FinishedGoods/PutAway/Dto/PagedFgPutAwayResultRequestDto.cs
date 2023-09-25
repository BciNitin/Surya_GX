using Abp.Application.Services.Dto;

namespace ELog.Application.FinishedGoods.PutAway.Dto
{
    public class PagedFgPutAwayResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? PalletId { get; set; }
        public string PalletBarcode { get; set; }
        public int PalletCount { get; set; }
        public int? LocationId { get; set; }
        public string LocationBarcode { get; set; }
        public bool? isActive { get; set; }
        public bool? isPicked { get; set; }


    }
}
