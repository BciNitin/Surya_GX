using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    [AutoMapFrom(typeof(WeightCaptureHeader))]
    public class WeightCaptureListDto : EntityDto<int>
    {
        public int? PurchaseOrderId { get; set; }

        public string PurchaseOrderNo { get; set; }

        public string InvoiceNo { get; set; }
        public string MaterialItemCode { get; set; }
        public string ManufacturedBatchNo { get; set; }
        public int? SubPlantId { get; set; }
        public int? MaterialId { get; set; }
        public string ItemDescription { get; set; }
    }
}