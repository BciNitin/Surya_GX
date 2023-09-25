using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Calenders.Dto
{
    public class PagedCalenderResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? HolidayTypeId { get; set; }
        public int? ApprovalStatusId { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}