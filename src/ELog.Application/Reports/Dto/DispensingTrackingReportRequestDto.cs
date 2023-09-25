using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class DispensingTrackingReportRequestDto
    {
        public int? PlantId { get; set; }
        public List<int?> LstProcessOrderId { get; set; }
        public List<string> LstSAPBatchNo { get; set; }
        public List<int?> LstWeighingMachineId { get; set; }
        public bool IsSampling { get; set; }
        public List<string> LstMaterialId { get; set; }
        public List<string> LstProductCode { get; set; }
        public List<string> LstProductBatch { get; set; }
    }
}