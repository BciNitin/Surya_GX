using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.WipPicking.Dto
{
    public class WipPickingDto : EntityDto<int>
    {
        public int? ProcessOrderId { get; set; }
        public int? ProductId { get; set; }
        public string Stage { get; set; }
        public int? SuggestedLocationId { get; set; }
        public int? LocationId { get; set; }
        public int? ContainerId { get; set; }

        public string ContainerCode { get; set; }
        public int? ContainerCount { get; set; }
        public float Quantity { get; set; }
        public string Batch { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProcessOrderNo { get; set; }
        public string ProcessOrder { get; set; }
    }
}
