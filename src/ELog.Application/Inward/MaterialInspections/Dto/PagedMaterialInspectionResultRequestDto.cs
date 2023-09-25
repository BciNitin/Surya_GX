using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.MaterialInspections
{
    public class PagedMaterialInspectionResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? TransactionStatusId { get; set; }
        public int? PurchaseOrderId { get; set; }
    }
}