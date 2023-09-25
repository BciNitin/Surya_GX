using ELog.Core;
using ELog.Core.Configuration;
using ELog.Core.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    [ExcludeFromCodeCoverage]
    public class PMMSDbContextFactory : IDesignTimeDbContextFactory<PMMSDbContext>
    {
        public PMMSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PMMSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PMMSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PMMSConsts.ConnectionStringName));

            return new PMMSDbContext(builder.Options);
        }
    }
}
