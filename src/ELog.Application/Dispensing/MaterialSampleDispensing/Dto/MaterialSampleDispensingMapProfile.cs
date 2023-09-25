using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.MaterialSampleDispensing.Dto
{
    public class MaterialSampleDispensingMapProfile : Profile
    {
        public MaterialSampleDispensingMapProfile()
        {
            CreateMap<MaterialSampleDispensingDto, DispensingHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
        }
    }
}