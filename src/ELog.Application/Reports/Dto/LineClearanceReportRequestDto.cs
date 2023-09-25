using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class LineClearanceReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public List<string> MaterialListId { get; set; }
        public List<string> ProductBatchNos { get; set; }
        public List<string> ProductSAPBatchNos { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<int> CubicleListIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool? IsSampling { get; set; }
    }
}