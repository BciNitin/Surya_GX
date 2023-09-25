using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Departments.Dto
{
    public class PagedDepartmentResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public string DepartmentCode { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }

    }
}