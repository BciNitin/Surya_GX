using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CubicleCleaningTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class EquipmentCleaningTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public EquipmentCleaningTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var existingequipmentCleaningType = _context.EquipmentCleaningTypeMaster.IgnoreQueryFilters().Select(x => x.Value).ToList() ?? default;
            var equipmentCleaningType = SeedHelper.SeedEntityData<string>(PMMSConsts.EquipmentCleaningTypeMasterSeedFilePath).Where(x => !existingequipmentCleaningType.Contains(x));
            foreach (var cleaningValue in equipmentCleaningType)
            {
                if (_context.EquipmentCleaningTypeMaster.IgnoreQueryFilters().Any(l => l.Value == cleaningValue))
                {
                    continue;
                }
                var equipmentCleaningTypeMaster = new ELog.Core.Entities.EquipmentCleaningTypeMaster
                {
                    Value = cleaningValue,
                    IsDeleted = false
                };
                _context.EquipmentCleaningTypeMaster.Add(equipmentCleaningTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}
