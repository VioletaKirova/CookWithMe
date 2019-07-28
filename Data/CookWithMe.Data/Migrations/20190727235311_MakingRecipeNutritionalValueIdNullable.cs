using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class MakingRecipeNutritionalValueIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NutritionalValueId",
                table: "Recipes",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NutritionalValueId",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
