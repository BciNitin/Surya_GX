using ELog.Core.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNQtyDetails")]
    public class GRNQtyDetail : PMMSFullAudit
    {
        [ForeignKey("GRNDetailId")]
        public int? GRNDetailId { get; set; }

        [Required]
        public float TotalQty { get; set; }

        [Required]
        public float NoOfContainer { get; set; }

        [Required]
        public float QtyPerContainer { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string IsDamaged { get; set; }

        [ForeignKey("GRNQtyDetailId")]
        public ICollection<GRNMaterialLabelPrintingContainerBarcode> GRNMaterialLabelPrintingContainerBarcodes { get; set; }
    }
}