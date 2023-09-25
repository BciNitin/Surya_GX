using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.HandlingUnits.Dto
{
    public class HandlingUnitMapProfile : Profile
    {
        public HandlingUnitMapProfile()
        {
            CreateMap<HandlingUnitDto, HandlingUnitMaster>();
            CreateMap<HandlingUnitDto, HandlingUnitMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateHandlingUnitDto, HandlingUnitMaster>();
        }
    }
}