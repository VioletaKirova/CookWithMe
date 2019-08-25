using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class ChangingRecipeSummaryMaxValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Recipes",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Recipes",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000);
        }
    }
}
