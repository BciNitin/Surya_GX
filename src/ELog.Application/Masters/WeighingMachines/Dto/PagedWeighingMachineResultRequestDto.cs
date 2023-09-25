using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    public class PagedWeighingMachineResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? SubPlantId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}