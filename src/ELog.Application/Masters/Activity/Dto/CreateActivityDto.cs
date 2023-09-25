using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Activity.Dto
{

    [AutoMapTo(typeof(ActivityMaster))]
    public class CreateActivityDto
    {
        [Required(ErrorMessage = "Activity Name is required.")]
        public string ActivityName { get; set; }

        [Required(ErrorMessage = "Activity Code is required.")]
        public string ActivityCode { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "SubModule is required.")]
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }

    }
}
