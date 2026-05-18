using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMove_Global_Logistic_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CreateServiceRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblServiceRequests",
                columns: table => new
                {
                    service_Request_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contract_Id = table.Column<int>(type: "int", nullable: false),
                    service_Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblServiceRequests", x => x.service_Request_Id);
                    table.ForeignKey(
                        name: "FK_tblServiceRequests_tblContracts_contract_Id",
                        column: x => x.contract_Id,
                        principalTable: "tblContracts",
                        principalColumn: "contract_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblServiceRequests_contract_Id",
                table: "tblServiceRequests",
                column: "contract_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblServiceRequests");
        }
    }
}
