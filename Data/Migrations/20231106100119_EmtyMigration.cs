using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmtyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7fc4080e-6421-4f5e-b4bd-f41c44c5a52a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b70fef68-1fdc-43d5-b180-fd3ecc04a21e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e9ce6d7e-2377-47f3-bbd6-94aa39c78482");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0360f135-6692-45d2-9e94-70cd056bd9e0");
        }
    }
}
