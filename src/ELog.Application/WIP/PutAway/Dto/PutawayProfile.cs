using AutoMapper;
using ELog.Core.Entities;
//using ELog.Application.Masters.InspectionChecklists.Dto;

namespace ELog.Application.WIP.PutAway.Dto
{
    public class PutawayProfile : Profile
    {
        public PutawayProfile()
        {
            CreateMap<PutawayDto, Putaway>();
            CreateMap<PutawayDto, Putaway>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreatePutawayDto, Putaway>();
            CreateMap<Putaway, PutawayDto>();
        }
    }
}
