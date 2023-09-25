﻿using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CheckpointMaster")]
    public class CheckpointMaster : PMMSFullAudit
    {
        [ForeignKey("InspectionChecklistId")]
        public int? InspectionChecklistId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string CheckpointName { get; set; }

        [Required]
        [ForeignKey("CheckpointTypeId")]
        public int CheckpointTypeId { get; set; }

        [Required]
        [ForeignKey("ModeId")]
        public int ModeId { get; set; }

        public int? TenantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ValueTag { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string AcceptanceValue { get; set; }

        [ForeignKey("CheckpointId")]
        public ICollection<VehicleInspectionDetail> VehicleInspectionDetails { get; set; }

        [ForeignKey("CheckPointId")]
        public ICollection<MaterialChecklistDetail> MaterialChecklistDetails { get; set; }

        [ForeignKey("CheckPointId")]
        public ICollection<WMCalibrationCheckpoint> WMCalibrationCheckpoints { get; set; }

        [ForeignKey("CheckPointId")]
        public ICollection<CubicleCleaningCheckpoint> CubicleCleaningCheckpoints { get; set; }
        [ForeignKey("CheckPointId")]
        public ICollection<LineClearanceCheckpoint> LineClearanceCheckpoints { get; set; }

        [ForeignKey("CheckpointId")]
        public ICollection<EquipmentUsageLogList> EquipmentUsageLogList { get; set; }

        [ForeignKey("CheckpointId")]
        public ICollection<AreaUsageListLog> AreaUsageListLog { get; set; }
    }
}