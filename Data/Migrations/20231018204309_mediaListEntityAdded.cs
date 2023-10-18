using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class mediaListEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Products_ProductId",
                table: "Media");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "MediaList");

            migrationBuilder.RenameIndex(
                name: "IX_Media_ProductId",
                table: "MediaList",
                newName: "IX_MediaList_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaList",
                table: "MediaList",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bab92b10-9cd3-45df-a42c-f18ccb37e3d1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c43e6df3-2389-4c9d-ae58-638d454adc7c");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaList_Products_ProductId",
                table: "MediaList",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaList_Products_ProductId",
                table: "MediaList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaList",
                table: "MediaList");

            migrationBuilder.RenameTable(
                name: "MediaList",
                newName: "Media");

            migrationBuilder.RenameIndex(
                name: "IX_MediaList_ProductId",
                table: "Media",
                newName: "IX_Media_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Products_ProductId",
                table: "Media",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
