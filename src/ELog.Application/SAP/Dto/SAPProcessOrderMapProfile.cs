using AutoMapper;

using ELog.Application.SAP.Dto;

namespace ELog.Application.SAP.MaterialMaster.Dto
{
    public class SAPProcessOrderMapProfile : Profile
    {
        public SAPProcessOrderMapProfile()
        {
            CreateMap<SAPProcessOrderDto, Core.Entities.SAPProcessOrder>();
            CreateMap<Core.Entities.SAPProcessOrder, SAPProcessOrderDto>();
        }
    }
}