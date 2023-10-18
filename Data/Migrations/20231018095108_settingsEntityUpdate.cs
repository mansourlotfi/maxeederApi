using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class settingsEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServicesRitchText",
                table: "Settings",
                newName: "WorkHoursEn");

            migrationBuilder.RenameColumn(
                name: "ServicePictureUrl",
                table: "Settings",
                newName: "WorkHours");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Settings",
                newName: "FooterTextEn");

            migrationBuilder.RenameColumn(
                name: "ContactUsRitchText",
                table: "Settings",
                newName: "AddressEn");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "29a1c05a-3db3-4820-b6d9-755a7607ef0c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2fac57d1-a091-4865-a03e-0c3003ace19b");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkHoursEn",
                table: "Settings",
                newName: "ServicesRitchText");

            migrationBuilder.RenameColumn(
                name: "WorkHours",
                table: "Settings",
                newName: "ServicePictureUrl");

            migrationBuilder.RenameColumn(
                name: "FooterTextEn",
                table: "Settings",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AddressEn",
                table: "Settings",
                newName: "ContactUsRitchText");

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
    }
}
