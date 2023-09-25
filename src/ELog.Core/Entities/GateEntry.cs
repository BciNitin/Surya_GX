using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GateEntry")]
    public class GateEntry : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Small)]
        public string GatePassNo { get; set; }

        public int PrintCount { get; set; }
        public int? TenantId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("DeviceMaster")]
        public int? PrinterId { get; set; }

        [ForeignKey("InvoiceId")]
        public int? InvoiceId { get; set; }

        [ForeignKey("GateEntryId")]
        public ICollection<VehicleInspectionHeader> VehicleInspectionHeaders { get; set; }

        [ForeignKey("GateEntryId")]
        public ICollection<MaterialInspectionHeader> MaterialInspectionHeaders { get; set; }
    }
}