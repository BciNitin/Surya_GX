using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("InProcessLabelDetails")]
    public class InProcessLabelDetails : PMMSFullAudit
    {
        public int CubicleId { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? ProductId { get; set; }
        public int ContainerBarcodeId { get; set; }
        public string ContainerBarcode { get; set; }
        public int? ScanBalanceId { get; set; }
        public string ScanBalance { get; set; }
        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public string NoOfContainer { get; set; }
        public string ProcessLabelBarcode { get; set; }
        public bool IsPrint { get; set; }

        public bool Ischeck { get; set; }

        public int PrintCount { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("DeviceMaster")]
        public int? PrinterId { get; set; }

    }
}
