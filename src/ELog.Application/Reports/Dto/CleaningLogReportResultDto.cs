using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
   public class CleaningLogReportResultDto : EntityDto<int>
    {
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string CleanerName { get; set; }
        public string VerifiedName { get; set; }
        public string CleaningType { get; set; }
        public int CleaningTypeId { get; set; }
        public int CubicleId { get; set; }
        public string CubicleCode { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentCode { get; set; }
        public string CheckpointName { get; set; }
        public string CheckpointType { get; set; }
        public string ValueTag { get; set; }
        public string AcceptanceValue { get; set; }
        public string UserdEnteredValue { get; set; }
        public string Remark { get; set; }
        public bool IsSampling { get; set; }
        public int? SubPlantId { get; set; }
        public string TransactionStatus { get; set; }
        public int? VerifiedBy { get; set; }
        public int? CleanerBy { get; set; }
        public DateTime CleaningDate { get; set; }
        public bool IsPortable { get; set; }
        public List<VehicleInspectionCheckPointReportResultDto> CheckPoints { get; set; }

    }
}
