using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ModeMaster")]
    public class ModeMaster : PMMSFullAudit
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ModeName { get; set; }

        public string Description { get; set; }

        public bool IsController { get; set; }
        [ForeignKey("ModeId")]
        public ICollection<User> Users { get; set; }

        [ForeignKey("ModeId")]
        public ICollection<DeviceMaster> DeviceMasters { get; set; }
    }
}