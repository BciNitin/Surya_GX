using Abp.Application.Services.Dto;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto
{
    public class PagedApprovalLevelResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int LevelCode { get; set; }

        public string LevelName { get; set; }

        public int? ActiveInactiveStatusId { get; set; }
        public string Keyword { get; set; }
    }
}