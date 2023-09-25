using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Locations.Dto
{
    public class LocationMapProfile : Profile
    {
        public LocationMapProfile()
        {
            CreateMap<LocationDto, LocationMaster>();
            CreateMap<LocationDto, LocationMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateLocationDto, LocationMaster>();
        }
    }
}