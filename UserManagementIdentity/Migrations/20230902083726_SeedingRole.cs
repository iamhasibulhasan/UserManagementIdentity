using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementIdentity.Migrations
{
    public partial class SeedingRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "65e0f11f-4d42-4448-89d2-29a93c050477", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "906beac2-cb3e-4fd7-a06f-441f520a0128", "3", "HR", "HR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b7bde20e-de8e-48d4-9975-41785184f835", "2", "Customer", "User" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65e0f11f-4d42-4448-89d2-29a93c050477");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "906beac2-cb3e-4fd7-a06f-441f520a0128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7bde20e-de8e-48d4-9975-41785184f835");
        }
    }
}
