using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Devices.Dto
{
    public class DeviceMapProfile : Profile
    {
        public DeviceMapProfile()
        {
            CreateMap<DeviceDto, DeviceMaster>();
            CreateMap<DeviceDto, DeviceMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateDeviceDto, DeviceMaster>();
        }
    }
}