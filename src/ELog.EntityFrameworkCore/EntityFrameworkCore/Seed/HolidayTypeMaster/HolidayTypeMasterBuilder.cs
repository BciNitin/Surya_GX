using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.HolidayTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class HolidayTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public HolidayTypeMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingHolidayTypes = _context.HolidayTypeMaster.IgnoreQueryFilters().Select(x => x.HolidayType).ToList() ?? default;

            var holidayTypes = SeedHelper.SeedEntityData<string>(PMMSConsts.HolidayTypeMasterSeedFilePath).Where(x => !existingHolidayTypes.Contains(x));
            foreach (var holidayType in holidayTypes)
            {
                if (_context.HolidayTypeMaster.IgnoreQueryFilters().Any(l => l.HolidayType == holidayType))
                {
                    continue;
                }
                var holidayTypeMaster = new ELog.Core.Entities.HolidayTypeMaster
                {
                    HolidayType = holidayType,
                    IsDeleted = false
                };
                _context.HolidayTypeMaster.Add(holidayTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}