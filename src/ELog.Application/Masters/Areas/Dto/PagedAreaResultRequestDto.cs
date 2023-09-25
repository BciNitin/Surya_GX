using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Areas.Dto
{
    public class PagedAreaResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public int? DepartmentId { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}