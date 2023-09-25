using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StandardWeightBoxMaster")]
    public class StandardWeightBoxMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantMaster")]
        public int SubPlantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string StandardWeightBoxId { get; set; }

        [ForeignKey("AreaMaster")]
        public int AreaId { get; set; }

        [ForeignKey("DepartmentMaster")]
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("StandardWeightBoxMasterId")]
        public ICollection<StandardWeightMaster> StandardWeightMasters { get; set; }

        [ForeignKey("StandardWeightBoxId")]
        public ICollection<WMCalibrationDetail> WMCalibrationDetails { get; set; }

        [ForeignKey("CValueStandardWeightBoxId")]
        public ICollection<WMCalibrationEccentricityTest> WMCValueCalibrationEccentricityTests { get; set; }

        [ForeignKey("LFValueStandardWeightBoxId")]
        public ICollection<WMCalibrationEccentricityTest> WMLFValueCalibrationEccentricityTests { get; set; }

        [ForeignKey("RFValueStandardWeightBoxId")]
        public ICollection<WMCalibrationEccentricityTest> WMRFValueCalibrationEccentricityTests { get; set; }

        [ForeignKey("LBValueStandardWeightBoxId")]
        public ICollection<WMCalibrationEccentricityTest> WMLBValueCalibrationEccentricityTests { get; set; }

        [ForeignKey("RBValueStandardWeightBoxId")]
        public ICollection<WMCalibrationEccentricityTest> WMRBValueCalibrationEccentricityTests { get; set; }

        [ForeignKey("WeightValue1StandardWeightBoxId")]
        public ICollection<WMCalibrationLinearityTest> WMWeightValue1CalibrationLinearityTests { get; set; }

        [ForeignKey("WeightValue2StandardWeightBoxId")]
        public ICollection<WMCalibrationLinearityTest> WMWeightValue2CalibrationLinearityTests { get; set; }

        [ForeignKey("WeightValue3StandardWeightBoxId")]
        public ICollection<WMCalibrationLinearityTest> WMWeightValue3CalibrationLinearityTests { get; set; }

        [ForeignKey("WeightValue4StandardWeightBoxId")]
        public ICollection<WMCalibrationLinearityTest> WMWeightValue4CalibrationLinearityTests { get; set; }

        [ForeignKey("WeightValue5StandardWeightBoxId")]
        public ICollection<WMCalibrationLinearityTest> WMWeightValue5CalibrationLinearityTests { get; set; }

        [ForeignKey("WeightValue1StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue1CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue2StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue2CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue3StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue3CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue4StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue4CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue5StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue5CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue6StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue6CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue7StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue7CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue8StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue8CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue9StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue9CalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WeightValue10StandardWeightBoxId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMWeightValue10CalibrationRepeatabilityTests { get; set; }
    }
}