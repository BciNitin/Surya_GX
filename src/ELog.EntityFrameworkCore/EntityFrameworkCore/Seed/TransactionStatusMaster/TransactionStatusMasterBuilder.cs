using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class TransactionStatusMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public TransactionStatusMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateTransactionStatusMaster();
        }

        private void CreateTransactionStatusMaster()
        {
            foreach (var status in SeedHelper.SeedEntityData<TransactionStatusMaster>(PMMSConsts.TransactionStatusSeedFilePath))
            {
                if (_context.TransactionStatusMaster.IgnoreQueryFilters().Any(l => l.TransactionStatus == status.TransactionStatus))
                {
                    continue;
                }
                status.CreationTime = DateTime.UtcNow;
                _context.TransactionStatusMaster.Add(status);
                _context.SaveChanges();
            }
        }
    }
}