using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Gates.Dto
{
    public class GateMapProfile : Profile
    {
        public GateMapProfile()
        {
            CreateMap<GateDto, GateMaster>();
            CreateMap<GateDto, GateMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateGateDto, GateMaster>();
        }
    }
}