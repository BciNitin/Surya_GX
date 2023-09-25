using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GRNMaterialLabelPrintingDetails")]
    public class GRNMaterialLabelPrintingDetail : PMMSFullAudit
    {
        [ForeignKey("GRNMaterialLabelPrintingHeaderId")]
        public int? GRNMaterialLabelPrintingHeaderId { get; set; }

        [ForeignKey("PrinterId")]
        public int? PrinterId { get; set; }

        public bool IsController { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string RangePrint { get; set; }

        public string Comment { get; set; }
    }
}