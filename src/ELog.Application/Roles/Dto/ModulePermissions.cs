using Abp.Application.Services.Dto;

using System.Collections.Generic;

namespace ELog.Application.Roles.Dto
{
    public class ModulePermissionsDto : EntityDto<int>
    {
        public int ModuleSubModuleId { get; set; }

        public List<int> GrantedPermissions { get; set; }
    }
}