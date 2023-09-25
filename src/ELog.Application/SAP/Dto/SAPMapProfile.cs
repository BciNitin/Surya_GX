using AutoMapper;

using ELog.Core.Entities;
using ELog.Core.SAP;

namespace ELog.Application.SAP.Dto
{
    public class SAPMapProfile : Profile
    {
        public SAPMapProfile()
        {
            CreateMap<SAPPlantMasterDto, SAPPlantMaster>();
            CreateMap<SAPPlantMaster, SAPPlantMasterDto>();
            CreateMap<SAPUOMMasterDto, SAPUOMMaster>();
            CreateMap<SAPUOMMaster, SAPUOMMasterDto>();
            CreateMap<GRNRequestResponseDto, SAPGRNPosting>();
            CreateMap<SAPGRNPosting, GRNRequestResponseDto>();
            CreateMap<SAPPlantMasterDto, PlantMaster>()
            .ForMember(dest => dest.PlantId, opt => opt.MapFrom(src => src.PlantCode))
            .ForMember(dest => dest.PlantName, opt => opt.MapFrom(src => src.PlantCode))
            .ForMember(dest => dest.TaxRegistrationNo, opt => opt.MapFrom(src => src.TaxRegNo))
            .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.State) ? null : src.State))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Country) ? null : src.Country))
            .ForMember(dest => dest.PlantTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.MasterPlantId, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovalStatusId, opt => opt.Ignore()).ReverseMap();
            CreateMap<SAPQualityControlDetail, SAPQualityControlDetailDto>()
                .ForMember(dest => dest.RetestDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReleasedOn, opt => opt.Ignore()).ReverseMap();
        }
    }
}