using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Z.Dto
{
    public class PagedZResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string ZField { get; set; }
        public string DescriptionField { get; set; }
    }
}