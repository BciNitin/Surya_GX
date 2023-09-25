using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationHeaders")]
    public class WMCalibrationHeader : PMMSFullAudit
    {
        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }

        [ForeignKey("CalibrationFrequencyId")]
        public int? CalibrationFrequencyId { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public int? ChecklistTypeId { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public int? InspectionChecklistId { get; set; }

        [ForeignKey("CalibrationStatusId")]
        public int? CalibrationStatusId { get; set; }

        public bool? IsReCalibrated { get; set; }

        public string InitialZeroReading { get; set; }

        public string ReCalibrationRemark { get; set; }
        public DateTime CalibrationTestDate { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationCheckpoint> WMCalibrationCheckpoints { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationDetail> WMCalibrationDetails { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationEccentricityTest> WMCalibrationEccentricityTests { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationLinearityTest> WMCalibrationLinearityTests { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationRepeatabilityTest> WMCalibrationRepeatabilityTests { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibrationUncertainityTest> WMCalibrationUncertainityTests { get; set; }

        [ForeignKey("WMCalibrationHeaderId")]
        public ICollection<WMCalibratedLatestMachineDetail> WMCalibratedLatestMachineDetails { get; set; }
    }
}