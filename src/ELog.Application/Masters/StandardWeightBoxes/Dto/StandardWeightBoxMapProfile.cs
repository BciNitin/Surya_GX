using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.StandardWeightBoxes.Dto
{
    public class StandardWeightBoxMapProfile : Profile
    {
        public StandardWeightBoxMapProfile()
        {
            CreateMap<StandardWeightBoxDto, StandardWeightBoxMaster>();
            CreateMap<StandardWeightBoxDto, StandardWeightBoxMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<StandardWeightBoxMaster, StandardWeightBoxDto>();
            CreateMap<StandardWeightBoxMaster, StandardWeightBoxListDto>();
            CreateMap<CreateStandardWeightBoxDto, StandardWeightBoxMaster>();
        }
    }
}