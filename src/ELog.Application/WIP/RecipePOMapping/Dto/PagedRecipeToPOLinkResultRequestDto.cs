using Abp.Application.Services.Dto;


namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    public class PagedRecipeToPOLinkResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ProductCode { get; set; }
        public string RecipeNo { get; set; }
    }
}
