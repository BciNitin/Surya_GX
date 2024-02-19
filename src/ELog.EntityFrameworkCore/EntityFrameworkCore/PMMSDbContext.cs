using Abp.Application.Editions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Elog.Core.Entities;
using ELog.Core;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.MultiTenancy;
using ELog.Core.SQLDtoEntities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    [AutoRepositoryTypes(
    typeof(IRepository<>),
    typeof(IRepository<,>),
    typeof(PMMSRepositoryBase<>),
    typeof(PMMSRepositoryBase<,>)
)]
    public class PMMSDbContext : AbpZeroDbContext<Tenant, Role, User, PMMSDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ApprovalStatusMaster> ApprovalStatusMaster { get; set; }
        public DbSet<PlantMaster> PlantMaster { get; set; }

        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<ModeMaster> ModeMaster { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<ModuleMaster> ModuleMaster { get; set; }
        public DbSet<SubModuleMaster> SubModuleMaster { get; set; }
        public DbSet<PermissionMaster> PermissionMaster { get; set; }
        public DbSet<ModuleSubModule> ModuleSubModule { get; set; }

        public DbSet<SubModuleTypeMaster> SubModuleTypeMaster { get; set; }
        public DbSet<ChecklistTypeMaster> ChecklistTypeMaster { get; set; }
        public DbSet<UserPlants> UserPlants { get; set; }
        public DbSet<CheckpointTypeMaster> CheckpointTypeMaster { get; set; }
        public DbSet<CheckpointMaster> CheckpointMaster { get; set; }


        public DbSet<ApprovalLevelMaster> ApprovalLevelMaster { get; set; }
        public DbSet<ApprovalUserModuleMappingMaster> ApprovalUserModuleMappingMaster { get; set; }

        public DbSet<ActivityMaster> ActivityMaster { get; set; }

        public DbSet<AuditReportDetailsDto> AuditSQLResult { get; set; }

        public DbSet<StatusMaster> StatusMaster { get; set; }

        public DbSet<AreaUsageLog> AreaUsageLog { get; set; }

        public DbSet<AreaUsageListLog> AreaUsageListLog { get; set; }


        //public DbSet<WeightVerificationDetail> WeightVerificationDetail { get; set; }




        public DbSet<PostWIPDataToSAP> PostWIPDataToSAP { get; set; }
        public DbSet<PalletMaster> PalletMaster { get; set; }
        public DbSet<OBDDetails> OBDDetails { get; set; }


        public DbSet<WMSPasswordManager> WMSPasswordManager { get; set; }

        public DbSet<LogoMaster> LogoMasters { get; set; }
        public DbSet<Loading> Loading { get; set; }


        public DbSet<ClientForm> ClientForm { get; set; }
        public DbSet<ElogControl> ElogControl { get; set; }
        public DbSet<ZMaster> ZMaster { get; set; }

        public DbSet<FormApproval> FormApproval { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<LogFormHistory> LogFormHistory { get; set; }
        public PMMSDbContext(DbContextOptions<PMMSDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>(""); //Removes table prefixes. You can specify another prefix.
            modelBuilder.ApplyUtcDateTimeConverter();
            ApplyUniqueConstraintColumns(modelBuilder);
            ApplyColumnNotNullables(modelBuilder);
            ApplyColumnDefaultValues(modelBuilder);
            RenameColumns(modelBuilder);
           // ApplySequence(modelBuilder);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.DeleterUser);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.CreatorUser);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.LastModifierUser);

            modelBuilder.Entity<UserPlants>()
                .HasOne<PlantMaster>(sc => sc.PlantMaster)
                .WithMany(s => s.UserPlants)
                .HasForeignKey(sc => sc.PlantId);

            modelBuilder.Entity<UserPlants>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserPlants)
                .HasForeignKey(sc => sc.UserId);
        }

        
        private void ApplyColumnDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p => p.IsActive).HasDefaultValue(true);
        }

        private void ApplyUniqueConstraintColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatusMaster>(x => x.HasIndex(x => x.ApprovalStatus).IsUnique(true));
        }

        private void ApplyColumnNotNullables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatusMaster>().Property(p => p.ApprovalStatus).IsRequired(true);
            modelBuilder.Entity<PlantMaster>().Property(p => p.PlantName).IsRequired(true);
            modelBuilder.Entity<RolePermissions>().Property(p => p.PermissionName).IsRequired(true);
            modelBuilder.Entity<Role>().Property(p => p.ApprovalStatusId).IsRequired(true);
            modelBuilder.Entity<ModuleMaster>().Property(p => p.Description).IsRequired(true);
            modelBuilder.Entity<SubModuleMaster>().Property(p => p.Description).IsRequired(true);
        }

        private void RenameColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackgroundJobInfo>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<BackgroundJobInfo>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<Edition>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<Edition>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<Edition>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<Edition>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<Edition>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<Edition>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");

            modelBuilder.Entity<OrganizationUnitRole>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<OrganizationUnitRole>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<PermissionSetting>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<PermissionSetting>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<RoleClaim>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<RoleClaim>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<Setting>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<Setting>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<Setting>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<Setting>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");

            modelBuilder.Entity<UserAccount>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<UserAccount>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<UserAccount>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<UserClaim>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserClaim>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserOrganizationUnit>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserOrganizationUnit>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserRole>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserRole>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserLoginAttempt>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
        }
    }
}