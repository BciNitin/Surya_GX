using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class DispensingReportDto
    {
        public string UserEnteredPlantId { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        public string ItemCode { get; set; }
        public string SAPBatchNumber { get; set; }
        public bool IsSampling { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string WeighingMachineCode { get; set; }
        public string UserName { get; set; }
        public float? NoOfPacks { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public float? GrossWeight { get; set; }
        public int? PlantId { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? WeighingMachineId { get; set; }
        public string ProductBatch { get; set; }
    }
}