using Abp.MultiTenancy;

using ELog.Core.Authorization.Users;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            //Add Default Constructor code here
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
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
    }
}