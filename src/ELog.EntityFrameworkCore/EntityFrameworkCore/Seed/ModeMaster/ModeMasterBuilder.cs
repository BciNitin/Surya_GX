using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class ModeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public ModeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateModeMaster();
        }

        private void CreateModeMaster()
        {
            foreach (var mode in SeedHelper.SeedEntityData<ModeMaster>(PMMSConsts.ModeMasterSeedFilePath))
            {
                if (_context.ModeMaster.IgnoreQueryFilters().Any(l => l.ModeName == mode.ModeName))
                {
                    return;
                }
                mode.CreationTime = DateTime.UtcNow;
                _context.ModeMaster.Add(mode);
                _context.SaveChanges();
            }

        }
    }
}