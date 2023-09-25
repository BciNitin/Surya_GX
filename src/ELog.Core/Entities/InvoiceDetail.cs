using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("InvoiceDetails")]
    public class InvoiceDetail : PMMSFullAudit
    {
        [ForeignKey("PurchaseOrderId")]
        public int? PurchaseOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PurchaseOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string VendorName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string VendorCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string InvoiceNo { get; set; }

        [Required]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string LRNo { get; set; }

        public DateTime? LRDate { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string DriverName { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string VehicleNumber { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string TransporterName { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string purchaseOrderDeliverSchedule { get; set; }

        [ForeignKey("InvoiceId")]
        public ICollection<VehicleInspectionHeader> VehicleInspectionHeaders { get; set; }

        [ForeignKey("InvoiceId")]
        public ICollection<GateEntry> GateEntries { get; set; }

        [ForeignKey("InvoiceId")]
        public ICollection<WeightCaptureHeader> WeightCaptureHeaders { get; set; }

        [ForeignKey("InvoiceId")]
        public ICollection<MaterialInspectionHeader> MaterialInspectionHeaders { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string VendorBatchNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string Manufacturer { get; set; }
        [StringLength(PMMSConsts.Small)]
        public string ManufacturerCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string DeliveryNote { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string BillofLanding { get; set; }
    }
}