using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class SamplingTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public SamplingTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateSamplingTypeMaster();
        }

        private void CreateSamplingTypeMaster()
        {
            foreach (var status in SeedHelper.SeedEntityData<SamplingTypeMaster>(PMMSConsts.SamplingTypeMasterSeedFilePath))
            {
                if (_context.SamplingTypeMaster.IgnoreQueryFilters().Any(l => l.Type == status.Type))
                {
                    continue;
                }
                status.CreationTime = DateTime.UtcNow;
                _context.SamplingTypeMaster.Add(status);
                _context.SaveChanges();
            }
        }
    }
}