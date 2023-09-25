using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    public class PagedInspectionChecklistResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? PlantId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}