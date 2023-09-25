using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Cubicles.Dto
{
    public class CubicleMapProfile : Profile
    {
        public CubicleMapProfile()
        {
            CreateMap<CubicleDto, CubicleMaster>();
            CreateMap<CubicleDto, CubicleMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateCubicleDto, CubicleMaster>();
        }
    }
}