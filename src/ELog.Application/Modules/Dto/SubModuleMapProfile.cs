using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Modules.Dto
{
    public class SubModuleMapProfile : Profile
    {
        public SubModuleMapProfile()
        {
            CreateMap<SubModuleDto, SubModuleMaster>()
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.IsApprovalWorkflowRequired, opt => opt.Ignore());
            CreateMap<SubModuleMaster, SubModuleListDto>();
            CreateMap<SubModuleMaster, UpdateSubModuleDto>();
            CreateMap<UpdateSubModuleDto, SubModuleMaster>();
        }
    }
}