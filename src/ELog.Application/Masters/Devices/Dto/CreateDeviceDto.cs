using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Devices.Dto
{
    [AutoMapTo(typeof(DeviceMaster))]
    public class CreateDeviceDto
    {
        public int SubPlantId { get; set; }

        [MaxLength(PMMSConsts.Small)]
        public string DeviceId { get; set; }

        public int? DeviceTypeId { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string Make { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string Model { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string SerialNo { get; set; }

        [MaxLength(PMMSConsts.Small)]
        public string IpAddress { get; set; }

        public int? Port { get; set; }

        public int? DepartmentId { get; set; }

        public int? AreaId { get; set; }

        public int? CubicleId { get; set; }
        public int? ModeId { get; set; }

        public bool IsActive { get; set; }
    }
}