using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    [AutoMapFrom(typeof(InspectionChecklistMaster))]
    public class InspectionChecklistDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Small)]
        public string ChecklistCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        public int PlantId { get; set; }

        public int SubModuleId { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        public string VersionString { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string FormatNumber { get; set; }

        [Required]
        public int ChecklistTypeId { get; set; }

        [Required]
        public int ModeId { get; set; }
        [Required]
        public List<CheckpointDto> Checkpoints { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}