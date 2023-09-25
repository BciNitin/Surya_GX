using Abp.Application.Services.Dto;

namespace ELog.Application.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? ApprovalStatusId { get; set; }
        public int? Status { get; set; }
    }
}