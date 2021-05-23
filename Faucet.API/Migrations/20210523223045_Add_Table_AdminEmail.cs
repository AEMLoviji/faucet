using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Faucet.API.Migrations
{
    public partial class Add_Table_AdminEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminEmail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LastSentTransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminEmail", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 22, 30, 45, 328, DateTimeKind.Utc).AddTicks(1302));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminEmail");

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 18, 55, 7, 59, DateTimeKind.Utc).AddTicks(7519));
        }
    }
}
