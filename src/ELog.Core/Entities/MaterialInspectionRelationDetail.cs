using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialInspectionRelationDetails")]
    public class MaterialInspectionRelationDetail : PMMSFullAudit
    {
        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }

        [ForeignKey("MaterialHeaderId")]
        public int MaterialHeaderId { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public int? ChecklistTypeId { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public int? InspectionChecklistId { get; set; }

        [ForeignKey("TransactionStatusId")]
        public int? TransactionStatusId { get; set; }

        [ForeignKey("MaterialRelationId")]
        public ICollection<MaterialChecklistDetail> MaterialCheckpoints { get; set; }

        [ForeignKey("MaterialRelationId")]
        public ICollection<MaterialConsignmentDetail> MaterialConsignments { get; set; }

        [ForeignKey("MaterialRelationId")]
        public ICollection<MaterialDamageDetail> MaterialDamageDetails { get; set; }
    }
}