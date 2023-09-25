using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.ChecklistTypes.Dto
{
    [AutoMapFrom(typeof(ChecklistTypeMaster))]
    public class ChecklistTypeListDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Small)]
        public string ChecklistTypeCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ChecklistName { get; set; }

        public int? SubPlantId { get; set; }

        public int? SubModuleId { get; set; }

        public bool IsActive { get; set; }
        public string UserEnteredPlantId { get; set; }
        public string UserEnteredSubModuleName { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}