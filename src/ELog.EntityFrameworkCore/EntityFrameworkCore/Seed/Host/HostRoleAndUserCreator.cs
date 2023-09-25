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

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class HostRoleAndUserCreator
    {
        private readonly PMMSDbContext _context;

        public HostRoleAndUserCreator(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            // Admin role for host

            var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true, IsActive = true, ApprovalStatusId = (int)ApprovalStatus.Approved }).Entity;
                _context.SaveChanges();
            }

            // Admin user for host

            var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost != null)
            {
                return;
            }
            var user = new User
            {
                TenantId = null,
                UserName = AbpUserBase.AdminUserName,
                Name = "admin",
                Surname = "admin",
                EmailAddress = PMMSConsts.AdminHostEmailAddress,
                IsEmailConfirmed = true,
                IsActive = true,
                ApprovalStatusId = (int)ApprovalStatus.Approved
            };

            user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
            user.SetNormalizedNames();

            adminUserForHost = _context.Users.Add(user).Entity;
            _context.SaveChanges();

            // Assign Admin role to admin user
            _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
            _context.SaveChanges();
        }
    }
}