using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using ELog.Core.Authorization.Users;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Authorization
{
    public class PMMSFullAudit : Entity<int>, IFullAudited
    {
        [Column("CreatedBy")]
        public long? CreatorUserId { get; set; }

        [Column("CreatedOn")]
        public DateTime CreationTime { get; set; }

        [Column("ModifiedBy")]
        public long? LastModifierUserId { get; set; }

        [Column("ModifiedOn")]
        public DateTime? LastModificationTime { get; set; }

        [Column("DeletedBy")]
        public long? DeleterUserId { get; set; }

        [Column("DeletedOn")]
        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User DeleterUser { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User LastModifierUser { get; set; }
    }
}
