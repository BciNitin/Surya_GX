using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.GRNs
{
    public class PagedGRNPostingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? InvoiceId { get; set; }
        public int? PurchaseOrderId { get; set; }
    }
}