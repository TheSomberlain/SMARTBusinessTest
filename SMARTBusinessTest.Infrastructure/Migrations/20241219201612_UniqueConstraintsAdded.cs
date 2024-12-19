using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMARTBusinessTest.Infrastructure.Migrations
{
    public partial class UniqueConstraintsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Facility_Name",
                table: "Facility",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Equipment_Name",
                table: "Equipment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Facility_Code",
                table: "Facility",
                column: "Facility_Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Facility_Name",
                table: "Facility",
                column: "Facility_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Equipment_Code",
                table: "Equipment",
                column: "Equipment_Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Equipment_Name",
                table: "Equipment",
                column: "Equipment_Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Facility_Facility_Code",
                table: "Facility");

            migrationBuilder.DropIndex(
                name: "IX_Facility_Facility_Name",
                table: "Facility");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_Equipment_Code",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_Equipment_Name",
                table: "Equipment");

            migrationBuilder.AlterColumn<string>(
                name: "Facility_Name",
                table: "Facility",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Equipment_Name",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
