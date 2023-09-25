using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto
{
    [AutoMapFrom(typeof(ApprovalUserModuleMappingMaster))]
    public class ApprovalUserModuleMappingListDto : EntityDto<int>
    {
        public string UserEnteredAppLevelId { get; set; }
        public string UserEnteredUserId { get; set; }
        public int AppLevelId { get; set; }
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
    }
}
