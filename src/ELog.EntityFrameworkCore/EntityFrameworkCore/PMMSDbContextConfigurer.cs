using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    public static class PMMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PMMSDbContext> builder, string connectionString)
        {
           // builder.UseSqlServer(connectionString);
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PMMSDbContext> builder, DbConnection connection)
        {
            //builder.UseSqlServer(connection);
            builder.UseMySql(connection);
        }
    }
}