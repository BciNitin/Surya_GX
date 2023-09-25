using Abp.Application.Services.Dto;

namespace ELog.Application.Password.Dto
{

    public class RequestedUsersListDto : EntityDto
    {

        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public string RoleNames { get; set; }

    }
}
