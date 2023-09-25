using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Devices.Dto
{
    public class PagedDeviceResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }

        public int? DeviceTypeId { get; set; }
        public string DeviceId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}