using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Cubicles.Dto
{
    [AutoMapTo(typeof(CubicleMaster))]
    public class CreateCubicleDto
    {
        [Required(ErrorMessage = "Plant is required.")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Cubicle Code is required.")]
        [MaxLength(PMMSConsts.Small, ErrorMessage = "Cubicle Code maximum length exceeded.")]
        public string CubicleCode { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "Storage Location is required.")]
        public int? SLOCId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}