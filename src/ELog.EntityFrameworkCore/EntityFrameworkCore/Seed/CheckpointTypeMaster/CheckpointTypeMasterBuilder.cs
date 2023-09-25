using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CheckpointTypeMaster
{
    [ExcludeFromCodeCoverage]
    public class CheckpointTypeMasterBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public CheckpointTypeMasterBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingCheckpoints = _context.CheckpointTypeMaster.IgnoreQueryFilters().Select(x => x.Title).ToList() ?? default;

            var checkpoints = SeedHelper.SeedEntityData<string>(PMMSConsts.CheckpointTypeMasterSeedFilePath).Where(x => !existingCheckpoints.Contains(x));
            foreach (var checkpoint in checkpoints)
            {
                if (_context.CheckpointTypeMaster.IgnoreQueryFilters().Any(l => l.Title == checkpoint))
                {
                    continue;
                }
                var checkpointTypeMaster = new ELog.Core.Entities.CheckpointTypeMaster
                {
                    Title = checkpoint,
                    IsDeleted = false
                };
                _context.CheckpointTypeMaster.Add(checkpointTypeMaster);
                _context.SaveChanges();
            }
        }
    }
}