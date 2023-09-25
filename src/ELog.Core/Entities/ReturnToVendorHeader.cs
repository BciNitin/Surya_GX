using ELog.Core.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ReturnToVendorHeader")]
    public class ReturnToVendorHeader : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string MaterialDocumentNo { get; set; }
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string MaterialCode { get; set; }
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNumber { get; set; }
        public int? TenantId { get; set; }
        [Required]
        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        public float? Qty { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string UOM { get; set; }
        [ForeignKey("ReturnToVendorHeaderId")]
        public ICollection<ReturnToVendorDetail> ReturnToVendorDetails { get; set; }
    }
}
