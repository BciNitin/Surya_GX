using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class MaterialTransferTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public MaterialTransferTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateMaterialTransferTypeMaster();
        }

        private void CreateMaterialTransferTypeMaster()
        {
            foreach (var status in SeedHelper.SeedEntityData<MaterialTransferTypeMaster>(PMMSConsts.MaterialTransferTypeSeedFilePath))
            {
                if (_context.MaterialTransferTypeMaster.IgnoreQueryFilters().Any(l => l.TransferType == status.TransferType))
                {
                    return;
                }
                status.CreationTime = DateTime.UtcNow;
                _context.MaterialTransferTypeMaster.Add(status);
            }
            _context.SaveChanges();
        }
    }
}