using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.WeightVerification.Dto
{
    public class PagedWeightVrifyReturnRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ProductCode { get; set; }
        public string ProcessOrderNo { get; set; }
    }
}
