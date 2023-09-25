using Abp.Application.Services.Dto;

namespace ELog.Application.CommonDto
{
    public class ApprovalStatusDto : EntityDto<int>
    {
        public int ApprovalStatusId { get; set; }
        public string Description { get; set; }
    }
}
