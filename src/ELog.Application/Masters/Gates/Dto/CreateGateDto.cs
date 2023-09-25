using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Gates.Dto
{
    [AutoMapTo(typeof(GateMaster))]
    public class CreateGateDto
    {
        [Required(ErrorMessage = "Plant is required.")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Gate Code is required.")]
        [MaxLength(PMMSConsts.Small, ErrorMessage = "Gate Code maximum length exceeded.")]
        public string GateCode { get; set; }

        [Required(ErrorMessage = "Gate Name is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Name maximum length exceeded.")]
        public string Name { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string AliasName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}