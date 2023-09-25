using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    public class InspectionChecklistMapProfile : Profile
    {
        public InspectionChecklistMapProfile()
        {
            CreateMap<InspectionChecklistDto, InspectionChecklistMaster>();
            CreateMap<InspectionChecklistDto, InspectionChecklistMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<InspectionChecklistMaster, InspectionChecklistListDto>();

            CreateMap<InspectionChecklistListDto, InspectionChecklistMaster>();
        }
    }
}