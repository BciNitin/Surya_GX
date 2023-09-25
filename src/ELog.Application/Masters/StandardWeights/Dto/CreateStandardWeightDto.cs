using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.StandardWeights.Dto
{
    [AutoMapTo(typeof(StandardWeightMaster))]
    public class CreateStandardWeightDto
    {
        [Required(ErrorMessage = "Sub Plant is required.")]
        public int SubPlantId { get; set; }

        [Required(ErrorMessage = "Standard Weight Id is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Standard Weight Id maximum length exceeded.")]
        public string StandardWeightId { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Capacity is not valid.")]
        public float? Capacity { get; set; }

        public string CapacityinDecimal { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        public int AreaId { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Standard Weight Box is required.")]
        public int StandardWeightBoxMasterId { get; set; }
        [Required(ErrorMessage = "Stamping Done On is required.")]
        public DateTime StampingDoneOn { get; set; }
        [Required(ErrorMessage = "Stamping Due On is required.")]
        public DateTime StampingDueOn { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}