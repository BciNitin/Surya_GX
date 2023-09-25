using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class VehicleInspectionReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public List<int> PurchaseOrderId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> TransactionStatusId { get; set; }
        public List<int> MaterialListId { get; set; }
    }
}