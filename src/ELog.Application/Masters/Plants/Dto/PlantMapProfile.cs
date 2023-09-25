using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Plants.Dto
{
    public class PlantMapProfile : Profile
    {
        public PlantMapProfile()
        {
            CreateMap<PlantDto, PlantMaster>();
            CreateMap<PlantDto, PlantMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreatePlantDto, PlantMaster>();
        }
    }
}