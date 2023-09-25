using Abp.Application.Services.Dto;


namespace ELog.Application.WIP.WipPicking.Dto
{
    public class PagedWipPickingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ContainerCode { get; set; }
        public string ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public int? ActiveInactiveStatusId { get; set; }
    }
}
