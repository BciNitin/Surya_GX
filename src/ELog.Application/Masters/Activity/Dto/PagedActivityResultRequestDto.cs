using Abp.Application.Services.Dto;


namespace ELog.Application.Activity.Dto
{
    public class PagedActivityResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? ModuleId { get; set; }
        public int? SubModuleId { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}
