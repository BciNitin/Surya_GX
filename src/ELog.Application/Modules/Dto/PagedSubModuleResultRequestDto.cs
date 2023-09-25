using Abp.Application.Services.Dto;

namespace ELog.Application.Modules.Dto
{
    public class PagedSubModuleResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? Status { get; set; }
        public int? ModuleId { get; set; }
        public int? ApprovalRequired { get; set; }
    }
}