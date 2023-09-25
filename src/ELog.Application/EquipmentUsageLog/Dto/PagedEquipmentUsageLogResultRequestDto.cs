using Abp.Application.Services.Dto;

namespace ELog.Application.EquipmentUsageLog.Dto
{
    public class PagedEquipmentUsageLogResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? ActivityId { get; set; }
        public int? equipmentBracodeId { get; set; }

        public int? processBarcodeId { get; set; }
    }
}
