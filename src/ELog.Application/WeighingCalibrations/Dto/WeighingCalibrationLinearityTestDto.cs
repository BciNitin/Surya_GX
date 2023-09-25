using Abp.Application.Services.Dto;

using System.Collections.Generic;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationLinearityTestDto : EntityDto<int>
    {
        public int? WMCalibrationHeaderId { get; set; }
        public int? StandardWeightBoxId { get; set; }
        public string LinearityInstruction { get; set; }
        public string RepeatabilityInstruction { get; set; }
        public string UncertainityInstruction { get; set; }
        public double WeightValue1 { get; set; }
        public double WeightValue2 { get; set; }
        public double WeightValue3 { get; set; }
        public double WeightValue4 { get; set; }
        public double WeightValue5 { get; set; }
        public double MeanValue { get; set; }
        public string InitialZeroReading { get; set; }
        public string MeanWeightRange { get; set; }
        public double StandardDeviationValue { get; set; }
        public double PRSDValue { get; set; }
        public int? TestResultId { get; set; }
        public int? WeighingMachineId { get; set; }
        public List<int> lstWeightId { get; set; }
        public string WeighingMachineCode { get; set; }
        public string UserEnteredWeightBox { get; set; }
        public string UserEnteredWeightValue1StandardWeightBox { get; set; }
        public string UserEnteredWeightValue2StandardWeightBox { get; set; }
        public string UserEnteredWeightValue3StandardWeightBox { get; set; }
        public string UserEnteredWeightValue4StandardWeightBox { get; set; }
        public string UserEnteredWeightValue5StandardWeightBox { get; set; }
        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }
        public string SpriritLevelBubble { get; set; }
        public int? WeightValue1StandardWeightBoxId { get; set; }
        public int? WeightValue2StandardWeightBoxId { get; set; }
        public int? WeightValue3StandardWeightBoxId { get; set; }
        public int? WeightValue4StandardWeightBoxId { get; set; }
        public int? WeightValue5StandardWeightBoxId { get; set; }

        public List<WMCalibrationStandardWeightDto> lstWeightValue1StandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstWeightValue2StandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstWeightValue3StandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstWeightValue4StandardWeight { get; set; }
        public List<WMCalibrationStandardWeightDto> lstWeightValue5StandardWeight { get; set; }
    }
}