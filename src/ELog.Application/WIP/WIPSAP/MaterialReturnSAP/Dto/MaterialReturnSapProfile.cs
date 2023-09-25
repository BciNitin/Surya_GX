using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WIP.WIPSAP.MaterialReturnSAP.Dto
{
    public class MaterialReturnSapProfile : Profile
    {
        public MaterialReturnSapProfile()
        {
            CreateMap<MaterialRteturnDetailsSAPDto, MaterialRteturnDetailsSAP>()
                // .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
        }
    }
}
