using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyImageList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaList_Products_ProductId",
                table: "MediaList");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MediaList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "63da370f-07ef-49df-b8eb-8625315e7502");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3cf89651-3233-49cc-8773-b5c4a66f5c29");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaList_Products_ProductId",
                table: "MediaList",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaList_Products_ProductId",
                table: "MediaList");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MediaList",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
