using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNHeaders")]
    public class GRNHeader : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Small)]
        public string GRNNumber { get; set; }

        public DateTime GRNPostingDate { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public int? PurchaseOrderId { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("GRNHeaderId")]
        public ICollection<GRNDetail> GRNDetails { get; set; }
    }
}