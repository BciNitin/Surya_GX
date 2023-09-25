using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.MaterialDispensing.Dto
{
    public class MaterialDispensingMapProfile : Profile
    {
        public MaterialDispensingMapProfile()
        {
            CreateMap<MaterialDispensingDto, DispensingHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
        }
    }
}