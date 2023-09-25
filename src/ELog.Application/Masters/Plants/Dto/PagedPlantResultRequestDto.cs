using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Plants.Dto
{
    public class PagedPlantResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? PlantTypeId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }

        public int? CountryId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}