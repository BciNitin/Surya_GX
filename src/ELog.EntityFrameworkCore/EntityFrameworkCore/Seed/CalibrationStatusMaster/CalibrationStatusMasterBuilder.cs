using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CalibrationStatusMaster
{
    [ExcludeFromCodeCoverage]
    public class CalibrationStatusMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public CalibrationStatusMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingCalibrationStatus = _context.CalibrationStatusMaster.IgnoreQueryFilters().Select(x => x.StatusName).ToList() ?? default;

            var calibrationStatuses = SeedHelper.SeedEntityData<string>(PMMSConsts.CalibrationStatusMasterSeedFilePath).Where(x => !existingCalibrationStatus.Contains(x));
            foreach (var calibrationStatus in calibrationStatuses)
            {
                if (_context.CalibrationStatusMaster.IgnoreQueryFilters().Any(l => l.StatusName == calibrationStatus))
                {
                    continue;
                }
                var calibrationStatusMaster = new ELog.Core.Entities.CalibrationStatusMaster
                {
                    StatusName = calibrationStatus,
                    IsDeleted = false
                };
                _context.CalibrationStatusMaster.Add(calibrationStatusMaster);
                _context.SaveChanges();
            }
        }
    }
}