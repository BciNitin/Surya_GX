using Abp.Authorization.Users;
using ELog.Core;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static ELog.Core.PMMSEnums;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Tenants
{
    [ExcludeFromCodeCoverage]
    public class TenantRoleAndUserBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // super admin role

            var superAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.SuperAdmin);
            if (superAdminRole == null)
            {
                superAdminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.SuperAdmin, StaticRoleNames.Tenants.SuperAdmin) { IsStatic = true, IsActive = true, ApprovalStatusId = (int)ApprovalStatus.Approved }).Entity;
                _context.SaveChanges();
            }
            //super admin user
            AddSuperAdminUser(superAdminRole);
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true, IsActive = true, ApprovalStatusId = (int)ApprovalStatus.Approved }).Entity;
                _context.SaveChanges();
            }

            // Admin user
            AddAdminUser(adminRole);
        }

        private void AddAdminUser(Role adminRole)
        {
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser != null)
            {
                return;
            }
            adminUser = User.CreateTenantAdminUser(_tenantId, PMMSConsts.TenantAdminEmailAddress);
            adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "Bci@123");
            adminUser.UserName = "admin";
            adminUser.IsEmailConfirmed = true;
            adminUser.IsActive = true;
            adminUser.ApprovalStatusId = (int)ApprovalStatus.Approved;
            _context.Users.Add(adminUser);
            _context.SaveChanges();

            // Assign Admin role to admin user
            _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
            _context.SaveChanges();
        }
        private void AddSuperAdminUser(Role superAdminRole)
        {
            var superAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == PMMSConsts.SuperAdminUserName);
            if (superAdminUser != null)
            {
                return;
            }
            superAdminUser = User.CreateTenantSuperAdminUser(_tenantId, PMMSConsts.AdminHostEmailAddress);
            superAdminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(superAdminUser, "Bci@123");
            superAdminUser.UserName = "superadmin";
            superAdminUser.IsEmailConfirmed = true;
            superAdminUser.IsActive = true;
            superAdminUser.ApprovalStatusId = (int)ApprovalStatus.Approved;
            _context.Users.Add(superAdminUser);
            _context.SaveChanges();

            // Assign Super Admin role to admin user
            _context.UserRoles.Add(new UserRole(_tenantId, superAdminUser.Id, superAdminRole.Id));
            _context.SaveChanges();
        }
    }
}