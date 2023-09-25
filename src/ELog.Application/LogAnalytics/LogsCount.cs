using ELog.Application.LogAnalytics.Dto;
using System.Collections.Generic;

namespace ELog.Application.LogAnalytics
{
    public class LogsCount
    {
        public int TotalActive { get; set; }
        public int TotalInActive { get; set; }
        public List<LogAnalyticsDto> logAnalyticsDto { get; set; }
    }
}
