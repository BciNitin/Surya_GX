using Abp.Application.Services.Dto;

using System;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationListDto : EntityDto<int>
    {
        public int? WeighingMachineId { get; set; }

        public int? CalibrationFrequencyId { get; set; }
        public int? CalibrationStatusId { get; set; }
        public DateTime? CalibrationTestDate { get; set; }
        public int? SubPlantId { get; set; }

        public string WeighingMachineCode { get; set; }
        public string UserEnteredCalibrationFrequency { get; set; }
        public string UserEnteredCalibrationStatus { get; set; }
    }
}