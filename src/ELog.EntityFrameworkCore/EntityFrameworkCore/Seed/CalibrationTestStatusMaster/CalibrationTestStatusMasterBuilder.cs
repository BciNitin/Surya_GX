using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CalibrationTestStatusMaster
{
    [ExcludeFromCodeCoverage]
    public class CalibrationTestStatusMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public CalibrationTestStatusMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingCalibrationTestStatus = _context.CalibrationTestStatusMaster.IgnoreQueryFilters().Select(x => x.StatusName).ToList() ?? default;

            var calibrationTestStatuses = SeedHelper.SeedEntityData<string>(PMMSConsts.CalibrationTestStatusMasterSeedFilePath).Where(x => !existingCalibrationTestStatus.Contains(x));
            foreach (var calibrationTestStatus in calibrationTestStatuses)
            {
                if (_context.CalibrationTestStatusMaster.IgnoreQueryFilters().Any(l => l.StatusName == calibrationTestStatus))
                {
                    continue;
                }
                var calibrationTestStatusMaster = new ELog.Core.Entities.CalibrationTestStatusMaster
                {
                    StatusName = calibrationTestStatus,
                    IsDeleted = false
                };
                _context.CalibrationTestStatusMaster.Add(calibrationTestStatusMaster);
                _context.SaveChanges();
            }
        }
    }
}