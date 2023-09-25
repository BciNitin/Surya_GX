using AutoMapper;

using ELog.Application.SAP.Dto;

namespace ELog.Application.SAP.MaterialMaster.Dto
{
    public class SAPReturntoMaterialMapProfile : Profile
    {
        public SAPReturntoMaterialMapProfile()
        {
            CreateMap<SAPReturntoMaterialDto, Core.Entities.SAPReturntoMaterial>();
            CreateMap<Core.Entities.SAPReturntoMaterial, SAPReturntoMaterialDto>();
        }
    }
}