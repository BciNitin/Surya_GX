using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class CubicleAssignedReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantIds { get; set; }
        public List<int?> CubicleIds { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<string> BatchNos { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> GroupStatusIds { get; set; }
        public bool? IsSampling { get; set; }
    }
}