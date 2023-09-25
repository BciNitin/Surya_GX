using Abp.Application.Services.Dto;



namespace ELog.Application.LogAnalytics.Dto
{
    public class PagedLogAnalyticsResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public string? FormName { get; set; }
        public bool? IsActive { get; set; }
        public int Count { get; set; }
        public int totalActiveForm { get; set; }
        public int totalInactiveForms { get; set; }
        public int totalApprovalCount { get; set; }
        public int totalDisApproveCount { get; set; }

    }
}
