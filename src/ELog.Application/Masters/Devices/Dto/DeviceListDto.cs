using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Devices.Dto
{
    [AutoMapFrom(typeof(DeviceMaster))]
    public class DeviceListDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }

        public string DeviceId { get; set; }

        public int? DeviceTypeId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public bool IsActive { get; set; }
        public string UserEnteredPlantId { get; set; }
        public string UserEnteredDeviceType { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}