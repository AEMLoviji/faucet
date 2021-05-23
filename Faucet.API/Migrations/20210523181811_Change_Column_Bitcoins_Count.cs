using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Faucet.API.Migrations
{
    public partial class Change_Column_Bitcoins_Count : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BitcoinsCount",
                table: "Balance",
                newName: "BitcoinsAmount");

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 18, 18, 10, 677, DateTimeKind.Utc).AddTicks(7300));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BitcoinsAmount",
                table: "Balance",
                newName: "BitcoinsCount");

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 18, 5, 37, 22, DateTimeKind.Utc).AddTicks(9410));
        }
    }
}
