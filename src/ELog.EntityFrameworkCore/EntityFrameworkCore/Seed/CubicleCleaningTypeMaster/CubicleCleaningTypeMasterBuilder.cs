using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CubicleCleaningTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class CubicleCleaningTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public CubicleCleaningTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var existingCubicleCleaningType = _context.CubicleCleaningTypeMaster.IgnoreQueryFilters().Select(x => x.Value).ToList() ?? default;
            var cubicleCleaningType = SeedHelper.SeedEntityData<string>(PMMSConsts.CubicleCleaningTypeMasterSeedFilePath).Where(x => !existingCubicleCleaningType.Contains(x));
            foreach (var cleaningValue in cubicleCleaningType)
            {
                if (_context.CubicleCleaningTypeMaster.IgnoreQueryFilters().Any(l => l.Value == cleaningValue))
                {
                    continue;
                }
                var cubicleCleaningTypeMaster = new ELog.Core.Entities.CubicleCleaningTypeMaster
                {
                    Value = cleaningValue,
                    IsDeleted = false
                };
                _context.CubicleCleaningTypeMaster.Add(cubicleCleaningTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}
