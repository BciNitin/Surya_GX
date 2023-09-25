using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Masters.WeighingMachines.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class CreateWeighingCalibrationDto
    {
        public string WeighingMachineCode { get; set; }
        public int? WeighingMachineId { get; set; }
        public int FrequencyModeld { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public float? Capacity { get; set; }
        public string UserEnteredUnitOfMeasurement { get; set; }
        public string InitialZeroReading { get; set; }
        public int? UnitOfMeasurement { get; set; }
        public float? LeastCount { get; set; }
        public int? ChecklistTypeId { get; set; }
        public int? InspectionChecklistId { get; set; }
        public bool? IsReCalibrated { get; set; }
        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }

        public string SpriritLevelBubble { get; set; }
        public string ReCalibrationRemark { get; set; }
        public List<SelectListDto> calibrationFrequencySelectListDtos { get; set; }
        public List<CalibrationFrequencyDto> calibrationFrequencyDtos { get; set; }
        public List<CheckpointDto> WeighingCalibrationCheckpoints { get; set; }
        public List<WeighingCalibrationDetailDto> lstWeighingCalibrationDetailDto { get; set; }
        public WeighingCalibrationDetailDto WeighingCalibrationDetailCurrentDto { get; set; }
        public WeighingCalibrationEccentricityTestDto WeighingCalibrationEccentricityTestDto { get; set; }
        public WeighingCalibrationLinearityTestDto WeighingCalibrationLinearityTestDto { get; set; }
        public WeighingCalibrationRepeatabilityTestDto WeighingCalibrationRepeatabilityTestDto { get; set; }
    }
}