using Abp.Domain.Entities.Auditing;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Authorization
{
    public class PMMSFullAuditedEntity : FullAuditedEntity
    {
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



    }
}
