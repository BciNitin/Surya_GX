using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.PutAway.Dto
{
    [AutoMapTo(typeof(Putaway))]
    public class PutawayDto : EntityDto<int>
    {
        public int LocationId { get; set; }
        public int ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public bool isActive { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public string StorageLocation { get; set; }
        public int ProcessOrderId { get; set; }
        public int ProductCodeId { get; set; }

        public string ProcessOrderNo { get; set; }
        public string NoOfContainer { get; set; }

    }
}
