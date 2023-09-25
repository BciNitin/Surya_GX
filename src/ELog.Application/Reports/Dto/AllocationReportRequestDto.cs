using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class AllocationReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public List<int?> AreaIds { get; set; }
        public List<string?> MaterialCodes { get; set; }
        public List<string> SapBatchNos { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> StockStatus { get; set; }
    }
}