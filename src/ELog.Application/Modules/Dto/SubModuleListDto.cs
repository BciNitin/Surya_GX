using Abp.Application.Services.Dto;

namespace ELog.Application.Modules.Dto
{
    public class SubModuleListDto : EntityDto
    {
        public string Name { get; set; }

        public string ModuleName { get; set; }
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public string SubModuleType { get; set; }
        public int? ModuleId { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalRequired { get; set; }
    }
}