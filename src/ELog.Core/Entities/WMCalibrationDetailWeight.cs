using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationDetailWeights")]
    public class WMCalibrationDetailWeight : PMMSFullAudit
    {
        public int? KeyTypeId { get; set; }
        public int? CapturedWeightKeyTypeId { get; set; }

        [ForeignKey("WMCalibrationDetailId")]
        public int? WMCalibrationDetailId { get; set; }

        [ForeignKey("WMCalibrationEccentricityTestId")]
        public int? WMCalibrationEccentricityTestId { get; set; }

        [ForeignKey("WMCalibrationLinearityTestId")]
        public int? WMCalibrationLinearityTestId { get; set; }

        [ForeignKey("WMCalibrationRepeatabilityTestId")]
        public int? WMCalibrationRepeatabilityTestId { get; set; }

        [ForeignKey("StandardWeightId")]
        public int? StandardWeightId { get; set; }
    }
}