using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.DeviceTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class DeviceTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public DeviceTypeMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingCountries = _context.DeviceTypeMaster.IgnoreQueryFilters().Select(x => x.DeviceName).ToList() ?? default;

            var equipmentTypes = SeedHelper.SeedEntityData<string>(PMMSConsts.DeviceTypeMasterSeedFilePath).Where(x => !existingCountries.Contains(x));
            foreach (var equipmentType in equipmentTypes)
            {
                if (_context.DeviceTypeMaster.IgnoreQueryFilters().Any(l => l.DeviceName == equipmentType))
                {
                    continue;
                }
                var equipmentTypeMaster = new ELog.Core.Entities.DeviceTypeMaster
                {
                    DeviceName = equipmentType,
                    IsDeleted = false
                };
                _context.DeviceTypeMaster.Add(equipmentTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}