using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Locations.Dto
{
    [AutoMapFrom(typeof(LocationMaster))]
    public class LocationDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Location Code is required.")]
        [MaxLength(PMMSConsts.Small, ErrorMessage = "Location Code maximum length exceeded.")]
        public string LocationCode { get; set; }

        [Required(ErrorMessage = "Storage Location Type is required.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Storage Location Type maximum length exceeded.")]
        public string StorageLocationType { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "Plant is required.")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }

        public string Zone { get; set; }
        public decimal? LocationTemperature { get; set; }

        public decimal? LocationTemperatureUL { get; set; }
        public int? TemperatureUnit { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SLOCType { get; set; }

        public int? LevelId { get; set; }
        public string Description { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}