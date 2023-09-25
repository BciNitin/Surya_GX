using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class DispensingTrackingReportInnerDto
    {
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }
        public int? PlantId { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        public string MaterialCodeId { get; set; }
        public string SAPBatchNumber { get; set; }
        public string GroupId { get; set; }
        public DateTime? DispensingTime { get; set; }
        public long? DispensingDoneBy { get; set; }
        public DateTime? CubicleAssignmentTime { get; set; }
        public long? CubicleAssignmentDoneBy { get; set; }
        public DateTime? LineClearanceTime { get; set; }
        public long? LineClearanceDoneBy { get; set; }
        public long? LineClearanceCheckBy { get; set; }
        public DateTime? PickingTime { get; set; }
        public long? PickingDoneBy { get; set; }
        public DateTime? PreStagingTime { get; set; }
        public long? PreStagingDoneBy { get; set; }
        public DateTime? StagingTime { get; set; }
        public long? StagingDoneBy { get; set; }
        public DateTime? StageOutTime { get; set; }
        public long? StageOutDoneBy { get; set; }
    }
}