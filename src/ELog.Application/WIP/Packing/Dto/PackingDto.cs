using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.Packing.Dto
{
    [AutoMapTo(typeof(PackingMaster))]
    public class PackingDto : EntityDto<int>
    {
        public string ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public string ProductCode { get; set; }

        public string BatchNo { get; set; }
        public int? ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public int? ContainerCount { get; set; }
        public int? Quantity { get; set; }

        public bool IsActive { get; set; }
    }
}
