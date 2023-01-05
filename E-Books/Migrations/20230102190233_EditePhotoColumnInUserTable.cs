using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Books.Migrations
{
    public partial class EditePhotoColumnInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_UsersAppId",
                table: "Photos");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UsersAppId",
                table: "Photos",
                column: "UsersAppId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_UsersAppId",
                table: "Photos");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UsersAppId",
                table: "Photos",
                column: "UsersAppId");
        }
    }
}
