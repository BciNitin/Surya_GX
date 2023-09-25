using Abp.Application.Services.Dto;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto
{
    public class PagedApprovalUserModuleMapRequestDto : PagedAndSortedResultRequestDto
    {
        public int? AppLevelId { get; set; }

        public int? UserId { get; set; }


        public int? ModuleId { get; set; }


        public int? SubModuleId { get; set; }

        public int? ActiveInactiveStatusId { get; set; }
    }
}
