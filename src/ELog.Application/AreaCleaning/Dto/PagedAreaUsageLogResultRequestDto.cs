using Abp.Application.Services.Dto;

namespace ELog.Application.AreaCleaning.Dto
{
    public class PagedAreaUsageLogResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? ActivityID { get; set; }
        public int? CubicalId { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}
