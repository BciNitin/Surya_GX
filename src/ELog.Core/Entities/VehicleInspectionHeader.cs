using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("VehicleInspectionHeaders")]
    public class VehicleInspectionHeader : PMMSFullAudit
    {
        [ForeignKey("GateEntryId")]
        public int? GateEntryId { get; set; }

        public DateTime InspectionDate { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("InvoiceId")]
        public int? InvoiceId { get; set; }

        [ForeignKey("VehicleInspectionHeaderId")]
        public ICollection<VehicleInspectionDetail> VehicleInspectionDetails { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public int? ChecklistTypeId { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public int? InspectionChecklistId { get; set; }

        [ForeignKey("TransactionStatusId")]
        public int? TransactionStatusId { get; set; }
    }
}