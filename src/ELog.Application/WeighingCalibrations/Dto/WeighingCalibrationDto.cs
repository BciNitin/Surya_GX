using Abp.Application.Services.Dto;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Masters.WeighingMachines.Dto;
using ELog.Application.SelectLists.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationDto : EntityDto<int>
    {
        public string WeighingMachineCode { get; set; }
        public int? WeighingMachineId { get; set; }
        public int FrequencyModeld { get; set; }
        public string UserEnteredFrequencyMode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public float? Capacity { get; set; }
        public string UserEnteredUnitOfMeasurement { get; set; }
        public string InitialZeroReading { get; set; }
        public int? UnitOfMeasurement { get; set; }
        public string EccentricityInstruction { get; set; }
        public string LeastCount { get; set; }

        public string RefrenceSOPNo { get; set; }
        public string FormatNo { get; set; }
        public string Version { get; set; }
        public int? LeastCountDigitAfterDecimal { get; set; }
        public int? ChecklistTypeId { get; set; }
        public int? InspectionChecklistId { get; set; }
        public bool? IsReCalibrated { get; set; }
        public bool IsAllCalibrationLevelFinished { get; set; }
        public bool IsCheckpointFinished { get; set; }

        public string ReCalibrationRemark { get; set; }

        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }
        public string SpriritLevelBubble { get; set; }
        public int WeighingCalibrationSaveType { get; set; }
        public int? CalibrationStatusId { get; set; }
        public int? UncertainityTestResultId { get; set; }
        public string UncertainityTestResult { get; set; }
        public double UncertainityValue { get; set; }
        public string LinearityInstruction { get; set; }
        public string RepeatabilityInstruction { get; set; }
        public string UncertainityInstruction { get; set; }
        public int UncertainityTestId { get; set; }
        public float AcceptableUncertainityValue { get; set; }
        public string CalibrationStatus { get; set; }
        public DateTime CalibrationTestDate { get; set; }
        public List<SelectListDto> calibrationFrequencySelectListDtos { get; set; }
        public List<CalibrationFrequencyDto> calibrationFrequencyDtos { get; set; }
        public List<CheckpointDto> WeighingCalibrationCheckpoints { get; set; }
        public List<WeighingCalibrationDetailDto> lstWeighingCalibrationDetailDto { get; set; }
        public WeighingCalibrationDetailDto WeighingCalibrationDetailCurrentDto { get; set; }
        public WeighingCalibrationEccentricityTestDto WeighingCalibrationEccentricityTestDto { get; set; }
        public WeighingCalibrationLinearityTestDto WeighingCalibrationLinearityTestDto { get; set; }
        public WeighingCalibrationRepeatabilityTestDto WeighingCalibrationRepeatabilityTestDto { get; set; }
        public List<WeighingMachineTestconfigurationDto> WeighingMachineTestConfigurations { get; set; }



        #region Linearity
        public float? LinearityAcceptanceValueWg1 { get; set; }
        public float? LinearityAcceptanceValueWg2 { get; set; }
        public float? LinearityAcceptanceValueWg3 { get; set; }
        public float? LinearityAcceptanceValueWg4 { get; set; }
        public float? LinearityAcceptanceValueWg5 { get; set; }

        #endregion


        #region Eccentricity
        public float? EccentricityAcceptanceMinValue { get; set; }
        public float? EccentricityAcceptanceMaxValue { get; set; }

        #endregion

        #region Repeatability
        public float? RepeatabilityAcceptanceMinValue { get; set; }
        public float? RepeatabilityAcceptanceMaxValue { get; set; }
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


    }
}