using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.MaterialReturn.Dto
{
    public class PagedMaterialReturnRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ProductNo { get; set; }
        public string BatchNo { get; set; }
        public string DocumentNo { get; set; }
        public int? ContainerId { get; set; }
    }
}
