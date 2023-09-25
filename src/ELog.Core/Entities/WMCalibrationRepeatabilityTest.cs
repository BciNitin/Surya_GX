﻿using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationRepeatabilityTests")]
    public class WMCalibrationRepeatabilityTest : PMMSFullAudit
    {
        [ForeignKey("WMCalibrationHeaderId")]
        public int? WMCalibrationHeaderId { get; set; }

        public string InitialZeroReading { get; set; }
        public double CalculatedCapacityWeight { get; set; }

        public double WeightValue1 { get; set; }
        public double WeightValue2 { get; set; }
        public double WeightValue3 { get; set; }
        public double WeightValue4 { get; set; }
        public double WeightValue5 { get; set; }
        public double WeightValue6 { get; set; }
        public double WeightValue7 { get; set; }
        public double WeightValue8 { get; set; }
        public double WeightValue9 { get; set; }
        public double WeightValue10 { get; set; }
        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }

        public string SpriritLevelBubble { get; set; }

        [ForeignKey("WeightValue1StandardWeightBoxId")]
        public int? WeightValue1StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue2StandardWeightBoxId")]
        public int? WeightValue2StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue3StandardWeightBoxId")]
        public int? WeightValue3StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue4StandardWeightBoxId")]
        public int? WeightValue4StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue5StandardWeightBoxId")]
        public int? WeightValue5StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue6StandardWeightBoxId")]
        public int? WeightValue6StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue7StandardWeightBoxId")]
        public int? WeightValue7StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue8StandardWeightBoxId")]
        public int? WeightValue8StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue9StandardWeightBoxId")]
        public int? WeightValue9StandardWeightBoxId { get; set; }

        [ForeignKey("WeightValue10StandardWeightBoxId")]
        public int? WeightValue10StandardWeightBoxId { get; set; }

        public double MeanValue { get; set; }
        public double StandardDeviationValue { get; set; }
        public double PRSDValue { get; set; }

        [ForeignKey("TestResultId")]
        public int? TestResultId { get; set; }

        [ForeignKey("WMCalibrationRepeatabilityTestId")]
        public ICollection<WMCalibrationDetailWeight> WMCalibrationDetailWeights { get; set; }
    }
}