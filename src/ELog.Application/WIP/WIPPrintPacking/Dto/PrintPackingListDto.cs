using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.WIPPrintPacking.Dto
{
    public class PrintPackingListDto : EntityDto<int>
    {
        public int? ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public string BatchNo { get; set; }
        public int? ContainerId { get; set; }
        public string ContainerBarcode { get; set; }
        public int? ContainerCount { get; set; }
        public string Quantity { get; set; }
        public string PackingLabelBarcode { get; set; }
        public bool IsPrint { get; set; }
        public int PrintCount { get; set; }
        public int? PrinterId { get; set; }
    }
}
