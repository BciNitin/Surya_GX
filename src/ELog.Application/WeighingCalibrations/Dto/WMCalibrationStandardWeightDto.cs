namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WMCalibrationStandardWeightDto
    {
        public int StandardWeightId { get; set; }
        public string UserEnteredStandardWeightId { get; set; }
    }

    public class WMCalibrationInternalStandardWeightDto
    {
        public int? WMCalibrationDetailTestId { get; set; }
        public int? WMCalibrationEccentricityTestId { get; set; }
        public int? WMCalibrationLinearityTestId { get; set; }
        public int? WMCalibrationRepeatabilityTestId { get; set; }
        public int? StandardWeightId { get; set; }
        public int? CapturedWeightKeyTypeId { get; set; }
        public string UserEnteredStandardWeight { get; set; }
    }
}