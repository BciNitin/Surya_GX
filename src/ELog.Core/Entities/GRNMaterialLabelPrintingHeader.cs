using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNMaterialLabelPrintingHeaders")]
    public class GRNMaterialLabelPrintingHeader : PMMSFullAudit
    {
        [ForeignKey("GRNDetailId")]
        public int? GRNDetailId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string PackDetails { get; set; }

        [ForeignKey("GRNMaterialLabelPrintingHeaderId")]
        public ICollection<GRNMaterialLabelPrintingDetail> GRNMaterialLabelPrintingDetails { get; set; }

        [ForeignKey("GRNMaterialLabelPrintingHeaderId")]
        public ICollection<GRNMaterialLabelPrintingContainerBarcode> GRNMaterialLabelPrintingContainerBarcodes { get; set; }
    }
}