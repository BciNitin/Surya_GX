using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Departments.Dto
{
    [AutoMapTo(typeof(DepartmentMaster))]
    public class CreateDepartmentDto
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
    }
}