using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.ChecklistTypes.Dto
{
    [AutoMapFrom(typeof(ChecklistTypeMaster))]
    public class ChecklistTypeDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Small)]
        public string ChecklistTypeCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ChecklistName { get; set; }

        public int? SubPlantId { get; set; }

        public int? SubModuleId { get; set; }

        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}