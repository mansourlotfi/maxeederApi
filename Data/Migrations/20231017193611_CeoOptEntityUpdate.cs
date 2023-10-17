using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class CeoOptEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CeoOptimizations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Page",
                table: "CeoOptimizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "CeoOptimizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "be74bb45-75e2-43c1-a9df-01806e3ee193");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "253856aa-c63d-4bcb-a8d5-075b0e427b7d");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "CeoOptimizations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d1be2700-4352-4502-9aba-b03c404754b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1108f521-a520-45d6-bee2-12c5021565aa");
        }
    }
}
