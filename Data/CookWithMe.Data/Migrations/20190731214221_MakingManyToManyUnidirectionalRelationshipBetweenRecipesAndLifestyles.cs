using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class MakingManyToManyUnidirectionalRelationshipBetweenRecipesAndLifestyles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Lifestyles_LifestyleId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_LifestyleId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "LifestyleId",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "RecipeLifestyles",
                columns: table => new
                {
                    RecipeId = table.Column<string>(nullable: false),
                    LifestyleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeLifestyles", x => new { x.RecipeId, x.LifestyleId });
                    table.ForeignKey(
                        name: "FK_RecipeLifestyles_Lifestyles_LifestyleId",
                        column: x => x.LifestyleId,
                        principalTable: "Lifestyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeLifestyles_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeLifestyles_LifestyleId",
                table: "RecipeLifestyles",
                column: "LifestyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeLifestyles");

            migrationBuilder.AddColumn<int>(
                name: "LifestyleId",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_LifestyleId",
                table: "Recipes",
                column: "LifestyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Lifestyles_LifestyleId",
                table: "Recipes",
                column: "LifestyleId",
                principalTable: "Lifestyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
