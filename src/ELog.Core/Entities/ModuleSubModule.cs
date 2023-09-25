
using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ModuleSubModule")]
    public class ModuleSubModule : PMMSFullAudit
    {
        public int ModuleId { get; set; }

        public int SubModuleId { get; set; }

        public bool IsSelected { get; set; }
        public bool IsMandatory { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("ModuleSubModuleId")]
        public virtual ICollection<RolePermissions> Permissions { get; set; }
    }
}