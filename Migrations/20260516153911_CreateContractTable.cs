using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMove_Global_Logistic_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CreateContractTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSupportDocuments",
                columns: table => new
                {
                    support_Doc_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSupportDocuments", x => x.support_Doc_Id);
                });

            migrationBuilder.CreateTable(
                name: "tblContracts",
                columns: table => new
                {
                    contract_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Support_Doc_Id = table.Column<int>(type: "int", nullable: true),
                    start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    contract_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    service_Level = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblContracts", x => x.contract_Id);
                    table.ForeignKey(
                        name: "FK_tblContracts_tblSupportDocuments_Support_Doc_Id",
                        column: x => x.Support_Doc_Id,
                        principalTable: "tblSupportDocuments",
                        principalColumn: "support_Doc_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblContracts_Support_Doc_Id",
                table: "tblContracts",
                column: "Support_Doc_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblContracts");

            migrationBuilder.DropTable(
                name: "tblSupportDocuments");
        }
    }
}
