using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.HandlingUnits.Dto
{
    [AutoMapTo(typeof(HandlingUnitMaster))]
    public class CreateHandlingUnitDto
    {
        [Required(ErrorMessage = "Plant is required.")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Handling Unit Code is required.")]
        [MaxLength(PMMSConsts.Small, ErrorMessage = "Handling Unit Code maximum length exceeded.")]
        public string HUCode { get; set; }

        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Name maximum length exceeded.")]
        public string Name { get; set; }
        public int? HandlingUnitTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}