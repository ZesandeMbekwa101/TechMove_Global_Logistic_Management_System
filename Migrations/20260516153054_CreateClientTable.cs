using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMove_Global_Logistic_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblClients",
                columns: table => new
                {
                    client_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    last_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phone_Number = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    email_Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    region = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    country = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClients", x => x.client_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblClients");
        }
    }
}
