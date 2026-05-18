using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMove_Global_Logistic_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyCodeToServiceRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency_Code",
                table: "tblServiceRequests",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency_Code",
                table: "tblServiceRequests");
        }
    }
}
