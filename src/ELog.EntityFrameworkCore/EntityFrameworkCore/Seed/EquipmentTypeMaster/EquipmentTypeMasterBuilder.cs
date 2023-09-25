using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.EquipmentTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class EquipmentTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public EquipmentTypeMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var existingEquipments = _context.EquipmentTypeMaster.IgnoreQueryFilters().Select(x => x.EquipmentName).ToList() ?? default;

            var equipmentTypes = SeedHelper.SeedEntityData<string>(PMMSConsts.EquipmentTypeMasterSeedFilePath).Where(x => !existingEquipments.Contains(x));
            foreach (var equipmentType in equipmentTypes)
            {
                var equipmentTypeMaster = new ELog.Core.Entities.EquipmentTypeMaster
                {
                    EquipmentName = equipmentType,
                    IsDeleted = false
                };
                _context.EquipmentTypeMaster.Add(equipmentTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}