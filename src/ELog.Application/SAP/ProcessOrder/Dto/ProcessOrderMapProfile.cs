using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.SAP.ProcessOrder.Dto
{
    public class ProcessOrderMapProfile : Profile
    {
        public ProcessOrderMapProfile()
        {
            CreateMap<ProcessOrderDto, ELog.Core.Entities.ProcessOrder>()
                .ForMember(x => x.ProcessOrderNo, opt => opt.MapFrom(x => x.ProcessOrderNo.Trim()))
                 .ForMember(x => x.PlantId, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<ProcessOrderMaterialDto, ProcessOrderMaterial>()
                 .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<QCMaterialDto, ELog.Core.Entities.ProcessOrderMaterial>()
          .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                 .ForMember(x => x.ItemCode, opt => opt.MapFrom(x => x.ItemCode.Trim())).ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<ProcessOrderMaterial, QCMaterialDto>()
                 .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                 .ForMember(x => x.ItemCode, opt => opt.MapFrom(x => x.ItemCode.Trim()));
        }
    }
}