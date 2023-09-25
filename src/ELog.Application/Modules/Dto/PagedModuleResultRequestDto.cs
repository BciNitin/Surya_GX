using Abp.Application.Services.Dto;

namespace ELog.Application.Modules.Dto
{
    public class PagedModuleResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? Status { get; set; }
    }
}