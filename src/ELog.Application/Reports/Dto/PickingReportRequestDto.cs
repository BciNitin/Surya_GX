using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class PickingReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public List<int?> ProcessOrderIds { get; set; }
        public List<int?> InspectionLotIds { get; set; }
        public List<String> MaterialCodes { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<string> SapBatchNos { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> StatusIds { get; set; }
        public bool? IsSampling { get; set; }
    }
}