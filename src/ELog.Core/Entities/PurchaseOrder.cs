using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PurchaseOrders")]
    public class PurchaseOrder : PMMSFullAudit
    {
        [ForeignKey("PlantId")]
        [Required]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string PurchaseOrderNo { get; set; }

        [Required]
        public DateTime PurchaseOrderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string VendorName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string VendorCode { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturerName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturerCode { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public ICollection<Material> PurchaseOrders { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public ICollection<WeightCaptureHeader> WeightCaptureHeaders { get; set; }
    }
}