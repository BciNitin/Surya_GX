using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.UnitOfMeasurements.Dto
{
    [AutoMapFrom(typeof(UnitOfMeasurementMaster))]
    public class UnitOfMeasurementDto : EntityDto<int>
    {
        public string UOMCode { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Name maximum length exceeded.")]
        public string Name { get; set; }

        public string Description { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UnitOfMeasurement { get; set; }

        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}