using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.UnitOfMeasurements.Dto
{
    public class UnitOfMeasurementMapProfile : Profile
    {
        public UnitOfMeasurementMapProfile()
        {
            CreateMap<UnitOfMeasurementDto, UnitOfMeasurementMaster>();
            CreateMap<UnitOfMeasurementDto, UnitOfMeasurementMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.UOMCode, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.IsActive, opt => opt.Ignore())
                .ForMember(x => x.ConversionUOM, opt => opt.Ignore())
                .ForMember(x => x.UnitOfMeasurement, opt => opt.Ignore());

            CreateMap<CreateUnitOfMeasurementDto, UnitOfMeasurementMaster>();
        }
    }
}