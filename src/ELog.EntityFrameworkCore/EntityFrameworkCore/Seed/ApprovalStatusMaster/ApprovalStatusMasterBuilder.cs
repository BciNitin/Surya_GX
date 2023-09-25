using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class ApprovalStatusMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public ApprovalStatusMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateApprovalStatusMaster();
        }

        private void CreateApprovalStatusMaster()
        {
            foreach (var status in SeedHelper.SeedEntityData<ApprovalStatusMaster>(PMMSConsts.ApprovalStatusSeedFilePath))
            {
                if (_context.ApprovalStatusMaster.IgnoreQueryFilters().Any(l => l.ApprovalStatus == status.ApprovalStatus))
                {
                    return;
                }
                status.CreationTime = DateTime.UtcNow;
                _context.ApprovalStatusMaster.Add(status);
            }
            _context.SaveChanges();
        }
    }
}