using Abp.Authorization.Users;
using Abp.Extensions;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public const int MaxEmployeeCodeLength = 20;

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        [Column("Email")]
        [StringLength(PMMSConsts.Medium)]

        public override string EmailAddress { get; set; }

        [Required]
        [Column("FirstName")]
        [StringLength(PMMSConsts.Medium)]
        public override string Name { get; set; }

        [Required]
        [Column("LastName")]
        [StringLength(PMMSConsts.Medium)]
        public override string Surname { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string EmployeeCode { get; set; }

        [NotMapped]
        public override string EmailConfirmationCode { get; set; }

        [NotMapped]
        public override string PasswordResetCode { get; set; }

        // [NotMapped]
        public override int AccessFailedCount { get; set; }
        // [NotMapped]
        public override DateTime? LockoutEndDateUtc { get; set; }

        [NotMapped]
        public override bool IsLockoutEnabled { get; set; }
        public bool IsLockout { get; set; }

        [NotMapped]
        public override bool IsPhoneNumberConfirmed { get; set; }

        [NotMapped]
        public override string SecurityStamp { get; set; }

        [NotMapped]
        public override bool IsTwoFactorEnabled { get; set; }

        [NotMapped]
        public override bool IsEmailConfirmed { get; set; }

        [ForeignKey("ReportingManagerId")]
        public User Users { get; set; }

        public int ApprovalStatusId { get; set; }

        public long? ReportingManagerId { get; set; }
        public int? PlantId { get; set; }

        public int? ModeId { get; set; }

        public int? DesignationId { get; set; }

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

        public string Description { get; set; }

        public ICollection<UserPlants> UserPlants { get; set; }
        public string ApprovalStatusDescription { get; set; }
        public int PasswordStatus { get; set; }
        public DateTime? PasswordResetTime { get; set; }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress != null ? emailAddress : null,
                Roles = new List<UserRole>()
            };

            //user.SetNormalizedNames();

            return user;
        }
        public static User CreateTenantSuperAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = PMMSConsts.SuperAdminUserName,
                Name = PMMSConsts.SuperAdminUserName,
                Surname = PMMSConsts.SuperAdminUserName,
                EmailAddress = emailAddress != null ? emailAddress : null,
                Roles = new List<UserRole>()
            };

            //user.SetNormalizedNames();

            return user;
        }
    }
}