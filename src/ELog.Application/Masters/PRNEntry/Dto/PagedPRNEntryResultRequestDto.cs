using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.PRNEntry.Dto
{
    public class PagedPRNEntryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public int? ModuleId { get; set; }
        public int? SubModuleId { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}
