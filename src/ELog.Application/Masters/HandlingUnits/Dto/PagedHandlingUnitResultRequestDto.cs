using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.HandlingUnits.Dto
{
    public class PagedHandlingUnitResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PlantId { get; set; }
        public string Keyword { get; set; }
        public int? HandlingUnitTypeId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}