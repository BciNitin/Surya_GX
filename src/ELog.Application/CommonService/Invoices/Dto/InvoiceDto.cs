using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.CommonService.Invoices.Dto
{
    [AutoMapTo(typeof(InvoiceDetail))]
    public class InvoiceDto : EntityDto<int>
    {
        public int? PurchaseOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PurchaseOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Invoice No is required.")]
        [StringLength(PMMSConsts.Medium)]
        public string InvoiceNo { get; set; }

        [Required]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string LRNo { get; set; }

        public DateTime? LRDate { get; set; }

        [Required(ErrorMessage = "Driver Name is required.")]
        [StringLength(PMMSConsts.Small)]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Vehicle Number is required.")]
        [StringLength(PMMSConsts.Small)]
        public string VehicleNumber { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string TransporterName { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string purchaseOrderDeliverSchedule { get; set; }

        public int? PlantId { get; set; }

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