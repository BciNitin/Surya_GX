using Abp.Application.Services.Dto;


namespace ELog.Application.WIP.PutAway.Dto
{
    public class PagedPutawayResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ContainerCode { get; set; }
        public int? LocationId { get; set; }
    }
}
