using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListingSql.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01ee07ff-3d20-4623-a4b1-2531a2af4f4d", "9af7d0c1-0ce8-449a-bf45-83aa36e2086c", "User", "USER" },
                    { "8d5d1c64-65cd-4d7a-88b6-64909842722d", "0629047f-10ba-4a7d-8d13-e883c6d81175", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01ee07ff-3d20-4623-a4b1-2531a2af4f4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d5d1c64-65cd-4d7a-88b6-64909842722d");
        }
    }
}
