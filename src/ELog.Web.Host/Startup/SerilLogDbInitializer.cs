using Destructurama;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Sinks.MSSqlServer;

using System.Collections.ObjectModel;
using System.Data;
using System.IO;

namespace ELog.Web.Core.Auditing
{
    public static class SerilLogDbInitializer
    {
        private const string _connectionString = "Server=localhost;Database=pmmsdev;Integrated Security=SSPI;";
        private const string _tableName = "Logging";
        public static IConfigurationRoot Configuration { get; set; }

        public static LoggerConfiguration CreateLoggerConfiguration()
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithMachineName()
                .Destructure.UsingAttributes()
                .Enrich.With(new PropertyBagEnricher())
                .Enrich.With(new RemovePropertyBagEnricher());
            //.WriteTo.MSSqlServer(
            //_connectionString,
            //new MSSqlServerSinkOptions
            //{
            //    TableName = _tableName,
            //    AutoCreateSqlTable = true
            //},
            //sinkOptionsSection: null,
            //appConfiguration: null,
            //restrictedToMinimumLevel: LogEventLevel.Information,
            //formatProvider: null,
            //columnOptions: BuildColumnOptions(),
            //columnOptionsSection: null,
            //logEventFormatter: null);
        }

        private static ColumnOptions BuildColumnOptions()
        {
            var columnOptions = new ColumnOptions
            {
                TimeStamp =
                {
                    ColumnName = "TimeStamp",
                    ConvertToUtc = false,
                },

                AdditionalColumns = new Collection<SqlColumn>
                {
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=512, ColumnName = "BrowserInfo" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=64, ColumnName = "ClientIpAddress" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=128, ColumnName = "ClientName" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=2000, ColumnName = "CustomData" },
                  new SqlColumn { DataType = SqlDbType.Int,AllowNull=true, ColumnName = "ExecutionDuration" },
                  new SqlColumn { DataType = SqlDbType.DateTime2,DataLength=7, AllowNull=true,ColumnName = "ExecutionTime" },
                  new SqlColumn { DataType = SqlDbType.Int, ColumnName = "ImpersonatorTenantId" },
                  new SqlColumn { DataType = SqlDbType.BigInt, ColumnName = "ImpersonatorUserId" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=256, ColumnName = "MethodName" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=1024,ColumnName = "Parameters" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=256, ColumnName = "ServiceName" },
                  new SqlColumn { DataType = SqlDbType.Int, ColumnName = "TenantId" },
                  new SqlColumn { DataType = SqlDbType.BigInt, ColumnName = "UserId" },
                  new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "ReturnValue" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=25,ColumnName = "ApplicationName" },
                  new SqlColumn { DataType = SqlDbType.NVarChar,DataLength=25,ColumnName = "MachineName" },
                  new SqlColumn { DataType = SqlDbType.Int, ColumnName = "ProcessId" },
                  new SqlColumn { DataType = SqlDbType.Int, ColumnName = "ThreadId" },

                }

            };

            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.Message);

            return columnOptions;
        }
    }
}
