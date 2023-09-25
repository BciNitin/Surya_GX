using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.Packing.Dto
{
    public class PackingListDto : EntityDto<int>
    {
        public int ProductCodeId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public string BatchNo { get; set; }
        public int? ContainerId { get; set; }

        public string ContainerCode { get; set; }
        public int? ContainerCount { get; set; }
        public int? Quantity { get; set; }

        public bool IsActive { get; set; }
        public string StorageLocation { get; set; }
        public string NoOfContainer { get; set; }

        public float? Qty { get; set; }

    }
}
