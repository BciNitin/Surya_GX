using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StandardWeightMaster")]
    public class StandardWeightMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantMaster")]
        public int SubPlantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string StandardWeightId { get; set; }

        public float? Capacity { get; set; }

        public string CapacityinDecimal { get; set; }
        public DateTime StampingDoneOn { get; set; }
        public DateTime StampingDueOn { get; set; }
        [ForeignKey("AreaMaster")]
        public int AreaId { get; set; }

        [ForeignKey("DepartmentMaster")]
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("StandardWeightBoxMaster")]
        public int? StandardWeightBoxMasterId { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public int? UnitOfMeasurementId { get; set; }

        [ForeignKey("StandardWeightId")]
        public ICollection<WMCalibrationDetailWeight> WMCalibrationDetailWeights { get; set; }
    }
}