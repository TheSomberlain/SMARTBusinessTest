using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMARTBusinessTest.Infrastructure.Migrations
{
    public partial class ContractTotalAreaAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalEquipmentArea",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEquipmentArea",
                table: "Contract");
        }
    }
}
