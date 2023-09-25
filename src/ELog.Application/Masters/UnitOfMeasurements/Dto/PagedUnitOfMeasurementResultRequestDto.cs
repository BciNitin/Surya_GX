using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.UnitOfMeasurements.Dto
{
    public class PagedUnitOfMeasurementResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }

        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}