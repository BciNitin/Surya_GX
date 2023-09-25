using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class SubModuleTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public SubModuleTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateSubModuleTypeMaster();
        }

        private void CreateSubModuleTypeMaster()
        {
            foreach (var subModuleType in SeedHelper.SeedEntityData<SubModuleTypeMaster>(PMMSConsts.SubModuleTypeSeedFilePath))
            {
                if (_context.SubModuleTypeMaster.IgnoreQueryFilters().Any(l => l.SubModuleType == subModuleType.SubModuleType))
                {
                    continue;
                }
                subModuleType.CreationTime = DateTime.UtcNow;
                _context.SubModuleTypeMaster.Add(subModuleType);
                _context.SaveChanges();
            }
        }
    }
}