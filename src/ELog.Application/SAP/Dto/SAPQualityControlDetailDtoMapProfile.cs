using AutoMapper;

using ELog.Application.SAP.Dto;

namespace ELog.Application.SAP.MaterialMaster.Dto
{
    public class SAPQualityControlDetailDtoMapProfile : Profile
    {
        public SAPQualityControlDetailDtoMapProfile()
        {
            CreateMap<SAPQualityControlDetailDto, Core.Entities.SAPQualityControlDetail>();
            CreateMap<Core.Entities.SAPQualityControlDetail, SAPQualityControlDetailDto>();
        }
    }
}