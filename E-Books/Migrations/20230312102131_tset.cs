using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Books.Migrations
{
    public partial class tset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartBook_Books_BookId",
                table: "CartBook");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBook_Carts_CartId",
                table: "CartBook");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UsersId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UsersId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartBook",
                table: "CartBook");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "CartBook",
                newName: "CartBooks");

            migrationBuilder.RenameIndex(
                name: "IX_CartBook_CartId",
                table: "CartBooks",
                newName: "IX_CartBooks_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Damascus,Syria");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 12, 13, 21, 31, 745, DateTimeKind.Local).AddTicks(3749),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 8, 16, 2, 27, 557, DateTimeKind.Local).AddTicks(3767));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks",
                columns: new[] { "BookId", "CartId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Books_BookId",
                table: "CartBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Carts_CartId",
                table: "CartBooks",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Books_BookId",
                table: "CartBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Carts_CartId",
                table: "CartBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks");

            migrationBuilder.RenameTable(
                name: "CartBooks",
                newName: "CartBook");

            migrationBuilder.RenameIndex(
                name: "IX_CartBooks_CartId",
                table: "CartBook",
                newName: "IX_CartBook_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Damascus,Syria",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBook",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 8, 16, 2, 27, 557, DateTimeKind.Local).AddTicks(3767),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 12, 13, 21, 31, 745, DateTimeKind.Local).AddTicks(3749));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartBook",
                table: "CartBook",
                columns: new[] { "BookId", "CartId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UsersId",
                table: "Orders",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartBook_Books_BookId",
                table: "CartBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBook_Carts_CartId",
                table: "CartBook",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UsersId",
                table: "Orders",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
