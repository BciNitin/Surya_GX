using AutoMapper;

namespace ELog.Application.WIP.MaterialReturn.Dto
{
    public class MaterialReturnProfileDto : Profile
    {
        public MaterialReturnProfileDto()
        {
            CreateMap<MaterialReturnDto, ELog.Core.Entities.MaterialReturn>();

            CreateMap<MaterialReturnDto, ELog.Core.Entities.MaterialReturn>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateMaterialReturnDto, ELog.Core.Entities.MaterialReturn>();
            CreateMap<ELog.Core.Entities.MaterialReturn, MaterialReturnDto>();
        }
    }
}
