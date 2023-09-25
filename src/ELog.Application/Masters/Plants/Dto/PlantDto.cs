using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Plants.Dto
{
    [AutoMapFrom(typeof(PlantMaster))]
    public class PlantDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Plant Name is required.")]
        [StringLength(PMMSConsts.Medium)]
        public string PlantName { get; set; }

        [Required(ErrorMessage = "Plant Id is required.")]
        [StringLength(PMMSConsts.Medium)]
        public string PlantId { get; set; }

        public int? MasterPlantId { get; set; }

        [Required(ErrorMessage = "Plant type is required.")]
        public int? PlantTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? TaxRegistrationNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? License { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? GS1Prefix { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string? Address1 { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string? Address2 { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string? PostalCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? City { get; set; }

        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }

        [EmailAddress]
        [StringLength(PMMSConsts.Medium)]
        public string? Email { get; set; }

        [MaxLength(PMMSConsts.Small, ErrorMessage = "Phone Number maximum length exceeded.")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9]\.[^\s]{2,})$", ErrorMessage = "Website Url is not valid.")]
        [MaxLength(PMMSConsts.Medium, ErrorMessage = "Website maximum length exceeded.")]
        public string? Website { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}