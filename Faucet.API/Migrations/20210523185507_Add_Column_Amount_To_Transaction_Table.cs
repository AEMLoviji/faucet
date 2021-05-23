using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Faucet.API.Migrations
{
    public partial class Add_Column_Amount_To_Transaction_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInUsd",
                table: "Transaction",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 18, 55, 7, 59, DateTimeKind.Utc).AddTicks(7519));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "AmountInUsd",
                table: "Transaction");

            migrationBuilder.UpdateData(
                table: "Balance",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 5, 23, 18, 33, 32, 236, DateTimeKind.Utc).AddTicks(7329));
        }
    }
}
