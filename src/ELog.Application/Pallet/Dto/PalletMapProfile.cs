using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Pallet.Dto
{
    public class PalletMapProfile : Profile
    {
        public PalletMapProfile()
        {
            CreateMap<PalletMasterDto, PalletMaster>();
            CreateMap<PalletMasterDto, PalletMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            //CreateMap<CreateAreaDto, AreaMaster>();
            CreateMap<PalletMasterListDto, PalletMaster>();
            CreateMap<PalletMaster, PalletMasterListDto>();
        }
    }
}
