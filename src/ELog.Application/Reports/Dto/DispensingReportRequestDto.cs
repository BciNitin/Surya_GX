using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class DispensingReportRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PlantId { get; set; }
        public List<int?> LstProcessOrderId { get; set; }
        public List<string> LstSAPBatchNo { get; set; }
        public List<int?> LstWeighingMachineId { get; set; }

        public List<string> LstMaterialId { get; set; }
        public List<string> LstProductCode { get; set; }
        public List<string> LstProductBatch { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}