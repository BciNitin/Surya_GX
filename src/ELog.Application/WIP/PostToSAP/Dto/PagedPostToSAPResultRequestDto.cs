using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.PostToSAP.Dto
{
    public class PagedPostToSAPResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }

        public int? ActiveInactiveStatusId { get; set; }

    }
}
