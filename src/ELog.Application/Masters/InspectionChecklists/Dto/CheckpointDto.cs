using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    [AutoMap(typeof(CheckpointMaster))]
    public class CheckpointDto : EntityDto<int>
    {
        public int? InspectionChecklistId { get; set; }

        [Required]
        public string CheckpointName { get; set; }

        [Required]
        public int CheckpointTypeId { get; set; }

        public string CheckpointTypeName { get; set; }
        public int GroupIndex { get; set; }

        [Required]
        public int ModeId { get; set; }

        public bool IsControllerMode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ValueTag { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string AcceptanceValue { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }

        public int? CheckPointId { get; set; }
        public int? PlantId { get; set; }
        public int? CheckListTypeId { get; set; }
    }
}