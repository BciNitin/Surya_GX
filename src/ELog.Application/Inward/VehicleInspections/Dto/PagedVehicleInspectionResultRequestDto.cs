using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.VehicleInspections
{
    public class PagedVehicleInspectionResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? TransactionStatusId { get; set; }
        public int? PurchaseOrderId { get; set; }
    }
}