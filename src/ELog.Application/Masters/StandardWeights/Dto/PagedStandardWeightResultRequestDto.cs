using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.StandardWeights.Dto
{
    public class PagedStandardWeightResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public float? Capacity { get; set; }

        public int? DepartmentId { get; set; }

        public int? ActiveInactiveStatusId { get; set; }

        public string Keyword { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}