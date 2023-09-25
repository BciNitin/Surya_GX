using Abp.Application.Services.Dto;

namespace ELog.Application.QCSampling.Dto
{
    public class PagedQCSamplingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? MaterialType { get; set; }
        public int? InspectionLevel { get; set; }
        public string LotQuantityMin { get; set; }
        public string LotQuantityMax { get; set; }
        public int? InspectionQuantity { get; set; }
    }
}
