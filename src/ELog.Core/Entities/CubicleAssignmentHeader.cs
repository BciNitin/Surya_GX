using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleAssignmentHeader")]
    public class CubicleAssignmentHeader : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string GroupId { get; set; }

        public DateTime CubicleAssignmentDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("GroupStatusId")]
        public int? GroupStatusId { get; set; }
        public bool IsSampling { get; set; }

        [ForeignKey("CubicleAssignmentHeaderId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }

        [ForeignKey("CubicleAssignmentHeaderId")]
        public ICollection<EquipmentAssignment> EquipmentAssignments { get; set; }
        [ForeignKey("GroupId")]
        public ICollection<LineClearanceTransaction> LineClearanceTransactions { get; set; }
    }
}