using Microsoft.EntityFrameworkCore.Migrations;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Helper
{
    public static class TemporalTableHelper
    {
        public static void AddTemporalTableSupportExistingTable(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"ALTER TABLE {tableName} ADD
            SysStartTime datetime2(0) GENERATED ALWAYS AS ROW START HIDDEN
            CONSTRAINT DF_{tableName}_SysStartTime DEFAULT SYSUTCDATETIME(),
            SysEndTime datetime2(0) GENERATED ALWAYS AS ROW END HIDDEN CONSTRAINT DF_{tableName}_SysEndTime DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
            PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime);");
            builder.Sql($@"ALTER TABLE {tableName}
SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {historyTableSchema}.{tableName} ));");
        }

        public static void RemoveTemporalTableSupportExisting(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"ALTER TABLE {tableName} SET (SYSTEM_VERSIONING = OFF);");
            builder.Sql($@"ALTER TABLE {tableName} DROP PERIOD FOR SYSTEM_TIME;");
            builder.Sql($@"ALTER TABLE {tableName} DROP CONSTRAINT IF EXISTS DF_{tableName}_SysStartTime;");
            builder.Sql($@"ALTER TABLE {tableName} DROP CONSTRAINT IF EXISTS DF_{tableName}_SysEndTime;");
            builder.Sql($@"ALTER TABLE {tableName} DROP COLUMN SysStartTime;");
            builder.Sql($@"ALTER TABLE {tableName} DROP COLUMN SysEndTime;");
            builder.Sql($@"Drop TABLE {historyTableSchema}.{tableName};");
        }

        public static void AddTemporalTableSupport(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"ALTER TABLE {tableName} ADD
            SysStartTime datetime2(0) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
            SysEndTime datetime2(0) GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
            PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime);");
            builder.Sql($@"ALTER TABLE {tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {historyTableSchema}.{tableName} ));");
        }

        public static void RemoveTemporalTableSupport(this MigrationBuilder builder, string tableName)
        {
            builder.Sql($@"ALTER TABLE {tableName} SET (SYSTEM_VERSIONING = OFF);");
            builder.Sql($@"ALTER TABLE {tableName} DROP PERIOD FOR SYSTEM_TIME;");
            builder.Sql($@"ALTER TABLE {tableName} DROP COLUMN SysStartTime,SysEndTime;");
        }
    }
}