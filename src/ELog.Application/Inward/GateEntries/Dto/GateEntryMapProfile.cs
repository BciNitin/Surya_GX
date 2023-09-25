using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.GateEntries.Dto
{
    public class GateEntryMapProfile : Profile
    {
        public GateEntryMapProfile()
        {
            CreateMap<GateEntryDto, GateEntry>();
            CreateMap<GateEntryDto, GateEntry>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<GateEntry, GateEntryListDto>();
            CreateMap<CreateGateEntryDto, GateEntry>();
            CreateMap<UpdateGateEntryDto, GateEntry>();
        }
    }
}