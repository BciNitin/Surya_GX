using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.PRNEntry.Dto
{
    [AutoMapFrom(typeof(PRNEntryMaster))]
    public class PRNEntryDto : EntityDto<int>
    {
        [Required(ErrorMessage = "PRN File Name is required.")]
        public string PRNFileName { get; set; }

        [Required(ErrorMessage = "Plant is required.")]
        public int SubPlantId { get; set; }
        public int? TenantId { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "SubModule is required.")]
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }

        public string UserEnteredApprovalStatus { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}
