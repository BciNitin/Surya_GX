using ELog.Core;
using ELog.Core.Authorization.Roles;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class DefaultRolePermissionBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public DefaultRolePermissionBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            //CreateDefaultRolePermissions();
        }

        private void CreateDefaultRolePermissions()
        {
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            var superadminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.SuperAdmin);
            var adminRolePermssions = _context.SubModuleMaster.Where(a => a.Name == PMMSPermissionConst.User_SubModule || a.Name == PMMSPermissionConst.Role_SubModule).Select(x => x.Id).ToList();
            var superAdminRolePermssions = _context.SubModuleMaster.Where(a => a.Name == PMMSPermissionConst.Module || a.Name == PMMSPermissionConst.SubModule || a.Name == PMMSPermissionConst.Plant_SubModule || a.Name == PMMSPermissionConst.Password_SubModule).Select(x => x.Id).ToList();
            AddAdminRolePermission(adminRole, adminRolePermssions);
            AddAdminRolePermission(superadminRole, superAdminRolePermssions);
        }

        private List<PermissionMaster> AddAdminRolePermission(Role adminRole, List<int> grantedModuleIds)
        {
            var permissions = new List<PermissionMaster>();
            var moduleSubModules = _context.ModuleSubModule.Where(p => grantedModuleIds.Contains(p.SubModuleId)).ToList();
            var allPermissions = _context.PermissionMaster.ToList();

            foreach (var moduleSubModule in moduleSubModules)
            {
                var subModule = _context.SubModuleMaster.IgnoreQueryFilters().FirstOrDefault(a => a.Id == moduleSubModule.SubModuleId);
                permissions = allPermissions;
                if (subModule.Name == PMMSConsts.ModuleName || subModule.Name == PMMSConsts.SubModuleName)
                {
                    permissions = permissions.Where(a => a.Action != PMMSConsts.AddPermission && a.Action != PMMSConsts.DeletePermission).ToList();
                }
                else if (subModule.Name == PMMSConsts.GateEntrySubModule)
                {
                    permissions = permissions.Where(a => a.Action != PMMSConsts.DeletePermission).ToList();
                }
                foreach (var permission in permissions)
                {
                    var existingPermission = _context.RolePermissions.IgnoreQueryFilters().FirstOrDefault(l => l.RoleId == adminRole.Id && l.ModuleSubModuleId == moduleSubModule.Id && l.PermissionId == permission.Id);
                    if (existingPermission == null)
                    {
                        RolePermissions rolePermission = new RolePermissions
                        {
                            ModuleSubModuleId = moduleSubModule.Id,
                            PermissionId = permission.Id,
                            PermissionName = subModule != null ? subModule.Name + "." + permission.Action : null,
                            RoleId = adminRole.Id,
                            IsDeleted = false,
                            CreationTime = DateTime.UtcNow
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }
            }
            _context.SaveChanges();
            return permissions;
        }
    }
}