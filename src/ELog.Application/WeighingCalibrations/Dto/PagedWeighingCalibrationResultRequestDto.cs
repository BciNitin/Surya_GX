using Abp.Application.Services.Dto;

using System;

namespace ELog.Application.WeighingCalibrations
{
    public class PagedWeighingCalibrationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? CalibrationStatusId { get; set; }
        public DateTime? CalibrationDate { get; set; }
        public string Keyword { get; set; }
    }
}