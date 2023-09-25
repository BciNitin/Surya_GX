using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeightCaptureHeaders")]
    public class WeightCaptureHeader : PMMSFullAudit
    {
        public int? TenantId { get; set; }

        [ForeignKey("InvoiceId")]
        public int InvoiceId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public int PurchaseOrderId { get; set; }

        [ForeignKey("WeightCaptureHeaderId")]
        public ICollection<WeightCaptureDetail> WeightCaptureHeaderDetails { get; set; }

        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }
        [ForeignKey("MfgBatchNoId")]
        public int MfgBatchNoId { get; set; }
        public int? UnitofMeasurementId { get; set; }
    }
}