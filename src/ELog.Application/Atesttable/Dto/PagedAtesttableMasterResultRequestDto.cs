using Abp.Application.Services.Dto;


namespace ELog.Application.Atesttable.Dto
{
    public class PagedAtesttableMasterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string testfield1 { get; set; }
        public string testfield2 { get; set; }
        public string testfield3 { get; set; }
        public string testfield4 { get; set; }
    }
}
