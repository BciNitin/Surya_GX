using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("RolePermissions")]
    public class RolePermissions : PMMSFullAudit
    {
        public int ModuleSubModuleId { get; set; }

        [ForeignKey("PermissionId")]
        public int PermissionId { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PermissionName { get; set; }
    }
}