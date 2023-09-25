using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialInspectionHeaders")]
    public class MaterialInspectionHeader : PMMSFullAudit
    {
        [ForeignKey("GateEntryId")]
        public int? GateEntryId { get; set; }

        [ForeignKey("InvoiceId")]
        public int? InvoiceId { get; set; }

        [ForeignKey("TransactionStatusId")]
        public int? TransactionStatusId { get; set; }

        [ForeignKey("MaterialHeaderId")]
        public ICollection<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }
    }
}