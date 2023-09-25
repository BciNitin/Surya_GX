using Abp.Authorization.Roles;

using ELog.Core.Authorization.Users;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 5000;

        public Role()
        {
            //Add Default Constructor code here
        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        [Column("CreatedBy")]
        public override long? CreatorUserId { get; set; }

        [Column("CreatedOn")]
        public override DateTime CreationTime { get; set; }

        [Column("ModifiedBy")]
        public override long? LastModifierUserId { get; set; }

        [Column("ModifiedOn")]
        public override DateTime? LastModificationTime { get; set; }

        [Column("DeletedBy")]
        public override long? DeleterUserId { get; set; }

        [Column("DeletedOn")]
        public override DateTime? DeletionTime { get; set; }

        public int ApprovalStatusId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string ApprovalStatusDescription { get; set; }

        [ForeignKey("RoleId")]
        public ICollection<RolePermissions> RolePermissions { get; set; }

        public string Description { get; set; }
    }
}