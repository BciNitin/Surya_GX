using AutoMapper;
using ELog.Core.Entities;


namespace ELog.Application.WIP.Packing.Dto
{
    public class PackingMapProfile : Profile
    {
        public PackingMapProfile()
        {
            //CreateMap<PackingDto, PackingMaster>();
            //CreateMap<PackingDto, PackingMaster>()
            //    .ForMember(x => x.CreationTime, opt => opt.Ignore());

            //CreateMap<CreatePackingDto, PackingMaster>();
            //CreateMap<PackingMaster, PackingListDto>();

            CreateMap<PackingDto, PackingMaster>()
                    .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<PackingMaster, PackingDto>();
            CreateMap<CreatePackingDto, PackingMaster>();
            CreateMap<CreatePackingDto, CreatePackingDto>();
            CreateMap<PackingListDto, PackingMaster>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            //.ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<PackingMaster, PackingListDto>();
        }
    }
}
