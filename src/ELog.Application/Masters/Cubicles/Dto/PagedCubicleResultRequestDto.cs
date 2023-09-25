using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Cubicles.Dto
{
    public class PagedCubicleResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PlantId { get; set; }
        public string CubicleCode { get; set; }
        public int? SLOCId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}