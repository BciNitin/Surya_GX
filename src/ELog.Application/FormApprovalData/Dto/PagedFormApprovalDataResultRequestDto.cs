using Abp.Application.Services.Dto;

namespace ELog.Application.FormApprovalData.Dto
{
    public class PagedFormApprovalDataResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? Id { get; set; }
        public int? FormId { get; set; }
        public int? Status { get; set; }
        public string? Remark { get; set; }
    }
}
