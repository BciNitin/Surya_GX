
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("EquipmentMaster")]
    public class EquipmentMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantId")]
        public int PlantId { get; set; }

        [ForeignKey("SLOCId")]
        public int? SLOCId { get; set; }

        [ForeignKey("EquipmentTypeId")]
        public int? EquipmentTypeId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string EquipmentCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Alias { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string EquipmentModel { get; set; }

        public string Description { get; set; }
        public bool? IsPortable { get; set; }
        public DateTime? DateOfProcurement { get; set; }
        public DateTime? DateOfInstallation { get; set; }
        public bool? IsMaintenanceRequired { get; set; }
        public int? MaintenanceScheduleDays { get; set; }

        public int? CommunicationType { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string VendorName { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string VendorDocumentNumber { get; set; }

        public DateTime? SupportExpiresOn { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string NetworkIPAddress { get; set; }

        public int? NetworkIPPort { get; set; }

        public bool IsActive { get; set; }

        public int? TenantId { get; set; }

        [Required]
        public int CleanHoldTime { get; set; }

        [ForeignKey("EquipmentId")]
        public ICollection<EquipmentAssignment> EquipmentAssignments { get; set; }

        [ForeignKey("RLAFId")]
        public ICollection<DispensingHeader> DispensingHeaders { get; set; }
    }
}