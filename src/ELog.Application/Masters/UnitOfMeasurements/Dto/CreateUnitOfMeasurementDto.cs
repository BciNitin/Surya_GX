using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.UnitOfMeasurements.Dto
{
    [AutoMapTo(typeof(UnitOfMeasurementMaster))]
    public class CreateUnitOfMeasurementDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Name maximum length exceeded.")]
        public string Name { get; set; }

        public string Description { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }

        [StringLength(PMMSConsts.Small)]

        [Required(ErrorMessage = "UOM is required.")]
        public string UnitOfMeasurement { get; set; }

        public bool IsActive { get; set; }
    }
}