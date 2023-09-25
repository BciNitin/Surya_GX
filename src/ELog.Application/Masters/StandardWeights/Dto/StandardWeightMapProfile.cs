using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.StandardWeights.Dto
{
    public class StandardWeightMapProfile : Profile
    {
        public StandardWeightMapProfile()
        {
            CreateMap<StandardWeightDto, StandardWeightMaster>();
            CreateMap<StandardWeightDto, StandardWeightMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateStandardWeightDto, StandardWeightMaster>();
        }
    }
}