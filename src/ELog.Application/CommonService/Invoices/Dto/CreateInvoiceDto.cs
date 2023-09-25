using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.CommonService.Invoices.Dto
{
    [AutoMapTo(typeof(InvoiceDetail))]
    public class CreateInvoiceDto
    {
        public int? PurchaseOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PurchaseOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string VendorName { get; set; }

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
        public string PurchaseOrderDeliverSchedule { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string VendorBatchNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string Manufacturer { get; set; }
        [StringLength(PMMSConsts.Small)]
        public string ManufacturerCode { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string DeliveryNote { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string BillofLanding { get; set; }
        [StringLength(PMMSConsts.Small)]
        public string VendorCode { get; set; }
    }
}