using Abp.Application.Services.Dto;

namespace ELog.Application.Modules.Dto
{
    public class ModuleListDto : EntityDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}