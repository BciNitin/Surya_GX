using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.UnitOfMeasurementTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class UnitOfMeasurementTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public UnitOfMeasurementTypeMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingUOM = _context.UnitOfMeasurementTypeMaster.IgnoreQueryFilters().Select(x => x.UnitOfMeasurementTypeName).ToList() ?? default;

            var unitOfMeasurementTypes = SeedHelper.SeedEntityData<string>(PMMSConsts.UnitOfMeasurementTypeMaasterSeedFilePath).Where(x => !existingUOM.Contains(x));
            foreach (var unitOfMeasurementType in unitOfMeasurementTypes)
            {
                var unitOfMeasurementTypeMaster = new ELog.Core.Entities.UnitOfMeasurementTypeMaster
                {
                    UnitOfMeasurementTypeName = unitOfMeasurementType,
                    IsDeleted = false
                };
                _context.UnitOfMeasurementTypeMaster.Add(unitOfMeasurementTypeMaster);
            }
            _context.SaveChanges();
        }
    }
}