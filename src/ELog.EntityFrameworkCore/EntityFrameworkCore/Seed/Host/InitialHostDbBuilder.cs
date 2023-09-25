using System.Diagnostics.CodeAnalysis;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class InitialHostDbBuilder
    {
        private readonly PMMSDbContext _context;

        public InitialHostDbBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
