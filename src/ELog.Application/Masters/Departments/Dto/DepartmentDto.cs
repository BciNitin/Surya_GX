using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Departments.Dto
{
    [AutoMapFrom(typeof(DepartmentMaster))]
    public class DepartmentDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Sub Plant is required.")]
        public int SubPlantId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Department Name maximum length exceeded.")]
        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}