using Abp.Application.Services.Dto;


namespace ELog.Application.Recipe.Dto
{
    public class PagedRecipeMasterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string DocumentVersion { get; set; }
        public string Keyword { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
    }
}
