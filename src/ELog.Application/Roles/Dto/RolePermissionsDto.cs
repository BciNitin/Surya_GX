using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Roles.Dto
{
    public class RolePermissionsDto
    {
        [Required]
        public int? ModuleSubModuleId { get; set; }

        public string ModuleName { get; set; }

        public string SubModuleName { get; set; }

        public bool IsGranted { get; set; }
        public bool IsSuperAdminPermission { get; set; }

        [Required]
        public List<ActionDto> GrantedPermissions { get; set; }
    }
}