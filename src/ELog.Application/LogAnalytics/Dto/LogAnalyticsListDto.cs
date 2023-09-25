using System.Collections.Generic;

namespace ELog.Application.LogAnalytics.Dto
{
    public class LogAnalyticsListDto
    {
        //public LogAnalyticsListDto(int Count, int totalActiveForm, int totalInactiveForms, int totalApprovalCount, int totalDisApproveCount) { }

        public int Count { get; set; }
        public int totalActiveForm { get; set; }
        public int totalInactiveForms { get; set; }
        public int totalApprovalCount { get; set; }
        public int totalDisApproveCount { get; set; }

        public List<LogAnalyticsDto> logAnalytics { get; set; }
    }
}

