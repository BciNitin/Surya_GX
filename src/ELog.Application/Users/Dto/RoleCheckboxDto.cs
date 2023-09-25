using System.Collections.Generic;

namespace ELog.Application.Users.Dto
{
    public class RoleCheckboxDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
        public List<RoleCheckboxDto> UserRoles { get; set; }
    }
}