using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CalibrationFrequencyMaster")]
    public class CalibrationFrequencyMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public int? FrequencyTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string CalibrationLevel { get; set; }

        public string CalibrationCriteria { get; set; }

        public float? StandardWeightValue { get; set; }
        public float? MinimumValue { get; set; }
        public float? MaximumValue { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("CalibrationLevelId")]
        public ICollection<WMCalibrationDetail> WMCalibrationDetails { get; set; }
    }
}