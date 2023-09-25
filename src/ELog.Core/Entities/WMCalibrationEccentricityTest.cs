using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationEccentricityTests")]
    public class WMCalibrationEccentricityTest : PMMSFullAudit
    {
        [ForeignKey("WMCalibrationHeaderId")]
        public int? WMCalibrationHeaderId { get; set; }

        public double CalculatedCapacityWeight { get; set; }

        public string InitialZeroReading { get; set; }

        public double CValue { get; set; }
        public double LFValue { get; set; }
        public double RFValue { get; set; }
        public double LBValue { get; set; }
        public double RBValue { get; set; }
        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }

        public string SpriritLevelBubble { get; set; }

        [ForeignKey("CValueStandardWeightBoxId")]
        public int? CValueStandardWeightBoxId { get; set; }

        [ForeignKey("LFValueStandardWeightBoxId")]
        public int? LFValueStandardWeightBoxId { get; set; }

        [ForeignKey("RFValueStandardWeightBoxId")]
        public int? RFValueStandardWeightBoxId { get; set; }

        [ForeignKey("LBValueStandardWeightBoxId")]
        public int? LBValueStandardWeightBoxId { get; set; }

        [ForeignKey("RBValueStandardWeightBoxId")]
        public int? RBValueStandardWeightBoxId { get; set; }

        public double MeanValue { get; set; }
        public double StandardDeviationValue { get; set; }
        public double PRSDValue { get; set; }

        [ForeignKey("TestResultId")]
        public int? TestResultId { get; set; }

        [ForeignKey("WMCalibrationEccentricityTestId")]
        public ICollection<WMCalibrationDetailWeight> WMCalibrationDetailWeights { get; set; }
    }
}