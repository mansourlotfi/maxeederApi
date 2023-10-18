using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeoOptimazationEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTagDescriptionEn",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTagKeyWordsEn",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextEn",
                table: "CeoOptimizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3e8d213a-23ef-4dda-9e91-2a4bd61b95d7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e1cff833-acda-4fd9-9746-b67199911fbb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "MetaTagDescriptionEn",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "MetaTagKeyWordsEn",
                table: "CeoOptimizations");

            migrationBuilder.DropColumn(
                name: "TextEn",
                table: "CeoOptimizations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "12c574fb-adf0-4d52-938d-506ea646d1bb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "64996ec9-515a-49ed-ae6d-ab5ac73cdc3b");
        }
    }
}
