using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.CubicleAssignments
{
    public class PagedCubicleAssignmentResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? InspectionLotId { get; set; }
    }
}