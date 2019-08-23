using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class SettingUniqueConstraintToCategoryTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_Title",
                table: "Categories",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Title",
                table: "Categories");
        }
    }
}
