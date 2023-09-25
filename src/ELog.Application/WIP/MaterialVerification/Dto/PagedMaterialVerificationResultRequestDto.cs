using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.MaterialVerification.Dto
{
    public class PagedMaterialVerificationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }

        public int? ProductID { get; set; }
        public int? ProcessOrderId { get; set; }

        public int? CubicleBarcodeId { get; set; }
    }
}
