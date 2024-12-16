using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMARTBusinessTest.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Equipment_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Equipment_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipment_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipment_Area = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Equipment_Id);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Facility_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Facility_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Facility_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facility_Area = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Facility_Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Contract_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Contract_FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Contract_Id);
                    table.ForeignKey(
                        name: "FK_Contract_Facility_Contract_FacilityId",
                        column: x => x.Contract_FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Facility_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Unit_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit_EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit_Amount = table.Column<int>(type: "int", nullable: false),
                    Unit_ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Unit_Id);
                    table.ForeignKey(
                        name: "FK_Unit_Contract_Unit_ContractId",
                        column: x => x.Unit_ContractId,
                        principalTable: "Contract",
                        principalColumn: "Contract_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Unit_Equipment_Unit_EquipmentId",
                        column: x => x.Unit_EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Equipment_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Contract_FacilityId",
                table: "Contract",
                column: "Contract_FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Unit_ContractId",
                table: "Unit",
                column: "Unit_ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Unit_EquipmentId",
                table: "Unit",
                column: "Unit_EquipmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Facility");
        }
    }
}
