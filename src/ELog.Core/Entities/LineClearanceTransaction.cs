using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LineClearanceTransactions")]
    public class LineClearanceTransaction : PMMSFullAudit
    {
        [Required]
        public DateTime ClearanceDate { get; set; }

        [ForeignKey("CubicleId")]
        public int CubicleId { get; set; }

        [ForeignKey("GroupId")]
        public int GroupId { get; set; }

        [ForeignKey("StatusId")]
        public int StatusId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? StopTime { get; set; }
        public int? VerifiedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int? TenantId { get; set; }
        public bool IsSampling { get; set; }
        public DateTime? ApprovedTime { get; set; }
        [ForeignKey("LineClearanceTransactionId")]
        public ICollection<LineClearanceCheckpoint> LineClearanceCheckpoints { get; set; }
    }
}