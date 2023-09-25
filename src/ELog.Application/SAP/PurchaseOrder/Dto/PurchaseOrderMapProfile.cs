using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.SAP.PurchaseOrder.Dto
{
    public class PurchaseOrderMapProfile : Profile
    {
        public PurchaseOrderMapProfile()
        {
            CreateMap<PurchaseOrderDto, ELog.Core.Entities.PurchaseOrder>()
                .ForMember(x => x.PurchaseOrderNo, opt => opt.MapFrom(x => x.PurchaseOrderNo.Trim()))
                 .ForMember(x => x.PlantId, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<MaterialDto, Material>()
                 .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<Material, MaterialInternalDto>();

        }
    }
}