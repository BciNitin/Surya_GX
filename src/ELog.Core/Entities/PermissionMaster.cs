using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PermissionMaster")]
    public class PermissionMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Action { get; set; }

        [ForeignKey("PermissionId")]
        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}