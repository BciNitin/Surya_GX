using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.PutAway.Dto
{
    public class PutawayListDto : EntityDto<int>
    {
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public int? ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public bool isActive { get; set; }
        public string ProductCode { get; set; }
        public string ProcessOrder { get; set; }
        public string ProductName { get; set; }
        public string StorageLocation { get; set; }


    }
}
