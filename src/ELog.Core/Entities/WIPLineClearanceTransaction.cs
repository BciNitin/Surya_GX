using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WIPLineClearanceTransaction")]
    public class WIPLineClearanceTransaction : PMMSFullAudit
    {
        [Required]
        public DateTime ClearanceDate { get; set; }

        public int? ProductId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [ForeignKey("CubicleMaster")]
        public int CubicleBarcodeId { get; set; }

        [ForeignKey("EquipmentMaster")]
        public int EquipmentBarcodeId { get; set; }

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

        public int? ChecklistTypeId { get; set; }

        public string Remarks { get; set; }

        [ForeignKey("LineClearanceTransactionId")]
        public ICollection<WIPLineClearanceCheckpoints> LineClearanceCheckpoints { get; set; }
    }
}
