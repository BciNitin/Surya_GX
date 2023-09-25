using Abp.Application.Services.Dto;

using System.Collections.Generic;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationEccentricityTestDto : EntityDto<int>
    {
        public int? WMCalibrationHeaderId { get; set; }

        public double CalculatedCapacityWeight { get; set; }

        public string EccentricityInstruction { get; set; }

        public string LinearityInstruction { get; set; }
        public string RepeatabilityInstruction { get; set; }
        public string UncertainityInstruction { get; set; }
        public string UserEnteredUnitOfMeasurement { get; set; }
        public string InitialZeroReading { get; set; }
        public int? StandardWeightBoxId { get; set; }

        public double CValue { get; set; }
        public double LFValue { get; set; }
        public double RFValue { get; set; }
        public double LBValue { get; set; }
        public double RBValue { get; set; }
        public double MeanValue { get; set; }

        public string MeanWeightRange { get; set; }
        public double StandardDeviationValue { get; set; }
        public double PRSDValue { get; set; }

        public int? TestResultId { get; set; }
        public string TestResult { get; set; }

        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }
        public string SpriritLevelBubble { get; set; }
        public int? WeighingMachineId { get; set; }
        public List<int> lstWeightId { get; set; }
        public string WeighingMachineCode { get; set; }
        public string UserEnteredWeightBox { get; set; }
        public string UserEnteredCValueStandardWeightBox { get; set; }
        public string UserEnteredLFValueStandardWeightBox { get; set; }
        public string UserEnteredRFValueStandardWeightBox { get; set; }
        public string UserEnteredLBValueStandardWeightBox { get; set; }
        public string UserEnteredRBValueStandardWeightBox { get; set; }

        public int? CValueStandardWeightBoxId { get; set; }

        public int? LFValueStandardWeightBoxId { get; set; }

        public int? RFValueStandardWeightBoxId { get; set; }

        public int? LBValueStandardWeightBoxId { get; set; }

        public int? RBValueStandardWeightBoxId { get; set; }

        public List<WMCalibrationStandardWeightDto> lstCValueStandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstLFValueStandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstRFValueStandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstLBValueStandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstRBValueStandardWeight { get; set; }
    }
}