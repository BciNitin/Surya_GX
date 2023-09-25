using Abp.Application.Services.Dto;


namespace ELog.Application.Pallet.Dto
{
    public class PagedPalletMasterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PalletCodeId { get; set; }
        public int? CartonBarcodeId { get; set; }

        //public string Description { get; set; }
        //public int? TenantId { get; set; }
        //public string ProductBatchNo { get; set; }
        public string Keyword { get; set; }
    }
}
