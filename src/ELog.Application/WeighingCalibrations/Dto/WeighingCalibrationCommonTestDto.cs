using System.Collections.Generic;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationCommonTestDto
    {
        public int WeighingMachineId { get; set; }
        public string WeighingMachineCode { get; set; }
        public string CalculatedCapacityWeight { get; set; }

        public string MeanWeightRange { get; set; }

        public string UserEnteredUnitOfMeasurement { get; set; }

        public string EccentricityInstruction { get; set; }

        public string LinearityInstruction { get; set; }
        public string RepeatabilityInstruction { get; set; }
        public string UncertainityInstruction { get; set; }
        public float? AcceptableMeanValue { get; set; }
        public float? AcceptableStandardDeviationValue { get; set; }
        public float? AcceptablePRSDValue { get; set; }
        public float? AcceptableUncertainityValue { get; set; }
        public List<CalibrationDto> lstCalibrations { get; set; }
    }

    public class CalibrationDto
    {
        public int CalibrationLevelId { get; set; }
        public string UserEnteredCalibrationLevel { get; set; }
        public string WeightRange { get; set; }
        public string StandardWeight { get; set; }
    }
}