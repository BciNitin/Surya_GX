using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleCleaningTransactions")]
    public class CubicleCleaningTransaction : PMMSFullAudit
    {
        [Required]
        public DateTime CleaningDate { get; set; }

        [ForeignKey("CubicleId")]
        public int CubicleId { get; set; }

        [ForeignKey("TypeId")]
        public int TypeId { get; set; }
        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public int CleanerId { get; set; }
        public int? VerifierId { get; set; }

        public string DoneBy { get; set; }
        public bool IsSampling { get; set; }
        public int? TenantId { get; set; }
        public string Remark { get; set; }
        [ForeignKey("CubicleCleaningTransactionId")]
        public ICollection<CubicleCleaningCheckpoint> CubicleCleaningCheckpoints { get; set; }
    }
}