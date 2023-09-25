using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.FrequencyTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class FrequencyTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public FrequencyTypeMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingFrequencyType = _context.FrequencyTypeMaster.IgnoreQueryFilters().Select(x => x.FrequencyName).ToList() ?? default;

            var frequencyTypes = SeedHelper.SeedEntityData<string>(PMMSConsts.FrequencyTypeMasterSeedFilePath).Where(x => !existingFrequencyType.Contains(x));
            foreach (var frequencyType in frequencyTypes)
            {
                if (_context.FrequencyTypeMaster.IgnoreQueryFilters().Any(l => l.FrequencyName == frequencyType))
                {
                    continue;
                }
                var frequencyTypeMaster = new ELog.Core.Entities.FrequencyTypeMaster
                {
                    FrequencyName = frequencyType,
                    IsDeleted = false
                };
                _context.FrequencyTypeMaster.Add(frequencyTypeMaster);
                _context.SaveChanges();
            }
        }
    }
}