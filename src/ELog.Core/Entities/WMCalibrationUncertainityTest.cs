using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationUncertainityTests")]
    public class WMCalibrationUncertainityTest : PMMSFullAudit
    {
        [ForeignKey("WMCalibrationHeaderId")]
        public int? WMCalibrationHeaderId { get; set; }

        public double UncertainityValue { get; set; }

        [ForeignKey("TestResultId")]
        public int? TestResultId { get; set; }
    }
}