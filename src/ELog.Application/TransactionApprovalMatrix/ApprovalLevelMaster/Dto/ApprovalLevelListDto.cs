using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto
{
    [AutoMapFrom(typeof(ApprovalLevelMaster))]
    public class ApprovalLevelListDto : EntityDto<int>
    {
        public int LevelCode { get; set; }

        public string LevelName { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}