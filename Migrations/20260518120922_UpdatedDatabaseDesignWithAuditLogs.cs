using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMove_Global_Logistic_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDatabaseDesignWithAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblContracts_tblSupportDocuments_Support_Doc_Id",
                table: "tblContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblServiceRequests_tblContracts_contract_Id",
                table: "tblServiceRequests");

            migrationBuilder.RenameColumn(
                name: "Support_Doc_Id",
                table: "tblContracts",
                newName: "support_Doc_Id");

            migrationBuilder.RenameIndex(
                name: "IX_tblContracts_Support_Doc_Id",
                table: "tblContracts",
                newName: "IX_tblContracts_support_Doc_Id");

            migrationBuilder.RenameColumn(
                name: "last_Name",
                table: "tblClients",
                newName: "contact_Person");

            migrationBuilder.RenameColumn(
                name: "first_Name",
                table: "tblClients",
                newName: "client_Name");

            migrationBuilder.AlterColumn<string>(
                name: "file_Path",
                table: "tblSupportDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "file_Name",
                table: "tblSupportDocuments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "file_Type",
                table: "tblSupportDocuments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "uploaded_On",
                table: "tblSupportDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tblServiceRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_On",
                table: "tblServiceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_Rate",
                table: "tblServiceRequests",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "usd_Amount",
                table: "tblServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "zar_Amount",
                table: "tblServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "service_Level",
                table: "tblContracts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "contract_Status",
                table: "tblContracts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "admin_Id",
                table: "tblContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "client_Id",
                table: "tblContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "phone_Number",
                table: "tblClients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "admin_Id",
                table: "tblClients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblAdmin",
                columns: table => new
                {
                    admin_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    last_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    user_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_On = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAdmin", x => x.admin_Id);
                });

            migrationBuilder.CreateTable(
                name: "tblAuditLogs",
                columns: table => new
                {
                    audit_Log_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    admin_Id = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ip_Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_On = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAuditLogs", x => x.audit_Log_Id);
                    table.ForeignKey(
                        name: "FK_tblAuditLogs_tblAdmin_admin_Id",
                        column: x => x.admin_Id,
                        principalTable: "tblAdmin",
                        principalColumn: "admin_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblContracts_admin_Id",
                table: "tblContracts",
                column: "admin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblContracts_client_Id",
                table: "tblContracts",
                column: "client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblClients_admin_Id",
                table: "tblClients",
                column: "admin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblAuditLogs_admin_Id",
                table: "tblAuditLogs",
                column: "admin_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblClients_tblAdmin_admin_Id",
                table: "tblClients",
                column: "admin_Id",
                principalTable: "tblAdmin",
                principalColumn: "admin_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblContracts_tblAdmin_admin_Id",
                table: "tblContracts",
                column: "admin_Id",
                principalTable: "tblAdmin",
                principalColumn: "admin_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblContracts_tblClients_client_Id",
                table: "tblContracts",
                column: "client_Id",
                principalTable: "tblClients",
                principalColumn: "client_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblContracts_tblSupportDocuments_support_Doc_Id",
                table: "tblContracts",
                column: "support_Doc_Id",
                principalTable: "tblSupportDocuments",
                principalColumn: "support_Doc_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblServiceRequests_tblContracts_contract_Id",
                table: "tblServiceRequests",
                column: "contract_Id",
                principalTable: "tblContracts",
                principalColumn: "contract_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblClients_tblAdmin_admin_Id",
                table: "tblClients");

            migrationBuilder.DropForeignKey(
                name: "FK_tblContracts_tblAdmin_admin_Id",
                table: "tblContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblContracts_tblClients_client_Id",
                table: "tblContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblContracts_tblSupportDocuments_support_Doc_Id",
                table: "tblContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblServiceRequests_tblContracts_contract_Id",
                table: "tblServiceRequests");

            migrationBuilder.DropTable(
                name: "tblAuditLogs");

            migrationBuilder.DropTable(
                name: "tblAdmin");

            migrationBuilder.DropIndex(
                name: "IX_tblContracts_admin_Id",
                table: "tblContracts");

            migrationBuilder.DropIndex(
                name: "IX_tblContracts_client_Id",
                table: "tblContracts");

            migrationBuilder.DropIndex(
                name: "IX_tblClients_admin_Id",
                table: "tblClients");

            migrationBuilder.DropColumn(
                name: "file_Type",
                table: "tblSupportDocuments");

            migrationBuilder.DropColumn(
                name: "uploaded_On",
                table: "tblSupportDocuments");

            migrationBuilder.DropColumn(
                name: "created_On",
                table: "tblServiceRequests");

            migrationBuilder.DropColumn(
                name: "exchange_Rate",
                table: "tblServiceRequests");

            migrationBuilder.DropColumn(
                name: "usd_Amount",
                table: "tblServiceRequests");

            migrationBuilder.DropColumn(
                name: "zar_Amount",
                table: "tblServiceRequests");

            migrationBuilder.DropColumn(
                name: "admin_Id",
                table: "tblContracts");

            migrationBuilder.DropColumn(
                name: "client_Id",
                table: "tblContracts");

            migrationBuilder.DropColumn(
                name: "admin_Id",
                table: "tblClients");

            migrationBuilder.RenameColumn(
                name: "support_Doc_Id",
                table: "tblContracts",
                newName: "Support_Doc_Id");

            migrationBuilder.RenameIndex(
                name: "IX_tblContracts_support_Doc_Id",
                table: "tblContracts",
                newName: "IX_tblContracts_Support_Doc_Id");

            migrationBuilder.RenameColumn(
                name: "contact_Person",
                table: "tblClients",
                newName: "last_Name");

            migrationBuilder.RenameColumn(
                name: "client_Name",
                table: "tblClients",
                newName: "first_Name");

            migrationBuilder.AlterColumn<string>(
                name: "file_Path",
                table: "tblSupportDocuments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "file_Name",
                table: "tblSupportDocuments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tblServiceRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "service_Level",
                table: "tblContracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "contract_Status",
                table: "tblContracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "phone_Number",
                table: "tblClients",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_tblContracts_tblSupportDocuments_Support_Doc_Id",
                table: "tblContracts",
                column: "Support_Doc_Id",
                principalTable: "tblSupportDocuments",
                principalColumn: "support_Doc_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblServiceRequests_tblContracts_contract_Id",
                table: "tblServiceRequests",
                column: "contract_Id",
                principalTable: "tblContracts",
                principalColumn: "contract_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
