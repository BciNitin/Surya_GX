using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNDetails")]
    public class GRNDetail : PMMSFullAudit
    {
        [ForeignKey("GRNHeaderId")]
        public int? GRNHeaderId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string SAPBatchNumber { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("MaterialId")]
        public int? MaterialId { get; set; }

        [ForeignKey("MfgBatchNoId")]
        public int? MfgBatchNoId { get; set; }

        [ForeignKey("InvoiceId")]
        public int? InvoiceId { get; set; }

        [Required]
        public float TotalQty { get; set; }



        [Required]
        public float NoOfContainer { get; set; }

        [Required]
        public float QtyPerContainer { get; set; }

        public string TotalQtyInDecimal { get; set; }

        public string QtyPerContainerInDecimal { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }

        [ForeignKey("GRNDetailId")]
        public ICollection<GRNQtyDetail> GRNQtyDetails { get; set; }

        [ForeignKey("GRNDetailId")]
        public ICollection<GRNMaterialLabelPrintingHeader> GRNMaterialLabelPrintingHeaders { get; set; }

        [ForeignKey("GRNDetailId")]
        public ICollection<Palletization> Palletizations { get; set; }

        [ForeignKey("GRNDetailId")]
        public ICollection<GRNMaterialLabelPrintingContainerBarcode> GRNMaterialLabelPrintingContainerBarcodes { get; set; }
    }
}