using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Books.Migrations
{
    public partial class PhotosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo");

            migrationBuilder.DropForeignKey(
                name: "FK_tUserTokens_Users_UserId",
                table: "tUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tUserTokens",
                table: "tUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "tUserTokens",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_UserId",
                table: "Photos",
                newName: "IX_Photos_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 23, 12, 1, 10, 469, DateTimeKind.Utc).AddTicks(247),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 21, 21, 27, 25, 169, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "tUserTokens");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UserId",
                table: "Photo",
                newName: "IX_Photo_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedOn",
                table: "CartBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 21, 21, 27, 25, 169, DateTimeKind.Local).AddTicks(1112),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 23, 12, 1, 10, 469, DateTimeKind.Utc).AddTicks(247));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tUserTokens",
                table: "tUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tUserTokens_Users_UserId",
                table: "tUserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
