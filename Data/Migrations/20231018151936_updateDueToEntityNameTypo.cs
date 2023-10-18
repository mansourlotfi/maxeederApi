using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDueToEntityNameTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CeoOptimizations",
                table: "CeoOptimizations");

            migrationBuilder.RenameTable(
                name: "CeoOptimizations",
                newName: "SeoOptimizations");

            migrationBuilder.RenameIndex(
                name: "IX_CeoOptimizations_Priority",
                table: "SeoOptimizations",
                newName: "IX_SeoOptimizations_Priority");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeoOptimizations",
                table: "SeoOptimizations",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6a795281-e928-4a0a-a431-36c7d556cbbc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2252d3eb-45d5-454d-b8f3-6f006bf7ff55");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeoOptimizations",
                table: "SeoOptimizations");

            migrationBuilder.RenameTable(
                name: "SeoOptimizations",
                newName: "CeoOptimizations");

            migrationBuilder.RenameIndex(
                name: "IX_SeoOptimizations_Priority",
                table: "CeoOptimizations",
                newName: "IX_CeoOptimizations_Priority");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeoOptimizations",
                table: "CeoOptimizations",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f82e6101-1e55-4368-8af3-1781cbff2169");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ac61fbdc-bc97-4929-9a90-d853481528a8");
        }
    }
}
