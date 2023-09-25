using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Masters.PRNEntry.Dto
{
    public class PRNEntryMapProfile : Profile
    {
        public PRNEntryMapProfile()
        {
            CreateMap<PRNEntryDto, PRNEntryMaster>();
            CreateMap<PRNEntryDto, PRNEntryMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreatePRNEntryDto, PRNEntryMaster>();
            CreateMap<PRNEntryListDto, PRNEntryMaster>();
            CreateMap<PRNEntryMaster, PRNEntryListDto>();

        }
    }
}
