using Abp.Application.Services.Dto;

namespace ELog.Application.Password.Dto
{
    public class PagedChangePasswordResultRequestDto : PagedAndSortedResultRequestDto
    {

        public int CurrentUser { get; set; }
        public string Keyword { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
