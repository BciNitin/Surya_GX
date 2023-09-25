using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Calenders.Dto
{
    public class CalendersMapProfile : Profile
    {
        public CalendersMapProfile()
        {
            CreateMap<CalenderDto, CalenderMaster>();
            CreateMap<CalenderDto, CalenderMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateCalenderDto, CalenderMaster>();
        }
    }
}