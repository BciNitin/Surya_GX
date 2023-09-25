using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Modules.Dto
{
    public class ModuleMapProfile : Profile
    {
        public ModuleMapProfile()
        {
            CreateMap<ModuleDto, ModuleMaster>().ForMember(x => x.Name, opt => opt.Ignore());
            CreateMap<ModuleMaster, ModuleListDto>();
            CreateMap<CreateModuleDto, ModuleMaster>();
        }
    }
}