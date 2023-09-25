using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNMaterialLabelPrintingContainerBarcodes")]
    public class GRNMaterialLabelPrintingContainerBarcode : PMMSFullAudit
    {
        [ForeignKey("GRNMaterialLabelPrintingHeaderId")]
        public int? GRNMaterialLabelPrintingHeaderId { get; set; }

        public int ContainerNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string MaterialLabelContainerBarCode { get; set; }

        [ForeignKey("GRNDetailId")]
        public int? GRNDetailId { get; set; }

        [ForeignKey("GRNQtyDetailId")]
        public int? GRNQtyDetailId { get; set; }

        public float? Quantity { get; set; }
        public float? BalanceQuantity { get; set; }
        public bool? IsLoosedContainer { get; set; }

        [ForeignKey("ContainerId")]
        public ICollection<Palletization> Palletizations { get; set; }

        [ForeignKey("ContainerId")]
        public ICollection<PutAwayBinToBinTransfer> PutAwayBinToBinTransfers { get; set; }
    }
}