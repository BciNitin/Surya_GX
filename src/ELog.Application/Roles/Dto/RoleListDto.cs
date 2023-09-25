using Abp.Application.Services.Dto;

namespace ELog.Application.Roles.Dto
{
    public class RoleListDto : EntityDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}