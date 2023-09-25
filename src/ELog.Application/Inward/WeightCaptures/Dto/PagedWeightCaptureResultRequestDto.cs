using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    public class PagedWeightCaptureResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? MaterialId { get; set; }
    }
}