using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.ChecklistTypes.Dto
{
    public class ChecklistTypeMapProfile : Profile
    {
        public ChecklistTypeMapProfile()
        {
            CreateMap<ChecklistTypeDto, ChecklistTypeMaster>();
            CreateMap<ChecklistTypeDto, ChecklistTypeMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.ChecklistTypeCode, opt => opt.Ignore());

            CreateMap<CreateChecklistTypeDto, ChecklistTypeMaster>();
        }
    }
}