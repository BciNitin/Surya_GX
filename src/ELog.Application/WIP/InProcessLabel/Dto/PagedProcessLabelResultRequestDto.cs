using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.InProcessLabel.Dto
{
    public class PagedProcessLabelResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }
        public string ProductCode { get; set; }
        public string CubicleCode { get; set; }
    }
}
