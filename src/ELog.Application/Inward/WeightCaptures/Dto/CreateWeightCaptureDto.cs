using Abp.AutoMapper;

using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    [AutoMapTo(typeof(WeightCaptureHeader))]
    public class CreateWeightCaptureDto
    {
        public int? TenantId { get; set; }

        [Required(ErrorMessage = "Invoice No is required.")]
        public int? InvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Purchase Order No is required.")]
        public int? PurchaseOrderId { get; set; }

        public string PurchaseOrderNo { get; set; }
        public ICollection<WeightCaptureDetailsDto> WeightCaptureHeaderDetails { get; set; }

        [Required(ErrorMessage = "Material is required.")]
        public int? MaterialId { get; set; }

        [Required(ErrorMessage = "Manufacturing is required.")]
        public int? MfgBatchNoId { get; set; }

        [Required(ErrorMessage = " Unit Of Measurement is required.")]
        public int? UnitOfMeasurementId { get; set; }
    }
}