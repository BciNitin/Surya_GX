using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Areas.Dto
{
    public class AreaMapProfile : Profile
    {
        public AreaMapProfile()
        {
            CreateMap<AreaDto, AreaMaster>();
            CreateMap<AreaDto, AreaMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateAreaDto, AreaMaster>();
            CreateMap<AreaListDto, AreaMaster>();
            CreateMap<AreaMaster, AreaListDto>();

        }
    }
}