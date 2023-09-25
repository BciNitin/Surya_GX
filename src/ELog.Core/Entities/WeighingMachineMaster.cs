using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeighingMachineMaster")]
    public class WeighingMachineMaster : PMMSFullAuditWithApprovalStatus
    {
        [StringLength(PMMSConsts.Small)]
        public string WeighingMachineCode { get; set; }

        [ForeignKey("SubPlantId")]
        public int? SubPlantId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string IPAddress { get; set; }

        public int? PortNumber { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public int? UnitOfMeasurementId { get; set; }

        public float? Capacity { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Make { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Modal { get; set; }

        public float? MinimumOperatingCapacity { get; set; }
        public float? MaximumOperatingCapacity { get; set; }
        public string LeastCount { get; set; }
        public int? LeastCountDigitAfterDecimal { get; set; }
        public DateTime? StampingDoneOn { get; set; }
        public DateTime? StampingDueOn { get; set; }
        public float? EccentricityAcceptanceValue { get; set; }
        [StringLength(PMMSConsts.Max)]
        public string EccentricityInstruction { get; set; }
        public float? LinearityAcceptanceValue { get; set; }


        #region Eccentricity
        public float? EccentricityAcceptanceMinValue { get; set; }
        public float? EccentricityAcceptanceMaxValue { get; set; }

        #endregion

        #region Repeatability
        public float? RepeatabilityAcceptanceMinValue { get; set; }
        public float? RepeatabilityAcceptanceMaxValue { get; set; }
        #endregion

        #region Linearity
        public float? LinearityAcceptanceValueWg1 { get; set; }
        public float? LinearityAcceptanceValueWg2 { get; set; }
        public float? LinearityAcceptanceValueWg3 { get; set; }
        public float? LinearityAcceptanceValueWg4 { get; set; }
        public float? LinearityAcceptanceValueWg5 { get; set; }

        #endregion


        #region Linearity Min
        public float? LinearityAcceptanceMinValueWg1 { get; set; }
        public float? LinearityAcceptanceMinValueWg2 { get; set; }
        public float? LinearityAcceptanceMinValueWg3 { get; set; }
        public float? LinearityAcceptanceMinValueWg4 { get; set; }
        public float? LinearityAcceptanceMinValueWg5 { get; set; }
        #endregion

        #region Linearity Max
        public float? LinearityAcceptanceMaxValueWg1 { get; set; }
        public float? LinearityAcceptanceMaxValueWg2 { get; set; }
        public float? LinearityAcceptanceMaxValueWg3 { get; set; }
        public float? LinearityAcceptanceMaxValueWg4 { get; set; }
        public float? LinearityAcceptanceMaxValueWg5 { get; set; }
        #endregion




        [StringLength(PMMSConsts.Max)]
        public string LinearityInstruction { get; set; }
        public float? RepeatabilityAcceptanceValue { get; set; }
        [StringLength(PMMSConsts.Max)]
        public string RepeatabilityInstruction { get; set; }
        public float? UncertaintyAcceptanceValue { get; set; }
        [StringLength(PMMSConsts.Max)]
        public string UncertaintyInstruction { get; set; }
        public float? PercentageRSDValue { get; set; }
        public float? StandardDeviationValue { get; set; }
        public float? MeanValue { get; set; }
        public float? MeanMinimumValue { get; set; }
        public float? MeanMaximumValue { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Formula { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public int? FrequencyTypeId { get; set; }
        public string RefrenceSOPNo { get; set; }
        public string FormatNo { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? TenantId { get; set; }
        public int? BalancedTypeId { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<CalibrationFrequencyMaster> Calibrations { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<WeightCaptureDetail> WeightCaptureDetails { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<WMCalibrationHeader> WMCalibrationHeaders { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<WMCalibratedLatestMachineDetail> WMCalibratedLatestMachineDetails { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<WeighingMachineTestConfiguration> WeighingMachineTestConfigurations { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<DispensingDetail> DispensingDetails { get; set; }

        [ForeignKey("WeighingMachineId")]
        public ICollection<SampleDestruction> SampleDestructions { get; set; }
    }
}