using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto
{
    public class ProcessOrderAfterReleasMapProfile : Profile
    {
        public ProcessOrderAfterReleasMapProfile()
        {
            CreateMap<ProcessOrderAfterReleasDto, ELog.Core.Entities.ProcessOrderAfterRelease>()
                .ForMember(x => x.ProcessOrderNo, opt => opt.MapFrom(x => x.ProcessOrderNo.Trim()))
                .ForMember(x => x.PlantId, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<ProcessOrderMaterialAfterReleasDto, ProcessOrderMaterialAfterRelease>()
                // .ForMember(x => x.ItemNo, opt => opt.MapFrom(x => x.ItemNo.Trim()))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
        }
    }
}
