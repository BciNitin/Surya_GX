using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Modules.Dto
{
    [AutoMapTo(typeof(ModuleSubModule))]
    public class CreateModuleSubModuleDto : EntityDto<int>
    {
        public int ModuleId { get; set; }

        public int SubModuleId { get; set; }

        public bool IsMandatory { get; set; }
    }
}