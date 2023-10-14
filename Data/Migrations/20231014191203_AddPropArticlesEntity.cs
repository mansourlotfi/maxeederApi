using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropArticlesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescEn",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "434dbe3e-c2f3-49dc-ad98-48ca70c06c31");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4ff15a99-8bcb-45ae-a698-aba80b1523ab");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ShortDescEn",
                table: "Articles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4a0907f8-47fa-4588-970c-6d1a62c6c325");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e20e770c-c3fe-434f-84af-c3ddeb3ae04a");
        }
    }
}
