using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    [AutoMapFrom(typeof(WeightCaptureHeader))]
    public class WeightCaptureDto : EntityDto<int>
    {
        public int? TenantId { get; set; }

        public int? InvoiceId { get; set; }
        public string invoiceNo { get; set; }
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public ICollection<WeightCaptureDetailsDto> WeightCaptureHeaderDetails { get; set; }

        public int? MaterialId { get; set; }

        public int? MfgBatchNoId { get; set; }
        public int? UnitofMeasurementId { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public WeightCaptureDetailsDto WeightCaptureDetailsDto { get; set; }
    }
}