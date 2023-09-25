using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Gates.Dto
{
    public class PagedGateResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PlantId { get; set; }
        public string GateCode { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}