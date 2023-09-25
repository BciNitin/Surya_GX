using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.StandardWeightBoxes.Dto
{
    [AutoMapFrom(typeof(AreaMaster))]
    public class StandardWeightBoxDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Sub Plant is required.")]
        public int SubPlantId { get; set; }

        [Required(ErrorMessage = "Standard Weight Box Id is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Standard Weight Box Id maximum length exceeded.")]
        public string StandardWeightBoxId { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}