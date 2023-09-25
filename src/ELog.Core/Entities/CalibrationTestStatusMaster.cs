using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CalibrationTestStatusMaster")]
    public class CalibrationTestStatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string StatusName { get; set; }

        [ForeignKey("TestResultId")]
        public ICollection<WMCalibrationEccentricityTest> WMCalibrationEccentricityTests { get; set; }

        [ForeignKey("TestResultId")]
        public ICollection<WMCalibrationLinearityTest> WMCalibrationLinearityTests { get; set; }

        [ForeignKey("TestResultId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMCalibrationRepeatabilityTests { get; set; }

        [ForeignKey("TestResultId")]
        public ICollection<WMCalibrationUncertainityTest> WMCalibrationUncertainityTests { get; set; }
    }
}