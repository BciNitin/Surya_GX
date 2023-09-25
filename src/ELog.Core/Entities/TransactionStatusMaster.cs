using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("TransactionStatusMaster")]
    public class TransactionStatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string TransactionStatus { get; set; }

        [ForeignKey("TransactionStatusId")]
        public ICollection<VehicleInspectionHeader> VehicleInspectionHeaders { get; set; }

        [ForeignKey("TransactionStatusId")]
        public ICollection<MaterialInspectionHeader> MaterialInspectionHeaders { get; set; }

        [ForeignKey("TransactionStatusId")]
        public ICollection<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }
    }
}