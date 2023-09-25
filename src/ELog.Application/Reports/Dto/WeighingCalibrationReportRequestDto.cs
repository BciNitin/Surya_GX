using Abp.Application.Services.Dto;

using System;

namespace ELog.Application.Reports.Dto
{
    public class WeighingCalibrationReportRequestDto : EntityDto<int>
    {
        public DateTime CalibrationStartDate { get; set; }
        public DateTime CalibrationEndDate { get; set; }
        public DateTime CalibrationDate { get; set; }
        public int? WeighingMachineId { get; set; }
        public int? SubPlantId { get; set; }
        public int? FrequencyModeld { get; set; }
    }
}