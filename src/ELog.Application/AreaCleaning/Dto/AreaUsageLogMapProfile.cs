using AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.AreaCleaning.Dto
{
    public class AreaUsageLogMapProfile : Profile
    {
        public AreaUsageLogMapProfile()
        {
            CreateMap<AreaUsageLogDto, AreaUsageLog>();
            CreateMap<AreaUsageLogDto, AreaUsageLog>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateAreaUsageLogDto, AreaUsageLog>();
            CreateMap<AreaUsageLogListDto, AreaUsageLog>();
            CreateMap<AreaUsageLog, AreaUsageLogListDto>();

            CreateMap<CheckpointDto, AreaUsageListLog>()
    .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<AreaUsageListLog, CheckpointDto>();

        }
    }
}
