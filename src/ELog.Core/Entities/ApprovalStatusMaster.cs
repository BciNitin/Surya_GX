using ELog.Core.Authorization;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ApprovalStatusMaster")]
    public class ApprovalStatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string ApprovalStatus { get; set; }

        [ForeignKey("ApprovalStatusId")]
        public ICollection<Role> Roles { get; set; }

        [ForeignKey("ApprovalStatusId")]
        public ICollection<User> Users { get; set; }

        public string Description { get; set; }
    }
}