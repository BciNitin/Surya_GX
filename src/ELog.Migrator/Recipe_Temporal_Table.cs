using ELog.EntityFrameworkCore.EntityFrameworkCore.Helper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELog.EntityFrameworkCore.Migrations
{
    public partial class Recipe_Temporal_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE SCHEMA History");
            migrationBuilder.AddTemporalTableSupportExistingTable("RecipeTransactionHeader", "History");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RemoveTemporalTableSupportExisting("RecipeTransactionHeader", "History");
        }
    }
}
