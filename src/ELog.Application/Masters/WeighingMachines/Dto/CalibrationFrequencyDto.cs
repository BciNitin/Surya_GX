using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    [AutoMap(typeof(CalibrationFrequencyMaster))]
    public class CalibrationFrequencyDto : EntityDto<int>
    {
        public int? WeighingMachineId { get; set; }

        public int? FrequencyTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string CalibrationLevel { get; set; }

        public string CalibrationCriteria { get; set; }

        public float? StandardWeightValue { get; set; }
        public float? MinimumValue { get; set; }
        public float? MaximumValue { get; set; }
    }
}