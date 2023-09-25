using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.WipPicking.Dto
{
    public class WipPickingProfile : Profile
    {
        public WipPickingProfile()
        {
            CreateMap<WipPickingDto, PickingMaster>();
            CreateMap<WipPickingDto, PickingMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateWipPickingDto, PickingMaster>();
            CreateMap<PickingMaster, WipPickingDto>();
        }
    }
}
