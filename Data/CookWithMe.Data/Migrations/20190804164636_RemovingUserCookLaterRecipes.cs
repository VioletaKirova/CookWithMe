using Microsoft.EntityFrameworkCore.Migrations;

namespace CookWithMe.Data.Migrations
{
    public partial class RemovingUserCookLaterRecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCookLaterRecipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCookLaterRecipes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RecipeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCookLaterRecipes", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_UserCookLaterRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCookLaterRecipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCookLaterRecipes_RecipeId",
                table: "UserCookLaterRecipes",
                column: "RecipeId");
        }
    }
}
