using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DesignationMaster")]
    public class DesignationMaster : PMMSFullAudit
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DesignationName { get; set; }

        [ForeignKey("DesignationId")]
        public ICollection<User> Users { get; set; }

        public string Description { get; set; }
    }
}