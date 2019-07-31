using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class MakingRecipeRrepTimeAndCookingTimePropertiesInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PreparationTime",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CookingTime",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PreparationTime",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
