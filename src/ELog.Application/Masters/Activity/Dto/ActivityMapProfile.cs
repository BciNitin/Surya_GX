using AutoMapper;
using ELog.Core.Entities;


namespace ELog.Application.Activity.Dto
{
    public class ActivityMapProfile : Profile
    {
        public ActivityMapProfile()
        {
            CreateMap<ActivityDto, ActivityMaster>();
            CreateMap<ActivityDto, ActivityMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateActivityDto, ActivityMaster>();
            CreateMap<ActivityListDto, ActivityMaster>();
            CreateMap<ActivityMaster, ActivityListDto>();

        }
    }
}
