using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Areas.Dto
{
    [AutoMapFrom(typeof(AreaMaster))]
    public class AreaDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Sub Plant is required.")]
        public int SubPlantId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string AreaCode { get; set; }

        [Required(ErrorMessage = "Area Name is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Area Name maximum length exceeded.")]
        public string AreaName { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string Zone { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}