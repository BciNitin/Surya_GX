using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LabelPrintPacking")]
    public class LabelPrintPacking : PMMSFullAudit
    {

        public int? ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public int ContainerBarcodeId { get; set; }
        public string ContainerBarcode { get; set; }

        public int ContainerCount { get; set; }

        public string Quantity { get; set; }
        public string PackingLabelBarcode { get; set; }

        public bool IsActive { get; set; }

        public bool IsPrint { get; set; }

        public int PrintCount { get; set; }

        [ForeignKey("DeviceMaster")]
        public int? PrinterId { get; set; }
    }
}
