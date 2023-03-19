using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Books.Migrations
{
    public partial class AddEmailColumnInOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 18, 22, 9, 21, 581, DateTimeKind.Local).AddTicks(8659),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 12, 13, 21, 31, 745, DateTimeKind.Local).AddTicks(3749));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 12, 13, 21, 31, 745, DateTimeKind.Local).AddTicks(3749),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 18, 22, 9, 21, 581, DateTimeKind.Local).AddTicks(8659));
        }
    }
}
