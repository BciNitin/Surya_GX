using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class ActionBuilder
    {
        private readonly PMMSDbContext _context;

        public ActionBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateActions();
        }

        private void CreateActions()
        {
            foreach (var action in SeedHelper.SeedEntityData<PermissionMaster>(PMMSConsts.ActionsSeedFilePath))
            {
                if (_context.PermissionMaster.IgnoreQueryFilters().Any(l => l.Action == action.Action))
                {
                    continue;
                }
                action.CreationTime = DateTime.UtcNow;
                _context.PermissionMaster.Add(action);
                _context.SaveChanges();
            }
        }
    }
}