using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Locations.Dto
{
    public class PagedLocationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string LocationCode { get; set; }

        public int? PlantId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}