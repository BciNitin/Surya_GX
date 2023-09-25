using Abp.AutoMapper;

using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Equipments.Dto
{
    [AutoMapTo(typeof(EquipmentMaster))]
    public class CreateEquipmentDto
    {
        [Required(ErrorMessage = "Plant is required.")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Storage Location is required.")]
        public int? SLOCId { get; set; }

        [Required(ErrorMessage = "Equipment Type is required.")]
        public int? EquipmentTypeId { get; set; }

        [Required(ErrorMessage = "Equipment Code is required.")]
        [MaxLength(64, ErrorMessage = "Equipment Code maximum length exceeded.")]
        public string EquipmentCode { get; set; }

        [MaxLength(64, ErrorMessage = "Name maximum length exceeded.")]
        public string Name { get; set; }

        [MaxLength(64, ErrorMessage = "Alias maximum length exceeded.")]
        public string Alias { get; set; }

        public string EquipmentModel { get; set; }
        public string Description { get; set; }
        public bool? IsPortable { get; set; }
        public DateTime? DateOfProcurement { get; set; }
        public DateTime? DateOfInstallation { get; set; }
        public bool? IsMaintenanceRequired { get; set; }
        public int? MaintenanceScheduleDays { get; set; }
        public int? CommunicationType { get; set; }

        [MaxLength(64, ErrorMessage = "Vendor Name maximum length exceeded.")]
        public string VendorName { get; set; }

        [MaxLength(64, ErrorMessage = "Vendor Document Number maximum length exceeded.")]
        public string VendorDocumentNumber { get; set; }

        public DateTime? SupportExpiresOn { get; set; }

        [RegularExpression(@"^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$", ErrorMessage = "Network IP Address is not valid.")]
        public string NetworkIPAddress { get; set; }

        [Range(0, 65535, ErrorMessage = "Network IP Port is not valid.")]
        public int? NetworkIPPort { get; set; }

        public bool IsActive { get; set; }
        [Required(ErrorMessage = " Clean Hold Time is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Clean Hold Time  must be grater than zero.")]
        public int CleanHoldTime { get; set; }
    }
}