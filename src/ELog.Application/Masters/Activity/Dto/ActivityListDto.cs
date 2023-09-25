using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Activity.Dto
{
    [AutoMapFrom(typeof(ActivityMaster))]
    public class ActivityListDto : EntityDto<int>
    {
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public string UserEnteredModuleId { get; set; }
        public string UserEnteredSubModuleId { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }

    }
}
