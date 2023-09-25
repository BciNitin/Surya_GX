using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.WipPicking.Dto
{
    [AutoMapTo(typeof(PickingMaster))]
    public class CreateWipPickingDto
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
    }
}
