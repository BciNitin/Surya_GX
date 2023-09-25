using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Masters.PRNEntry.Dto
{
    [AutoMapFrom(typeof(PRNEntryMaster))]
    public class PRNEntryListDto : EntityDto<int>
    {
        public string PRNFileName { get; set; }
        public int SubPlantId { get; set; }
        public int? TenantId { get; set; }
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }

        public string UserEnteredSubPlantId { get; set; }
        public string UserEnteredModuleId { get; set; }
        public string UserEnteredSubModuleId { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}
