using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityUpdatedForOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomUserRoles_AspNetUsers_UserId",
                table: "CustomUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_CustomUserRoles_UserId",
                table: "CustomUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomUserRoles");

            migrationBuilder.AddColumn<string>(
                name: "RandomeCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "dadffe1c-4f34-4077-be82-5c497b25ae8d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c6350dcc-0fda-43e2-b979-81a6de1cf0ad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomeCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d8695da5-a16d-4ec8-8884-5908185c2027");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "360be09d-ad7c-463e-bd91-0d090fcfab42");

            migrationBuilder.CreateIndex(
                name: "IX_CustomUserRoles_UserId",
                table: "CustomUserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomUserRoles_AspNetUsers_UserId",
                table: "CustomUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
