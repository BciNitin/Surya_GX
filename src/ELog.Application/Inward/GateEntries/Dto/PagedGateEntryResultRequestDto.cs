using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.GateEntries
{
    public class PagedGateEntryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}