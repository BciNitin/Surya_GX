using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class LineClearanceReportResultDto : EntityDto<int>
    {
        public DateTime ClearanceDate { get; set; }
        public int? SubPlantId { get; set; }
        public string CheckpointName { get; set; }
        public string CheckpointType { get; set; }
        public string ValueTag { get; set; }
        public string AcceptanceValue { get; set; }
        public string UserdEnteredValue { get; set; }
        public string Remark { get; set; }
        public int? TransactionStatusId { get; set; }
        public string TransactionStatus { get; set; }
        public string MaterialCode { get; set; }
        public string ChecklistName { get; set; }
        public int MaterialId { get; set; }
        public int CheckPointId { get; set; }
        public string CubicleCode { get; set; }
        public int CubicleId { get; set; }
        public string GroupCode { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        public string SapBatchNo { get; set; }
        public bool? IsSampling { get; set; }
        public string CheckedBy { get; set; }
        public string DoneBy { get; set; }
        public int? VerifiedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int LineClearanceCheckPointId { get; set; }
        public List<VehicleInspectionCheckPointReportResultDto> CheckPoints { get; set; }
    }
}