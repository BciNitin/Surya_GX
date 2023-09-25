
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ELog.Application.Roles.Dto
{
    public class ModuleSubModulesDto
    {
        public int ModuleSubModuleId { get; set; }

        [JsonIgnore]
        public string ModuleName { get; set; }

        [JsonIgnore]
        public string SubModuleName { get; set; }

        public List<ActionDto> GrantedPermissions { get; set; }
    }
}