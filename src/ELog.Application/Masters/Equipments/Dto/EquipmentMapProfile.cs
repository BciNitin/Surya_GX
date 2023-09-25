using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Equipments.Dto
{
    public class EquipmentMapProfile : Profile
    {
        public EquipmentMapProfile()
        {
            CreateMap<EquipmentDto, EquipmentMaster>();
            CreateMap<EquipmentDto, EquipmentMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateEquipmentDto, EquipmentMaster>();
        }
    }
}