using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.WIPPrintPacking.Dto
{
    [AutoMapTo(typeof(LabelPrintPacking))]
    public class PrintPackingDto : EntityDto<int>
    {
        public string ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public string BatchNo { get; set; }

        public int? ContainerId { get; set; }
        public string ContainerBarcode { get; set; }
        public int? ContainerCount { get; set; }
        public string Quantity { get; set; }
        public string PackingLabelBarcode { get; set; }
        public string labelProcessBarcodeCode { get; set; }
        public bool IsPrint { get; set; }

        public int PrintCount { get; set; }
        public int? PrinterId { get; set; }
    }
}
